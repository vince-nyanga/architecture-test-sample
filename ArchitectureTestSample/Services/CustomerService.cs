using ArchitectureTestSample.Brokers;

namespace ArchitectureTestSample.Services;

internal sealed class CustomerService : ICustomerService
{
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly IStorageBroker _storageBroker;

    public CustomerService(
        IDateTimeBroker dateTimeBroker,
        IStorageBroker storageBroker)
    {
        _dateTimeBroker = dateTimeBroker;
        _storageBroker = storageBroker;
    }
}