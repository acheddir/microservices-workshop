﻿namespace OrderMgmt.Domain.Model.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Order Add(Order order);
    void Update(Order order);
    
    Task<Order?> GetAsync(Guid orderId);
}