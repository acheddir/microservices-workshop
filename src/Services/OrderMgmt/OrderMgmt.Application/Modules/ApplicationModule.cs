using OrderMgmt.Domain.Model.Customers;
using OrderMgmt.Domain.Model.Orders;
using OrderMgmt.Infrastructure.Idempotency;
using OrderMgmt.Infrastructure.Repositories;

namespace OrderMgmt.Application.Modules;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CustomerRepository>()
            .As<ICustomerRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<OrderRepository>()
            .As<IOrderRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<RequestManager>()
            .As<IRequestManager>()
            .InstancePerLifetimeScope();
    }
}