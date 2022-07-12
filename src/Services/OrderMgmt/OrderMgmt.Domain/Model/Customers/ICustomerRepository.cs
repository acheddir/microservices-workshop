namespace OrderMgmt.Domain.Model.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Customer Add(Customer customer);
    Customer Update(Customer customer);
    Task<Customer> FindAsync(string customerIg);
    Task<Customer> FindByIdAsync(string id);
}