using Mono.Cecil;
using NetArchTest.Rules;

namespace ArchitectureTestSample.Arch.Tests.CustomRules;

internal sealed class MaximumConstructorParametersRule : ICustomRule
{
    private readonly int _maximumParameters;

    public MaximumConstructorParametersRule(int maximumParameters)
    {
        _maximumParameters = maximumParameters;
    }

    public bool MeetsRule(TypeDefinition type)
    {
        var constructors = type.Methods.Where(x => x.IsConstructor).ToList();
        
        return constructors.Count == 0 || constructors.All(x => x.Parameters.Count <= _maximumParameters);
    }
}