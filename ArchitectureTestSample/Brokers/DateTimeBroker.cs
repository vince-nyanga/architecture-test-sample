namespace ArchitectureTestSample.Brokers;

internal sealed class DateTimeBroker : IDateTimeBroker
{
    public DateTimeOffset GetCurrentDateTime() => DateTimeOffset.UtcNow;
}