using Milan.Web.Models;

namespace Milan.Web.Services;

public sealed class OrderService
{
    private readonly object _sync = new();
    private readonly Dictionary<Guid, Order> _orders = [];
    private readonly Dictionary<string, Guid> _confirmationKeys = new(StringComparer.Ordinal);

    public Order Create(string customerId, Cart cart, string confirmationKey)
    {
        lock (_sync)
        {
            if (_confirmationKeys.TryGetValue(confirmationKey, out var existingId))
                return _orders[existingId];
            if (cart.Items.Count == 0) throw new InvalidOperationException("El carrito está vacío.");
            if (cart.Items.Any(item => item.Quantity is < 1 or > 99)) throw new InvalidOperationException("Hay cantidades inválidas.");

            var lines = cart.Items.Select(item => new OrderLine(item.ProductId, item.Name, item.ImagePath, item.UnitPrice, item.Quantity)).ToArray();
            var order = new Order(Guid.NewGuid(), customerId, DateTimeOffset.UtcNow, lines, lines.Sum(item => item.Subtotal), "Pagado (simulado)");
            _orders.Add(order.Id, order);
            _confirmationKeys.Add(confirmationKey, order.Id);
            return order;
        }
    }

    public IReadOnlyList<Order> GetForCustomer(string customerId)
    {
        lock (_sync) return _orders.Values.Where(order => order.CustomerId == customerId).OrderByDescending(order => order.CreatedAt).ToArray();
    }

    public Order? FindForCustomer(Guid id, string customerId)
    {
        lock (_sync) return _orders.TryGetValue(id, out var order) && order.CustomerId == customerId ? order : null;
    }
}
