namespace ArchitectureTestSample.Brokers;

public interface IDateTimeBroker
{
    DateTimeOffset GetCurrentDateTime();
}