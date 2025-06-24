using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Queries.Profile;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Application.UseCases.CvParser.Responses;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;
using JobMagnet.Host.ViewModels.Profile;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Fixtures.Customizations;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ProfileControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/profile";
    private const string InvalidId = "100";
    private const int ContactInfoCount = 3;
    private const int TalentsCount = 8;
    private const int ProjectCount = 3;
    private const int TestimonialsCount = 6;
    private const int EducationCount = 4;
    private const int WorkExperienceCount = 2;
    private const int SkillDetailsCount = 12;
    private const int ServiceDetailsCount = 3;

    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ProfileControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid Name is provided")]
    public async Task ReturnRecord_WhenValidNameProvidedAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();
        var publicProfile = await SetupPublicProfileAsync(entity);
        var queryParameters = new Dictionary<string, string?>
        {
            { nameof(ProfileQueryParameters.ProfileSlug), publicProfile.ProfileSlugUrl }
        };

        var requestUrl = QueryHelpers.AddQueryString(RequestUriController, queryParameters!);

        // --- When ---
        var response = await _httpClient.GetAsync(requestUrl);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileViewModel>(response);
        responseData.ShouldNotBeNull();
        responseData.ShouldBeAssignableTo<ProfileViewModel>();

        responseData.About.ShouldNotBeNull();
        responseData.SkillSet.ShouldNotBeNull();
        responseData.SkillSet.SkillDetails.Length.ShouldBe(SkillDetailsCount);
        responseData.Summary.ShouldNotBeNull();
        responseData.Summary.Education.AcademicBackground.Length.ShouldBe(EducationCount);
        responseData.Summary.WorkExperience.Position.Length.ShouldBe(WorkExperienceCount);
        responseData.PersonalData.ShouldNotBeNull();
        responseData.PersonalData.SocialNetworks.Length.ShouldBe(ContactInfoCount);
        responseData.Testimonials!.Length.ShouldBe(TestimonialsCount);
        responseData.Project!.Length.ShouldBe(ProjectCount);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();
        var createRequest = _fixture.Build<ProfileCommand>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileResponse>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository.GetByIdAsync(responseData.Id, CancellationToken.None);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid ID is provided")]
    public async Task ReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileResponse>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when GET request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        _ = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{InvalidId}");

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Return 204 when a valid PUT request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();
        var updateRequest = _fixture.Build<ProfileCommand>()
            .Create();

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updateRequest);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id, CancellationToken.None);
        dbEntity.ShouldNotBeNull();
        dbEntity.Should().BeEquivalentTo(updateRequest, options => options.ExcludingMissingMembers());
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
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Create a new profile and return 201 when a valid CV file is loaded")]
    public async Task ReturnCreatedAndPersistData_WhenIsValidCVFileAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();

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
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var responseData = await response.Content.ReadFromJsonAsync<CreateProfileResponse>();
        responseData.ShouldNotBeNull();
        responseData.UserEmail.ShouldNotBeNullOrEmpty();
        responseData.ProfileUrl.ShouldNotBeNullOrEmpty();
        responseData.ProfileId.ShouldBeGreaterThan(0);
        responseData.UserEmail.Should().Be("laura.gomez.dev@example.net");
        responseData.ProfileUrl.Should().StartWith("laura-gomez-");
    }

    private async Task<ProfileEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<ProfileEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ProfileEntity>>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithContactInfo(ContactInfoCount)
            .WithTalents(TalentsCount)
            .WithProjects(ProjectCount)
            .WithSummary()
            .WithEducation(EducationCount)
            .WithWorkExperience(WorkExperienceCount)
            .WithSkillSet()
            .WithSkills(SkillDetailsCount)
            .WithTestimonials(TestimonialsCount)
            .Build();

        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }

    private async Task<PublicProfileIdentifierEntity> SetupPublicProfileAsync(ProfileEntity profile)
    {
        var slugGenerator = new Mock<IProfileSlugGenerator>();
        const string slug = "alexander-gonzalez-6ca66d";
        profile.VanityUrls.AddPublicProfileIdentifier(slug);
        slugGenerator
            .Setup(sg => sg.GenerateProfileSlug(It.IsAny<ProfileEntity>()))
            .Returns(slug);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository =
            scope.ServiceProvider.GetRequiredService<ICommandRepository<PublicProfileIdentifierEntity>>();

        await commandRepository.CreateRangeAsync(profile.PublicProfileIdentifiers, CancellationToken.None);
        await commandRepository.SaveChangesAsync();

        return profile.PublicProfileIdentifiers.FirstOrDefault() ??
               throw new InvalidOperationException("Public profile identifier was not created.");
    }
}