using System.Security.Claims;
using Milan.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Milan.Web.Pages;
[Authorize]
public sealed class CompraModel(SessionCartService carts, OrderService orders) : PageModel
{
    public Milan.Web.Models.Cart Cart { get; private set; } = new();
    public string ConfirmationKey { get; private set; } = "";
    public void OnGet() { Cart = carts.Get(); ConfirmationKey = carts.GetConfirmationKey(); }
    public IActionResult OnPost(bool confirmed, string confirmationKey)
    {
        if (!confirmed) { ModelState.AddModelError(string.Empty, "Confirma que deseas continuar con la compra."); Cart = carts.Get(); ConfirmationKey = carts.GetConfirmationKey(); return Page(); }
        if (string.IsNullOrWhiteSpace(confirmationKey) || !Guid.TryParseExact(confirmationKey, "N", out _)) return BadRequest();
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var cart = carts.Get();
        try
        {
            var order = orders.Create(customerId, cart, $"{customerId}:{confirmationKey}");
            carts.Clear();
            return RedirectToPage("/PedidoConfirmado", new { id = order.Id });
        }
        catch (InvalidOperationException ex) { ModelState.AddModelError(string.Empty, ex.Message); Cart = cart; ConfirmationKey = carts.GetConfirmationKey(); return Page(); }
    }
}
