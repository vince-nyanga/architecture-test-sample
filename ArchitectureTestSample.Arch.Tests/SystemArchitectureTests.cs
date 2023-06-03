using ArchitectureTestSample.Arch.Tests.CustomRules;
using ArchitectureTestSample.Brokers;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using NetArchTest.Rules;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTestSample.Arch.Tests;

public class SystemArchitectureTests
{
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(IStorageBroker).Assembly).Build();

    private readonly IObjectProvider<Class> _brokerClasses =
        Classes().That().ResideInNamespace("ArchitectureTestSample.Brokers").As("Brokers");
    
    private readonly IObjectProvider<Class> _serviceClasses =
        Classes().That().ResideInNamespace("ArchitectureTestSample.Services").As("Services");

    private static readonly Types Types = Types.InAssembly(typeof(Program).Assembly);

    [Fact]
    public void BrokersShouldBeSealedAndInternalV1()
    {
        // given
        IArchRule rule = Classes().
            That().Are(_brokerClasses)
            .Should().BeInternal()
            .AndShould().BeSealed();

        // then
        rule.Check(Architecture);
    }
    
    [Fact]
    public void BrokersShouldBeSealedAndInternalV2()
    {
        // given
        var conditions = Types.That()
            .AreClasses()
            .And().ResideInNamespace("ArchitectureTestSample.Brokers")
            .Should().BeSealed()
            .And().NotBePublic();
        
        // when
        var result = conditions.GetResult();

        // then
       AssertConditionsAreMet(result, "brokers should be sealed and internal");
    }

    [Fact]
    public void ServicesShouldDependOnBrokersV1()
    {
        // given
        IArchRule rule = Classes().
            That().Are(_serviceClasses)
            .Should().DependOnAnyTypesThat().ResideInNamespace("ArchitectureTestSample.Brokers");
        
        // then
        rule.Check(Architecture);
    }

    [Fact]
    public void ServicesShouldDependOnBrokersV2()
    {
        // given
        var conditions = Types.That()
            .AreClasses()
            .And().ResideInNamespace("ArchitectureTestSample.Services")
            .Should().HaveDependencyOn("ArchitectureTestSample.Brokers");
        
        // when
        var result = conditions.GetResult();
        
        // then
        AssertConditionsAreMet(result, "services should depend on brokers");
    }
    
    [Fact]
    public void AllClassesShouldHaveMaximumOfThreeConstructorParametersV1()
    {
        // given
        var maximumConstructorParametersCondition = new MaximumConstructorParametersCondition(3);
        IArchRule rule = Classes().Should().FollowCustomCondition(maximumConstructorParametersCondition);
        
        // then
        rule.Check(Architecture);
    }
    
    [Fact] 
    public void AllClassesShouldHaveMaximumOfThreeConstructorParametersV2()
    {
        // given
        var maximumConstructorParametersRule = new MaximumConstructorParametersRule(maximumParameters: 3);
        var conditions = Types.That()
            .AreClasses()
            .Should().MeetCustomRule(maximumConstructorParametersRule);
        
        // when
        var result = conditions.GetResult();

        // then
        AssertConditionsAreMet(result, "all classes should have maximum of three constructor parameters");
    }
    
    private static void AssertConditionsAreMet(TestResult result, string ruleName)
    {
        var failingTypes = string.Join(", ", result.FailingTypes?.Select(x => x.Name) ?? Array.Empty<string>());
        Assert.True(result.IsSuccessful, userMessage: $"The following types do not meet the '{ruleName}' rule: {failingTypes}");
    }
}