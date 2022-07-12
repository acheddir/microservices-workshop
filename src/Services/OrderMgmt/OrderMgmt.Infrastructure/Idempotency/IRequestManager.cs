namespace OrderMgmt.Infrastructure.Idempotency;

public interface IRequestManager
{
    Task<bool> ExistsAsync(Guid id);
    Task CreateRequestForCommandAsync<T>(Guid id);
}