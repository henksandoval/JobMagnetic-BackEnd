using System.Net;
using AutoFixture;
using AwesomeAssertions;
using AwesomeAssertions.Execution;
using JobMagnet.Application.Contracts.Commands.CareerHistory;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.CareerHistory;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould
{
    [Fact(DisplayName = "Should return 201 Created with the new career history when payload is valid for an existing profile")]
    public async Task CreateCareerHistory_WhenProfileExistsAndPayloadIsValid()
    {
        // --- Given ---
        _educationCount = 0;
        _workExperienceCount = 0;
        var profile = await SetupProfileAsync();

        var workExperienceBases = GenerateUniqueWorkExperiences(5);
        var educationBases = GenerateUniqueAcademicDegrees(5);
        var careerHistoryBase = _fixture.Build<CareerHistoryBase>()
            .With(x => x.ProfileId, profile.Id.Value)
            .With(x => x.WorkExperiences, workExperienceBases)
            .With(x => x.Education, educationBases)
            .Create();

        var createRequest = _fixture.Build<CareerHistoryCommand>()
            .With(x => x.CareerHistoryData, careerHistoryBase)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/career-history", httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseData = await TestUtilities.DeserializeResponseAsync<CareerHistoryResponse>(response);
            responseData.Should().NotBeNull();

            var locationHeader = response.Headers.Location!.ToString();
            locationHeader.Should().NotBeNull();
            var expectedHeader = $"{RequestUriController}/{profile.Id.Value}/career-history";
            locationHeader.Should().Match(currentHeader =>
                currentHeader.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase)
            );

            responseData.CareerHistoryData.Should().BeEquivalentTo(careerHistoryBase, options => options
                .Excluding(x => x.Education)
                .Excluding(x => x.WorkExperiences)
            );

            var entityCreated = await FindCareerHistoryByIdAsync(profile.Id);

            entityCreated.Should().NotBeNull();
            entityCreated!.WorkExperiences.Should().HaveCount(workExperienceBases.Count);
            entityCreated.AcademicDegree.Should().HaveCount(educationBases.Count);
        }
    }

    [Fact(DisplayName = "Should return 400 Bad Request when career history already exists")]
    public async Task CreateCareerHistory_WhenCareerHistoryAlreadyExists()
    {
        // --- Given ---
        _loadCareerHistory = true;
        var profile = await SetupProfileAsync();

        var workExperienceBases = _fixture.CreateMany<WorkExperienceBase>(3).ToList();
        var educationBases = _fixture.CreateMany<AcademicDegreeBase>(3).ToList();
        var careerHistoryBase = _fixture.Build<CareerHistoryBase>()
            .With(x => x.ProfileId, profile.Id.Value)
            .With(x => x.Introduction, "Another career history attempt")
            .With(x => x.WorkExperiences, workExperienceBases)
            .With(x => x.Education, educationBases)
            .Create();

        var createRequest = _fixture.Build<CareerHistoryCommand>()
            .With(x => x.CareerHistoryData, careerHistoryBase)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/career-history", httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [Fact(DisplayName = "Should return 200 OK with career history when the profile exists and has career history")]
    public async Task GetCareerHistory_WhenProfileExistsAndHasCareerHistory()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _educationCount = 3;
        _workExperienceCount = 2;
        var profile = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profile.Id.Value}/career-history");

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseData = await TestUtilities.DeserializeResponseAsync<CareerHistoryResponse>(response);
            responseData.Should().NotBeNull();
            responseData.CareerHistoryData.ProfileId.Should().Be(profile.Id.Value);
            responseData.CareerHistoryData.Introduction.Should().NotBeNullOrWhiteSpace();
            responseData.CareerHistoryData.Education.Should().HaveCount(_educationCount);
            responseData.CareerHistoryData.WorkExperiences.Should().HaveCount(_workExperienceCount);
        }
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile has no career history")]
    public async Task GetCareerHistory_WhenProfileHasNoCareerHistory()
    {
        // --- Given ---
        _loadCareerHistory = false;
        var profile = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profile.Id.Value}/career-history");

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    [Fact(DisplayName = "Should return 204 No Content when updating career history introduction")]
    public async Task UpdateCareerHistoryIntroduction_WhenProfileAndCareerHistoryExist()
    {
        // --- Given ---
        _loadCareerHistory = true;
        var profile = await SetupProfileAsync();

        var updatedIntroduction = "Updated career journey with new experiences...";
        var careerHistoryBase = _fixture.Build<CareerHistoryBase>()
            .With(x => x.ProfileId, profile.Id.Value)
            .With(x => x.Introduction, updatedIntroduction)
            .Create();

        var command = _fixture.Build<CareerHistoryCommand>()
            .With(x => x.CareerHistoryData, careerHistoryBase)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(command);

        // --- When ---
        var response = await _httpClient.PutAsync($"{RequestUriController}/{profile.Id.Value}/career-history", httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verify the update
            var entityUpdated = await FindCareerHistoryByIdAsync(profile.Id);
            entityUpdated.Should().NotBeNull();
            entityUpdated!.Introduction.Should().Be(updatedIntroduction);
        }
    }

    [Fact(DisplayName = "Should return 204 No Content when deleting career history")]
    public async Task DeleteCareerHistory_WhenProfileAndCareerHistoryExist()
    {
        // --- Given ---
        _loadCareerHistory = true;
        var profile = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{profile.Id.Value}/career-history");

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verify deletion
            var entityDeleted = await FindCareerHistoryByIdAsync(profile.Id);
            entityDeleted.Should().BeNull();
        }
    }

    [Fact(DisplayName = "Should return 201 Created when adding a AcademicDegree")]
    public async Task AddAcademicDegree_WhenCareerHistoryExists()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _educationCount = 2;
        var profile = await SetupProfileAsync();

        var academicDegreeData = _fixture.Build<AcademicDegreeBase>()
            .With(x => x.Degree, "Master of Computer Science")
            .With(x => x.InstitutionName, "Stanford University")
            .With(x => x.InstitutionLocation, "Stanford, CA")
            .With(x => x.StartDate, DateTime.Now.AddYears(-2))
            .With(x => x.EndDate, DateTime.Now)
            .With(x => x.Description, "Advanced degree in Computer Science")
            .Create();

        var command = _fixture.Build<AcademicDegreeCommand>()
            .With(x => x.AcademicDegreeData, academicDegreeData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(command);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/career-history/academic-degree", httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseData = await TestUtilities.DeserializeResponseAsync<AcademicDegreeBase>(response);
            responseData.Should().NotBeNull();
            responseData.Should().BeEquivalentTo(academicDegreeData, options => options
                .Excluding(x => x.Id)
            );

            var entityUpdated = await FindCareerHistoryByIdAsync(profile.Id);
            entityUpdated.Should().NotBeNull();
            entityUpdated!.AcademicDegree.Should().HaveCount(_educationCount + 1);
            entityUpdated.AcademicDegree.Should().Contain(q =>
                q.Degree == academicDegreeData.Degree &&
                q.InstitutionName == academicDegreeData.InstitutionName
            );
        }
    }

    [Fact(DisplayName = "Should return 400 Bad Request when adding duplicate AcademicDegree")]
    public async Task AddAcademicDegree_WhenDuplicateAcademicDegree()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _educationCount = 1;
        var profile = await SetupProfileAsync();

        var existingAcademicDegree = profile.CareerHistory!.AcademicDegree.First();

        var academicDegreeData = _fixture.Build<AcademicDegreeBase>()
            .With(x => x.Degree, existingAcademicDegree.Degree)
            .With(x => x.InstitutionName, existingAcademicDegree.InstitutionName)
            .With(x => x.StartDate, DateTime.Now.AddYears(-1))
            .Create();

        var command = _fixture.Build<AcademicDegreeCommand>()
            .With(x => x.AcademicDegreeData, academicDegreeData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(command);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/career-history/academic-degree", httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [Fact(DisplayName = "Should return 204 No Content when updating a AcademicDegree")]
    public async Task UpdateAcademicDegree_WhenAcademicDegreeExists()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _educationCount = 3;
        var profile = await SetupProfileAsync();
        var academicDegree = profile.CareerHistory!.AcademicDegree.First();

        var updatedData = _fixture.Build<AcademicDegreeBase>()
            .With(x => x.Degree, "Updated Computer Science Degree")
            .With(x => x.InstitutionName, "Updated University Name")
            .With(x => x.InstitutionLocation, "New Location, CA")
            .With(x => x.StartDate, DateTime.Now.AddYears(-3))
            .With(x => x.EndDate, DateTime.Now.AddYears(-1))
            .With(x => x.Description, "Updated description")
            .Create();

        var command = _fixture.Build<AcademicDegreeCommand>()
            .With(x => x.AcademicDegreeData, updatedData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(command);

        // --- When ---
        var response = await _httpClient.PutAsync(
            $"{RequestUriController}/{profile.Id.Value}/career-history/academic-degree/{academicDegree.Id.Value}",
            httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var entityUpdated = await FindCareerHistoryByIdAsync(profile.Id);
            entityUpdated.Should().NotBeNull();

            var updatedAcademicDegree = entityUpdated!.AcademicDegree.FirstOrDefault(q => q.Id == academicDegree.Id);
            updatedAcademicDegree.Should().NotBeNull();
            updatedAcademicDegree!.Degree.Should().Be(updatedData.Degree);
            updatedAcademicDegree.InstitutionName.Should().Be(updatedData.InstitutionName);
        }
    }

    [Fact(DisplayName = "Should return 204 No Content when deleting a AcademicDegree")]
    public async Task DeleteAcademicDegree_WhenAcademicDegreeExists()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _educationCount = 3;
        var profile = await SetupProfileAsync();
        var academicDegreeToDelete = profile.CareerHistory!.AcademicDegree.First();
        var academicDegreeId = academicDegreeToDelete.Id.Value;

        // --- When ---
        var response = await _httpClient.DeleteAsync(
            $"{RequestUriController}/{profile.Id.Value}/career-history/academic-degree/{academicDegreeId}");

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var entityUpdated = await FindCareerHistoryByIdAsync(profile.Id);
            entityUpdated.Should().NotBeNull();
            entityUpdated!.AcademicDegree.Should().HaveCount(_educationCount - 1);
            entityUpdated.AcademicDegree.Should().NotContain(q => q.Id.Value == academicDegreeId);
        }
    }

    [Fact(DisplayName = "Should return 201 Created when adding a work experience")]
    public async Task AddWorkExperience_WhenCareerHistoryExists()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _workExperienceCount = 2;
        var profile = await SetupProfileAsync();

        var workExperienceData = _fixture.Build<WorkExperienceBase>()
            .With(x => x.JobTitle, "Senior Software Engineer")
            .With(x => x.CompanyName, "Tech Innovations Corp")
            .With(x => x.CompanyLocation, "San Francisco, CA")
            .With(x => x.StartDate, DateTime.Now.AddYears(-2))
            .With(x => x.EndDate, (DateTime?)null)
            .With(x => x.Description, "Leading full-stack development initiatives")
            .With(x => x.Responsibilities, new List<string> { "Architecture design", "Team leadership" })
            .Create();

        var command = _fixture.Build<WorkExperienceCommand>()
            .With(x => x.WorkExperienceData, workExperienceData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(command);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/career-history/work-experiences", httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseData = await TestUtilities.DeserializeResponseAsync<WorkExperienceBase>(response);
            responseData.Should().NotBeNull();
            responseData.Should().BeEquivalentTo(workExperienceData, options => options
                .Excluding(x => x.Id)
                .Excluding(x => x.Responsibilities)
            );

            var entityUpdated = await FindCareerHistoryByIdAsync(profile.Id);
            entityUpdated.Should().NotBeNull();
            entityUpdated!.WorkExperiences.Should().HaveCount(_workExperienceCount + 1);
            entityUpdated.WorkExperiences.Should().Contain(w =>
                w.JobTitle == workExperienceData.JobTitle &&
                w.CompanyName == workExperienceData.CompanyName
            );
        }
    }

    [Fact(DisplayName = "Should return 204 No Content when updating a work experience")]
    public async Task UpdateWorkExperience_WhenWorkExperienceExists()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _workExperienceCount = 3;
        var profile = await SetupProfileAsync();
        var workExperience = profile.CareerHistory!.WorkExperiences.First();

        var updatedData = _fixture.Build<WorkExperienceBase>()
            .With(x => x.JobTitle, "Lead Software Architect")
            .With(x => x.CompanyName, "Updated Tech Solutions")
            .With(x => x.CompanyLocation, "Austin, TX")
            .With(x => x.StartDate, DateTime.Now.AddYears(-3))
            .With(x => x.EndDate, DateTime.Now.AddMonths(-6))
            .With(x => x.Description, "Leading enterprise architecture initiatives")
            .Create();

        var command = _fixture.Build<WorkExperienceCommand>()
            .With(x => x.WorkExperienceData, updatedData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(command);

        // --- When ---
        var response = await _httpClient.PutAsync(
            $"{RequestUriController}/{profile.Id.Value}/career-history/work-experiences/{workExperience.Id.Value}",
            httpContent);

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var entityUpdated = await FindCareerHistoryByIdAsync(profile.Id);
            entityUpdated.Should().NotBeNull();

            var updatedWorkExperience = entityUpdated!.WorkExperiences.FirstOrDefault(w => w.Id == workExperience.Id);
            updatedWorkExperience.Should().NotBeNull();
            updatedWorkExperience!.JobTitle.Should().Be(updatedData.JobTitle);
            updatedWorkExperience.CompanyName.Should().Be(updatedData.CompanyName);
            updatedWorkExperience.CompanyLocation.Should().Be(updatedData.CompanyLocation);
        }
    }

    [Fact(DisplayName = "Should return 204 No Content when deleting a work experience")]
    public async Task DeleteWorkExperience_WhenWorkExperienceExists()
    {
        // --- Given ---
        _loadCareerHistory = true;
        _workExperienceCount = 3;
        var profile = await SetupProfileAsync();
        var workExperienceToDelete = profile.CareerHistory!.WorkExperiences.First();
        var workExperienceId = workExperienceToDelete.Id.Value;

        // --- When ---
        var response = await _httpClient.DeleteAsync(
            $"{RequestUriController}/{profile.Id.Value}/career-history/work-experiences/{workExperienceId}");

        // --- Then ---
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var entityUpdated = await FindCareerHistoryByIdAsync(profile.Id);
            entityUpdated.Should().NotBeNull();
            entityUpdated!.WorkExperiences.Should().HaveCount(_workExperienceCount - 1);
            entityUpdated.WorkExperiences.Should().NotContain(w => w.Id.Value == workExperienceId);
        }
    }

    private async Task<CareerHistory?> FindCareerHistoryByIdAsync(ProfileId id)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository
            .WithCareerHistory()
            .WhereCondition(x => x.Id == id)
            .BuildFirstOrDefaultAsync(CancellationToken.None, true);

        return entityCreated?.CareerHistory;
    }

    private List<AcademicDegreeBase> GenerateUniqueAcademicDegrees(int count)
    {
        var set = new HashSet<(string Degree, string Institution)>();
        var list = new List<AcademicDegreeBase>();

        while (list.Count < count)
        {
            var next = _fixture.Create<AcademicDegreeBase>();
            if (!set.Add((next.Degree, next.InstitutionName)!)) continue;
            list.Add(next);
        }

        return list;
    }

    private List<WorkExperienceBase> GenerateUniqueWorkExperiences(int count)
    {
        var set = new HashSet<(string JobTitle, string CompanyName, DateTime StartDate)>();
        var list = new List<WorkExperienceBase>();

        while (list.Count < count)
        {
            var next = _fixture.Create<WorkExperienceBase>();
            if (!set.Add((next.JobTitle, next.CompanyName, next.StartDate)!)) continue;
            list.Add(next);
        }

        return list;
    }
}