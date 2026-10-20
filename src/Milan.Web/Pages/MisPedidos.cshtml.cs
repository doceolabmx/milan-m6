using System.Security.Claims;
using Milan.Web.Models;
using Milan.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Milan.Web.Pages;
[Authorize]
public sealed class MisPedidosModel(OrderService orders) : PageModel
{
    public IReadOnlyList<Order> Orders { get; private set; } = [];
    public void OnGet() => Orders = orders.GetForCustomer(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
