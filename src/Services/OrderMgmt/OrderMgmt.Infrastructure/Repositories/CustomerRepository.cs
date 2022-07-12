namespace OrderMgmt.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly OrderMgmtContext _context;

    public CustomerRepository(OrderMgmtContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public IUnitOfWork UnitOfWork => _context;

    public Customer Add(Customer customer)
    {
        if (customer.IsTransient())
        {
            return _context.Customers
                .Add(customer)
                .Entity;
        }
        else
        {
            return customer;
        }
    }

    public Customer Update(Customer customer)
    {
        return _context.Customers
            .Update(customer)
            .Entity;
    }

    public async Task<Customer> FindAsync(string customerIg)
    {
        var customer = await _context.Customers
            .Include(b => b.PaymentMethods)
            .Where(b => b.IG == customerIg)
            .SingleOrDefaultAsync();

        return customer;
    }

    public async Task<Customer> FindByIdAsync(string id)
    {
        var customer = await _context.Customers
            .Include(b => b.PaymentMethods)
            .Where(b => b.Id == Guid.Parse(id))
            .SingleOrDefaultAsync();

        return customer;
    }
}