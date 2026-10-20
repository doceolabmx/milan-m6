using System.Security.Claims;
using Milan.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Milan.Web.Pages.Cuenta;
[AllowAnonymous]
public sealed class EntrarModel(DemoAccountService accounts) : PageModel
{
    [BindProperty] public string Email { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    [BindProperty] public string? ReturnUrl { get; set; }
    public IActionResult OnGet(string? returnUrl)
    {
        if (User.Identity?.IsAuthenticated == true) return LocalRedirect(SafeReturnUrl(returnUrl));
        ReturnUrl = SafeReturnUrl(returnUrl); return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        ReturnUrl = SafeReturnUrl(ReturnUrl);
        var account = accounts.Validate(Email, Password);
        if (account is null) { ModelState.AddModelError(string.Empty, "El correo o la contraseña no son correctos."); return Page(); }
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, account.Id), new Claim(ClaimTypes.Name, account.Name), new Claim(ClaimTypes.Email, account.Email), new Claim(ClaimTypes.Role, "Cliente") };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return LocalRedirect(SafeReturnUrl(ReturnUrl));
    }
    private static string SafeReturnUrl(string? url) =>
        !string.IsNullOrWhiteSpace(url) && url.StartsWith('/') && !url.StartsWith("//") && !(url.Length > 1 && url[1] == '\\') ? url : "/";
}
