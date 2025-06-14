﻿using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProfileEntity : SoftDeletableEntity<long>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public virtual ResumeEntity? Resume { get; set; }
    public virtual SkillEntity? Skill { get; set; }
    public virtual ServiceEntity? Services { get; set; }
    public virtual SummaryEntity? Summary { get; set; }
    public virtual ICollection<TalentEntity> Talents { get; set; } = new HashSet<TalentEntity>();
    public virtual ICollection<PortfolioGalleryEntity> PortfolioGallery { get; set; } =
        new HashSet<PortfolioGalleryEntity>();
    public virtual ICollection<TestimonialEntity> Testimonials { get; set; } = new HashSet<TestimonialEntity>();
    public virtual ICollection<PublicProfileIdentifierEntity> PublicProfileIdentifiers { get; set; } =
        new HashSet<PublicProfileIdentifierEntity>();

    public void AddPublicProfileIdentifier(PublicProfileIdentifierEntity publicIdentifierEntity)
    {
        ArgumentNullException.ThrowIfNull(publicIdentifierEntity);

        PublicProfileIdentifiers.Add(publicIdentifierEntity);
    }
}