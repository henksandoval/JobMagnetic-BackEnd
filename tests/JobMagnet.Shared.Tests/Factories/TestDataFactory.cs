using System.Collections.ObjectModel;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Data;
using JobMagnet.Shared.Tests.Abstractions;

namespace JobMagnet.Shared.Tests.Factories;

public class TestDataFactory
{
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;
    private readonly Lazy<IReadOnlyList<ContactType>> _predefinedContactTypes;
    private readonly Lazy<IReadOnlyList<SkillType>> _predefinedSkillTypes;
    public IReadOnlyList<ContactType> PredefinedContactTypes => _predefinedContactTypes.Value;
    public IReadOnlyList<SkillType> PredefinedSkillTypes => _predefinedSkillTypes.Value;

    public TestDataFactory()
    {
        _clock = new DeterministicClock();
        _guidGenerator = new SequentialGuidGenerator();

        _predefinedContactTypes = new Lazy<IReadOnlyList<ContactType>>(CreateAllPredefinedContactTypes);
        _predefinedSkillTypes = new Lazy<IReadOnlyList<SkillType>>(CreateAllPredefinedSkillTypes);
    }

    private ReadOnlyCollection<ContactType> CreateAllPredefinedContactTypes()
    {
        var list = new List<ContactType>();

        foreach (var rawDef in ContactRawData.Data)
        {
            var contactType = ContactType.CreateInstance(
                _guidGenerator,
                _clock,
                rawDef.Name,
                rawDef.IconClass);

            foreach (var alias in rawDef.Aliases) contactType.AddAlias(alias.Name, _clock);
            list.Add(contactType);
        }

        return list.AsReadOnly();
    }

    private ReadOnlyCollection<SkillType> CreateAllPredefinedSkillTypes()
    {
        var skills = new List<SkillType>();
        var categoryCache = new Dictionary<string, SkillCategory>();

        foreach (var rawDef in SkillRawData.Data)
        {
            if (!categoryCache.TryGetValue(rawDef.Category.Name, out var category))
            {
                category = SkillCategory.CreateInstance(
                    _guidGenerator,
                    _clock,
                    rawDef.Category.Name);
                categoryCache.Add(rawDef.Category.Name, category);
            }

            var skill = SkillType.CreateInstance(
                _guidGenerator,
                _clock,
                rawDef.Name,
                category,
                new Uri(rawDef.Uri));

            foreach (var alias in rawDef.Aliases)
                skill.AddAlias(alias.Name);

            skills.Add(skill);
        }

        return skills.AsReadOnly();
    }
}