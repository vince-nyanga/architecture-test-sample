using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace ArchitectureTestSample.Arch.Tests.CustomRules;

internal sealed class MaximumConstructorParametersCondition : ICondition<Class>
{
    private readonly int _maximumParameters;
    
    public string Description => $"should have no more than {_maximumParameters} constructor parameters";

    public MaximumConstructorParametersCondition(int maximumParameters)
    {
        _maximumParameters = maximumParameters;
    }

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (var @class in objects)
        {
            var constructors = @class.GetConstructors().ToList();
            
            if (constructors.Count == 0)
            {
                yield return new ConditionResult(@class, pass: true);
            }
            
            if (constructors.Any(x => x.Parameters.Count() <= _maximumParameters))
            {
                yield return new ConditionResult(@class, pass: true);
            }

            yield return new ConditionResult(@class, pass: false, failDescription: $"has a constructor with more than {_maximumParameters} parameters");
        }
    }

    public bool CheckEmpty()
    {
        return true;
    }
}