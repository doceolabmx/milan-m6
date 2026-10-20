using System.Security.Claims;
using Milan.Web.Models;
using Milan.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Milan.Web.Pages;
[Authorize]
public sealed class PedidoConfirmadoModel(OrderService orders) : PageModel
{
    public Order? Order { get; private set; }
    public IActionResult OnGet(Guid id)
    {
        Order = orders.FindForCustomer(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Order is null ? NotFound() : Page();
    }
}
