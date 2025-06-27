using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Data;
using JobMagnet.Shared.Tests.Abstractions;

namespace JobMagnet.Shared.Tests.Factories;

public class TestDataFactory
{
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;

    private readonly Lazy<IReadOnlyList<ContactType>> _predefinedContactTypes;

    public TestDataFactory()
    {
        _clock = new DeterministicClock();
        _guidGenerator = new SequentialGuidGenerator();

        _predefinedContactTypes = new Lazy<IReadOnlyList<ContactType>>(CreateAllPredefinedContactTypes);
    }

    public IReadOnlyList<ContactType> PredefinedContactTypes => _predefinedContactTypes.Value;

    private IReadOnlyList<ContactType> CreateAllPredefinedContactTypes()
    {
        var list = new List<ContactType>();

        foreach (var rawDef in ContactRawData.Data)
        {
            var contactType = ContactType.CreateInstance(
                _guidGenerator,
                _clock,
                rawDef.Name,
                rawDef.IconClass);

            foreach (var alias in rawDef.Aliases)
            {
                contactType.AddAlias(alias, _clock);
            }
            list.Add(contactType);
        }
        return list.AsReadOnly();
    }
}