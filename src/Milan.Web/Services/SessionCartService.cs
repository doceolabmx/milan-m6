using System.Text.Json;
using Milan.Web.Models;

namespace Milan.Web.Services;

public sealed class SessionCartService(IHttpContextAccessor accessor, CatalogService catalog)
{
    private const string Key = "milan.cart";
    private const string ConfirmationKey = "milan.checkout-key";

    public Cart Get()
    {
        var json = accessor.HttpContext!.Session.GetString(Key);
        return json is null ? new Cart() : JsonSerializer.Deserialize<Cart>(json) ?? new Cart();
    }

    public void Add(int productId)
    {
        var product = catalog.Find(productId);
        if (product is null) return;
        var cart = Get(); cart.Add(product); Save(cart); InvalidateConfirmation();
    }

    public bool SetQuantity(int productId, int quantity)
    {
        if (quantity is < 0 or > 99) return false;
        var cart = Get(); cart.SetQuantity(productId, quantity); Save(cart); InvalidateConfirmation(); return true;
    }

    public void Remove(int productId) { var cart = Get(); cart.Remove(productId); Save(cart); InvalidateConfirmation(); }
    public void Clear() => Save(new Cart());

    public string GetConfirmationKey()
    {
        var session = accessor.HttpContext!.Session;
        var key = session.GetString(ConfirmationKey);
        if (key is null) { key = Guid.NewGuid().ToString("N"); session.SetString(ConfirmationKey, key); }
        return key;
    }

    private void Save(Cart cart) => accessor.HttpContext!.Session.SetString(Key, JsonSerializer.Serialize(cart));
    private void InvalidateConfirmation() => accessor.HttpContext!.Session.Remove(ConfirmationKey);
}
