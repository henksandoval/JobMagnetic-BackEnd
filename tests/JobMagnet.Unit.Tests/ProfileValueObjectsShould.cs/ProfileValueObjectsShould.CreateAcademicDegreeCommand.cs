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
    
    [Fact (DisplayName = "Return CreateAcademicDegreeCommand should create AcademicInfo with valid data")]
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
    
    [Fact(DisplayName = "Return FromStrings should parse dates correctly")]
    public void Command_WhenTheFormatProviderIsNull_ShouldAssignTheValueToTheRight_And_ReturnTheSuccessfulResult  ()
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
        command.StartDate.Should().Be(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        command.EndDate.Should().Be(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }
    
    [Fact(DisplayName = "Return AcademicInfo should return empty strings when data is null")]
    public void AcademicInfo_WithNullData_EmptyStringsMustBeReturned()
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
    
    [Fact(DisplayName = "Return FromStrings should parse dates with specific format provider")]
    public void FromStrings_WhenAFormatProviderPassesASpecificIFormatProvider_ItShouldReturnSuccessful()
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
    
    [Theory(DisplayName = "Return FromStrings should throw exception for invalid or null date strings")]
    [InlineData("", "2024-01-01", typeof(FormatException))]
    [InlineData("not-a-date", "2024-01-01", typeof(FormatException))]
    [InlineData("2020-01-01", "not-a-date-either", typeof(FormatException))]
    [InlineData(null, "2024-01-01", typeof(ArgumentNullException))]
    [InlineData(null, null, typeof(ArgumentNullException))]
    public void FromStrings_WithInvalidOrNullDateString_ShouldThrowException(string startDate, string endDate, Type expectedException)
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
            CultureInfo.InvariantCulture);

        // Assert
        act.Should().Throw<Exception>().Which.Should().BeOfType(expectedException);
    }
    
    [Fact(DisplayName = "Return Constructor should initialize properties correctly")]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var guidGenerator = _fixture.Create<IGuidGenerator>();
        var careerHistoryId = new CareerHistoryId(_fixture.Create<Guid>());
        var academicInfo = _fixture.Create<CreateAcademicDegreeCommand.AcademicInfo>();
        var startDate = _fixture.Create<DateTime>();
        var endDate = _fixture.Create<DateTime?>();

        // Act
        var command = new CreateAcademicDegreeCommand(
            guidGenerator,
            careerHistoryId,
            academicInfo,
            startDate,
            endDate);

        // Assert
        command.GuidGenerator.Should().Be(guidGenerator);
        command.CareerHistoryId.Should().Be(careerHistoryId);
        command.Academic.Should().Be(academicInfo);
        command.StartDate.Should().Be(startDate);
        command.EndDate.Should().Be(endDate);
    }
    
    [Fact(DisplayName = "Return ToString should return a non-empty string representing the object")]
    public void ToString_ShouldReturnStringRepresentation()
    {
        // Arrange
        var academicInfo = _fixture.Create<CreateAcademicDegreeCommand.AcademicInfo>();
        var command = _fixture.Create<CreateAcademicDegreeCommand>();

        // Act & Assert
        academicInfo.ToString().Should().Contain(academicInfo.Degree);
        command.ToString().Should().Contain(command.Academic.ToString());
    }
    
}