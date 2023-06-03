using Mono.Cecil;
using NetArchTest.Rules;

namespace ArchitectureTestSample.Arch.Tests.CustomRules;

internal sealed class MaximumConstructorParametersRule : ICustomRule
{
    private readonly int _maximumParameterCount;

    public MaximumConstructorParametersRule(int maximumParameterCount)
    {
        _maximumParameterCount = maximumParameterCount;
    }

    public bool MeetsRule(TypeDefinition type)
    {
        var constructors = type.Methods.Where(x => x.IsConstructor).ToList();
        
        return constructors.Count == 0 || constructors.All(x => x.Parameters.Count <= _maximumParameterCount);
    }
}