namespace OrderMgmt.Infrastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly OrderMgmtContext _context;

    public RequestManager(OrderMgmtContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var request = await _context.FindAsync<ClientRequest>(id);

        return request != null;
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exists = await ExistsAsync(id);

        var request = exists
            ? throw new OrderMgmtException($"Request with {id} already exists")
            : new ClientRequest
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

        _context.Add(request);

        await _context.SaveChangesAsync();
    }
}