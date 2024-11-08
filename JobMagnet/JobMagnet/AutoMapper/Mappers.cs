using JobMagnet.Entities;
using JobMagnet.Models;

namespace JobMagnet.AutoMapper;

public static class Mappers
{
    public static AboutModel MapAboutModel(AboutEntity entity)
    {
        return new AboutModel
        {
            Id = entity.Id,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl,
            Text = entity.Text,
            Hobbies = entity.Hobbies,
            Birthday = entity.Birthday,
            WebSite = entity.WebSite,
            PhoneNumber = entity.PhoneNumber,
            City = entity.City,
            Age = entity.Age,
            Degree = entity.Degree,
            Email = entity.Email,
            Freelance = entity.Freelance,
            WorkExperience = entity.WorkExperience,
        };
    }

    public static AboutEntity MapAboutEntity(AboutModel model)
    {
        return new AboutEntity
        {
            Id = model.Id,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            Text = model.Text,
            Hobbies = model.Hobbies,
            Birthday = model.Birthday,
            WebSite = model.WebSite,
            PhoneNumber = model.PhoneNumber,
            City = model.City,
            Age = model.Age,
            Degree = model.Degree,
            Email = model.Email,
            Freelance = model.Freelance,
            WorkExperience = model.WorkExperience,
        };
    }

    public static AboutEntity MapAboutCreate(AboutCreateRequest createRequest)
    {
        return new AboutEntity
        {
            Description = createRequest.Description,
            ImageUrl = createRequest.ImageUrl,
            Text = createRequest.Text,
            Hobbies = createRequest.Hobbies,
            Birthday = createRequest.Birthday,
            WebSite = createRequest.WebSite,
            PhoneNumber = createRequest.PhoneNumber,
            City = createRequest.City,
            Age = createRequest.Age,
            Degree = createRequest.Degree,
            Email = createRequest.Email,
            Freelance = createRequest.Freelance,
            WorkExperience = createRequest.WorkExperience,
        };
    }
    
    // SkillEntity
    public static SkillModel MapSkillModel(SkillEntity entity)
    {
        return new SkillModel
        {
            Id = entity.Id,
            Overview = entity.Overview,
            Rank = entity.Rank,
            Name = entity.Name,
            IconUrl = entity.IconUrl,
        };
    }
    
    public static SkillEntity MapSkillEntity(SkillModel model)
    {
        return new SkillEntity
        {
            Id = model.Id,
            Overview = model.Overview,
            Rank = model.Rank,
            Name = model.Name,
            IconUrl = model.IconUrl,
        };
    }
    public static SkillEntity MapSkillCreate(SkillCreateRequest createRequest)
    {
        return new SkillEntity
        {
            Overview = createRequest.Overview,
            Rank = createRequest.Rank,
            Name = createRequest.Name,
            IconUrl = createRequest.IconUrl,
        };
    }
    
    //SummaryEntity

    public static SummaryEntity MapSummaryCreate(SummaryCreateRequest createRequest)
    {
        return new SummaryEntity
        {
          About = createRequest.About,
          Name = createRequest.Name,
          Introduction = createRequest.Introduction,
        };
    }

    public static SummaryModel MapSummaryModel(SummaryEntity entity)
    {
        return new SummaryModel
        {
            Id = entity.Id,
            About = entity.About,
            Name = entity.Name,
            Introduction = entity.Introduction,
        };
    }
}