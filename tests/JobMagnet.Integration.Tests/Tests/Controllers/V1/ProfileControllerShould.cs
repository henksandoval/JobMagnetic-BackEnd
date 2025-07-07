using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Queries.Profile;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Application.UseCases.CvParser.Responses;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;
using JobMagnet.Host.ViewModels.Profile;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Fixtures.Customizations;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/profile";
    private const string InvalidId = "100";
    private readonly DeterministicClock _clock;
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly SequentialGuidGenerator _guidGenerator;
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;
    private bool _loadSkillSet = true;
    private int _contactInfoCount;
    private int _talentsCount;
    private int _testimonialsCount;
    private int _educationCount;
    private int _workExperienceCount;
    private int _skillsCount;
    private int _projectCount;

    public ProfileControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _guidGenerator = new SequentialGuidGenerator();
        _clock = new DeterministicClock();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid Name is provided")]
    public async Task ReturnRecord_WhenValidNameProvidedAsync()
    {
        // --- Given ---
        _contactInfoCount = 3;
        _talentsCount = 8;
        _testimonialsCount = 6;
        _educationCount = 4;
        _workExperienceCount = 2;
        _skillsCount = 12;
        _projectCount = 3;
        var entity = await SetupProfileAsync();
        var publicProfile = await SetupPublicProfileAsync(entity);
        var queryParameters = new Dictionary<string, string?>
        {
            { nameof(ProfileQueryParameters.ProfileSlug), publicProfile.ProfileSlugUrl }
        };

        var requestUrl = QueryHelpers.AddQueryString(RequestUriController, queryParameters);

        // --- When ---
        var response = await _httpClient.GetAsync(requestUrl);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileViewModel>(response);
        responseData.Should().NotBeNull();
        responseData.Should().BeAssignableTo<ProfileViewModel>();

        responseData.About.Should().NotBeNull();
        responseData.SkillSet.Should().NotBeNull();
        responseData.SkillSet.SkillDetails.Length.Should().Be(_skillsCount);
        responseData.Summary.Should().NotBeNull();
        responseData.Summary.Education.AcademicBackground.Length.Should().Be(_educationCount);
        responseData.Summary.WorkExperience.Position.Length.Should().Be(_workExperienceCount);
        responseData.PersonalData.Should().NotBeNull();
        responseData.PersonalData.Professions.Should().NotBeNull();
        responseData.PersonalData.SocialNetworks.Length.Should().Be(_contactInfoCount);
        responseData.Testimonials!.Length.Should().Be(_testimonialsCount);
        responseData.Project!.Length.Should().Be(_projectCount);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // --- Given ---
        var createRequest = _fixture.Build<ProfileCommand>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileResponse>(response);
        responseData.Should().NotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.Should().NotBeNull();
        var expectedHeader = $"{RequestUriController}/{responseData.Id}";
        locationHeader.Should().Match(currentHeader =>
            currentHeader.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase)
        );

        var entityCreated = await FindProfileByIdAsync(responseData.Id);

        entityCreated.Should().NotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid ID is provided")]
    public async Task ReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // --- Given ---
        var entity = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id.Value}");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileResponse>(response);
        responseData.Should().NotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when GET request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        _ = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{InvalidId}");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Return 204 when a valid PUT request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // --- Given ---
        var entity = await SetupProfileAsync();
        var updateRequest = _fixture.Build<ProfileCommand>()
            .Create();

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id.Value}", updateRequest);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedProfile = await FindProfileByIdAsync(entity.Id.Value);
        updatedProfile.Should().NotBeNull();
        updatedProfile.Should().BeEquivalentTo(updateRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when a PUT request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ProfileCommand>().Create();

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{InvalidId}", updatedEntity);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Create a new profile and return 201 when a valid CV file is loaded")]
    public async Task ReturnCreatedAndPersistData_WhenIsValidCVFileAsync()
    {
        // --- Given ---
        const string fileName = "cv_laura_gomez.pdf";
        const string cvContent = StaticCustomizations.CvContent;
        var fileBytes = Encoding.UTF8.GetBytes(cvContent);
        var memoryStream = new MemoryStream(fileBytes);

        using var multipartContent = new MultipartFormDataContent();
        var fileStreamContent = new StreamContent(memoryStream);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Text.Plain);

        multipartContent.Add(fileStreamContent, "cvFile", fileName);

        // --- When ---
        var response = await _httpClient.PostAsync(RequestUriController + "/create-from-cv", multipartContent);

        // --- Then ---
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseData = await response.Content.ReadFromJsonAsync<CreateProfileResponse>();
        responseData.Should().NotBeNull();
        responseData.UserEmail.Should().NotBeNullOrEmpty();
        responseData.ProfileUrl.Should().NotBeNullOrEmpty();
        responseData.UserEmail.Should().Be("laura.gomez.dev@example.net");
        responseData.ProfileUrl.Should().StartWith("laura-gomez-");
    }

    private async Task<Profile> SetupProfileAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<Profile> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<Profile>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var context = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithTalents(_talentsCount)
            .WithProjects(_projectCount)
            .WithSummary()
            .WithEducation(_educationCount)
            .WithWorkExperience(_workExperienceCount)
            .WithTestimonials(_testimonialsCount)
            .WithContactInfo(_testFixture.SeededContactTypes.ToArray(), _contactInfoCount)
            .WithSkillSet(_loadSkillSet)
            .WithSkills(_testFixture.SeededSkillTypes.ToArray(), _skillsCount)
            .Build();

        var skillTypes = _skillsCount > 0 ? entity.GetSkills().Select(s => s.SkillType) : [];
        foreach (var skillType in skillTypes.Distinct())
        {
            if (context.Entry(skillType.Category).State == EntityState.Detached)
                context.Entry(skillType.Category).State = EntityState.Unchanged;

            if (context.Entry(skillType).State == EntityState.Detached)
                context.Entry(skillType).State = EntityState.Unchanged;
        }

        var contactTypes = _contactInfoCount > 0 ? entity.ProfileHeader?.ContactInfo?.Select(s => s.ContactType).Distinct() : [];
        foreach (var contactType in contactTypes!.Distinct())
        {
            if (context.Entry(contactType).State == EntityState.Detached)
                context.Entry(contactType).State = EntityState.Unchanged;
        }

        await commandRepository.CreateAsync(entity, CancellationToken.None);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return entity;
    }

    private async Task<VanityUrl> SetupPublicProfileAsync(Profile profile)
    {
        var slugGenerator = new Mock<IProfileSlugGenerator>();
        const string slug = "alexander-gonzalez-6ca66d";
        profile.AddVanityUrl(_guidGenerator, slug);
        slugGenerator
            .Setup(sg => sg.GenerateProfileSlug(It.IsAny<Profile>()))
            .Returns(slug);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository =
            scope.ServiceProvider.GetRequiredService<IGenericCommandRepository<VanityUrl>>();

        await commandRepository.CreateRangeAsync(profile.VanityUrls, CancellationToken.None);
        await commandRepository.SaveChangesAsync();

        return profile.VanityUrls.FirstOrDefault() ??
               throw new InvalidOperationException("Public profile identifier was not created.");
    }

    private async Task<Profile?> FindProfileByIdAsync(Guid id)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository.GetByIdAsync(new ProfileId(id), CancellationToken.None);
        return entityCreated;
    }
}