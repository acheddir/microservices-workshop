namespace OrderMgmt.Application.Models;

public record OrderDraftDto
{
    public double Total { get; set; }
    public IEnumerable<OrderItemDto>? OrderItems { get; set; }
}