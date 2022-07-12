namespace OrderMgmt.Application.Models;

public record Order
{
    public int ordernumber { get; init; }
    public DateTime date { get; init; }
    public string status { get; init; }
    public string description { get; init; }
    public string street { get; init; }
    public string city { get; init; }
    public string zipcode { get; init; }
    public string country { get; init; }
    public List<OrderItem> orderitems { get; init; }
    public decimal total { get; set; }
}

public record OrderItem
{
    public string productname { get; init; }
    public int units { get; init; }
    public double unitprice { get; init; }
    public string pictureurl { get; init; }
}