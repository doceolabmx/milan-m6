using System.Globalization;
using Milan.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
var mexico = CultureInfo.GetCultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentCulture = mexico;
CultureInfo.DefaultThreadCurrentUICulture = mexico;
builder.Services.AddRazorPages();
builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { options.Cookie.Name = ".Milan.Session"; options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true; options.IdleTimeout = TimeSpan.FromHours(2); });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = ".Milan.Auth"; options.Cookie.HttpOnly = true; options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; options.LoginPath = "/Cuenta/Entrar";
    options.AccessDeniedPath = "/Cuenta/Entrar"; options.SlidingExpiration = true; options.ExpireTimeSpan = TimeSpan.FromHours(8);
});
builder.Services.AddAuthorization();
builder.Services.AddSingleton<CatalogService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<DemoAccountService>();
builder.Services.AddScoped<SessionCartService>();
var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");
app.UseStaticFiles(); app.UseRouting(); app.UseSession(); app.UseAuthentication(); app.UseAuthorization(); app.MapRazorPages();
app.Run();
public partial class Program;
