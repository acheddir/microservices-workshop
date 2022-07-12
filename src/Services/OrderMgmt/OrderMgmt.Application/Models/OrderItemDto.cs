namespace OrderMgmt.Application.Models;

public record OrderItemDto
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public double UnitPrice { get; set; }
    public double Discount { get; set; }
    public int Units { get; set; }
    public string? PictureUrl { get; set; }
}