using Milan.Web.Models;
using Milan.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Milan.Web.Pages;
public sealed class CarritoModel(SessionCartService carts) : PageModel
{
    public Cart Cart { get; private set; } = new();
    [TempData] public string? Message { get; set; }
    public void OnGet() => Cart = carts.Get();
    public IActionResult OnPostAgregar(int productId) { carts.Add(productId); Message = "Artículo agregado al carrito."; return RedirectToPage(); }
    public IActionResult OnPostCantidad(int productId, int quantity)
    {
        if (!carts.SetQuantity(productId, quantity)) Message = "La cantidad debe ser de 0 a 99.";
        else Message = "Carrito actualizado.";
        return RedirectToPage();
    }
    public IActionResult OnPostEliminar(int productId) { carts.Remove(productId); Message = "Artículo eliminado del carrito."; return RedirectToPage(); }
}
