namespace Milan.Web.Models;

public sealed record OrderLine(int ProductId, string Name, string ImagePath, decimal UnitPrice, int Quantity)
{
    public decimal Subtotal => UnitPrice * Quantity;
}

public sealed record Order(Guid Id, string CustomerId, DateTimeOffset CreatedAt, IReadOnlyList<OrderLine> Items, decimal Total, string Status);
