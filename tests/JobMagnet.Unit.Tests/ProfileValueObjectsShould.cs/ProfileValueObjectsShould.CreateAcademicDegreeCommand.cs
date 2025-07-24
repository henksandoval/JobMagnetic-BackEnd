using System.Globalization;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;

namespace JobMagnet.Unit.Tests.ProfileValueObjectsShould.cs;

public class ProfileValueObjectsShould
{
    private readonly IFixture _fixture = new Fixture();
    
    public ProfileValueObjectsShould()
    {
        _fixture.Register<IGuidGenerator>(() => new SequentialGuidGenerator());
    }
    
    [Theory (DisplayName = "Return Academic Info should throw ArgumentException for invalid fields")]
    [InlineData(null, "some institution", "some location", "some description", "degree")]
    [InlineData("", "some institution", "some location", "some description", "degree")]
    [InlineData(" ", "some institution", "some location", "some description", "degree")]
    [InlineData("some degree", null, "some location", "some description", "institutionName")]
    [InlineData("some degree", "", "some location", "some description", "institutionName")]
    [InlineData("some degree", " ", "some location", "some description", "institutionName")]
    [InlineData("some degree", "some institution", null, "some description", "institutionLocation")]
    [InlineData("some degree", "some institution", "", "some description", "institutionLocation")]
    [InlineData("some degree", "some institution", " ", "some description", "institutionLocation")]
    [InlineData("some degree", "some institution", "some location", null, "description")]
    [InlineData("some degree", "some institution", "some location", "", "description")]
    [InlineData("some degree", "some institution", "some location", " ", "description")]
    public void ValidateAcademicInfo_WithInValidData_ShouldThrowException( string degree,
        string institutionName,
        string institutionLocation,
        string description,
        string expectedParamName)
    {
        // Act
        var act = () => new CreateAcademicDegreeCommand.AcademicInfo(
            degree, 
            institutionName, 
            institutionLocation, 
            description, 
            true);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName(expectedParamName);
    }
    
    [Fact (DisplayName = "CreateAcademicDegreeCommand should create AcademicInfo with valid data")]
    public void Command_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var academicInfo = _fixture.Build<CreateAcademicDegreeCommand.AcademicInfo>()
            .With(x => x.Degree, "Bachelor of Science")
            .With(x => x.InstitutionName, "University of Technology")
            .With(x => x.InstitutionLocation, "City of Innovation")
            .With(x => x.Description, "A comprehensive study of computer science.")
            .Create();

        // Assert
        academicInfo.Degree.Should().Be("Bachelor of Science");
        academicInfo.InstitutionName.Should().Be("University of Technology");
        academicInfo.InstitutionLocation.Should().Be("City of Innovation");
        academicInfo.Description.Should().Be("A comprehensive study of computer science.");
    }
    
    [Fact(DisplayName = "CreateAcademicDegreeCommand should create with valid data and null format provider")]
    public void Command_WithValidDataAndNullFormatProvider_ShouldCreateSuccessfully()
    {
        // Arrange
        var guidGenerator = _fixture.Create<IGuidGenerator>();
        var careerHistoryId = new CareerHistoryId(_fixture.Create<Guid>());
        var academicInfo = _fixture.Build<CreateAcademicDegreeCommand.AcademicInfo>()
            .With(x => x.Degree, "  Bachelor of Science   ")
            .With(x => x.InstitutionName, "University of Technology")
            .With(x => x.InstitutionLocation, "City of Innovation")
            .With(x => x.Description, "  A comprehensive study of computer science. ")
            .Create();

        // Act
        var command = CreateAcademicDegreeCommand.FromStrings(
            guidGenerator,
            careerHistoryId,
            academicInfo,
            "2020-01-01",
            "2024-01-01",
            null);

        // Assert
        command.Should().NotBeNull();
        command.GuidGenerator.Should().Be(guidGenerator);
        command.CareerHistoryId.Should().Be(careerHistoryId);
        command.Academic.Should().Be(academicInfo);
        command.StartDate.Should().Be(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        command.EndDate.Should().Be(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }
    
    [Fact(DisplayName = "AcademicInfo should not throw when validations are disabled")]
    public void AcademicInfo_WithInvalidDataAndValidationsDisabled_ShouldCreateEmptyStrings()
    {
        // Act
        var academicInfo = () => new CreateAcademicDegreeCommand.AcademicInfo(null, null, null, null, applyValidations: false);
    
        // Assert
        var academic = academicInfo.Should().NotThrow().Subject;
    
        // Assert
        academic.Degree.Should().Be(string.Empty);
        academic.InstitutionName.Should().Be(string.Empty);
        academic.InstitutionLocation.Should().Be(string.Empty);
        academic.Description.Should().Be(string.Empty);
    }
    
    [Theory(DisplayName = "FromStrings should handle null or whitespace EndDate")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FromStrings_WithNullOrWhitespaceEndDate_ShouldResultInNullEndDate(string endDateString)
    {
        // Arrange
        var guidGenerator = _fixture.Create<IGuidGenerator>();
        var careerHistoryId = new CareerHistoryId(_fixture.Create<Guid>());
        var academic = _fixture.Create<CreateAcademicDegreeCommand.AcademicInfo>();
    
        // Act
        var command = CreateAcademicDegreeCommand.FromStrings(
            guidGenerator,
            careerHistoryId,
            academic,
            "2020-01-01",
            endDateString,
            null);
    
        // Assert:
        command.EndDate.Should().BeNull();
    }
    
    [Fact(DisplayName = "FromStrings should use provided format provider")]
    public void FromStrings_WithSpecificFormatProvider_ShouldParseCorrectly()
    {
        // Arrange
        var guidGenerator = _fixture.Create<IGuidGenerator>();
        var careerHistoryId = new CareerHistoryId(_fixture.Create<Guid>());
        var academic = _fixture.Create<CreateAcademicDegreeCommand.AcademicInfo>();
        var spanishCulture = new CultureInfo("es-ES");

        // Act
        var command = CreateAcademicDegreeCommand.FromStrings(
            guidGenerator,
            careerHistoryId,
            academic,
            "15/05/2020",
            "30/06/2024",
            spanishCulture);

        // Assert
        command.StartDate.Should().Be(new DateTime(2020, 5, 15, 0, 0, 0, DateTimeKind.Utc));
        command.EndDate.Should().Be(new DateTime(2024, 6, 30, 0, 0, 0, DateTimeKind.Utc));
    }
    
    [Theory(DisplayName = "FromStrings should throw FormatException for invalid date strings")]
    [InlineData("not-a-date", "2024-01-01")]
    [InlineData("2020-01-01", "not-a-date-either")]
    public void FromStrings_WithInvalidDateString_ShouldThrowFormatException(string startDate, string endDate)
    {
        // Arrange
        var guidGenerator = _fixture.Create<IGuidGenerator>();
        var careerHistoryId = new CareerHistoryId(_fixture.Create<Guid>());
        var academic = _fixture.Create<CreateAcademicDegreeCommand.AcademicInfo>();

        // Act
        var act = () => CreateAcademicDegreeCommand.FromStrings(
            guidGenerator,
            careerHistoryId,
            academic,
            startDate,
            endDate,
            null);

        // Assert
        act.Should().Throw<FormatException>();
    }
}