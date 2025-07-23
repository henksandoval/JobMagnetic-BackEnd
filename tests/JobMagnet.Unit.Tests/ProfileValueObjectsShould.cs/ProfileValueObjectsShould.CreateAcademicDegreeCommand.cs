using AwesomeAssertions;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

namespace JobMagnet.Unit.Tests.ProfileValueObjectsShould.cs;

public class ProfileValueObjectsShould
{
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
        const string degree = "  Bachelor of Science   ";
        const string institutionName = "University of Technology";
        const string institutionLocation = "City of Innovation";
        const string description = "  A comprehensive study of computer science. ";

        // Act
        var academicInfo = new CreateAcademicDegreeCommand.AcademicInfo(
            degree,
            institutionName,
            institutionLocation,
            description);

        // Assert
        academicInfo.Degree.Should().Be("Bachelor of Science");
        academicInfo.InstitutionName.Should().Be("University of Technology");
        academicInfo.InstitutionLocation.Should().Be("City of Innovation");
        academicInfo.Description.Should().Be("A comprehensive study of computer science.");
    }
}