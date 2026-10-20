using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Milan.Tests;

public sealed class CommerceFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public CommerceFlowTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task CustomerCanPurchaseAndOrdersStayPrivate()
    {
        using var ana = CreateClient();
        var catalog = await ana.GetAsync("/");
        var catalogHtml = await catalog.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, catalog.StatusCode);
        Assert.Contains("AeroMILA Halcón P2", WebUtility.HtmlDecode(catalogHtml));
        Assert.Equal(HttpStatusCode.OK, (await ana.GetAsync("/images/products/1.png")).StatusCode);

        var filtered = WebUtility.HtmlDecode(await (await ana.GetAsync("/?brand=AeroMILA&type=Fotografía")).Content.ReadAsStringAsync());
        Assert.Contains("AeroMILA Vista Aérea 6K", filtered);
        Assert.DoesNotContain("AeroMILA Brisa Mini", filtered);
        Assert.DoesNotContain("AgroVuelo Órbita Foto", filtered);

        var token = Extract(catalogHtml, "__RequestVerificationToken");
        var added = await ana.PostAsync("/Carrito?handler=Agregar", Form(token, ("productId", "1")));
        Assert.Equal(HttpStatusCode.Redirect, added.StatusCode);
        Assert.Contains("AeroMILA Halcón P2", WebUtility.HtmlDecode(await (await ana.GetAsync("/Carrito")).Content.ReadAsStringAsync()));

        var challenge = await ana.GetAsync("/Compra");
        Assert.Equal(HttpStatusCode.Redirect, challenge.StatusCode);
        Assert.Contains("/Cuenta/Entrar", challenge.Headers.Location!.OriginalString);
        var login = await ana.GetAsync(challenge.Headers.Location);
        var loginHtml = await login.Content.ReadAsStringAsync();
        var loginToken = Extract(loginHtml, "__RequestVerificationToken");
        var signedIn = await ana.PostAsync("/Cuenta/Entrar", Form(loginToken, ("Email", "cliente@doceolab.com"), ("Password", "Pass@word1"), ("ReturnUrl", "/Compra")));
        Assert.Equal(HttpStatusCode.Redirect, signedIn.StatusCode);
        Assert.Equal("/Compra", signedIn.Headers.Location!.OriginalString);

        var checkout = await ana.GetAsync("/Compra");
        var checkoutHtml = await checkout.Content.ReadAsStringAsync();
        Assert.Contains("$19.50", checkoutHtml);
        var confirmToken = Extract(checkoutHtml, "__RequestVerificationToken");
        var confirmationKey = ExtractInput(checkoutHtml, "confirmationKey");
        var purchase = await ana.PostAsync("/Compra", Form(confirmToken, ("confirmed", "true"), ("confirmationKey", confirmationKey)));
        Assert.Equal(HttpStatusCode.Redirect, purchase.StatusCode);
        var receipt = await ana.GetAsync(purchase.Headers.Location);
        Assert.Equal(HttpStatusCode.OK, receipt.StatusCode);
        Assert.Contains("COMPRA EXITOSA", await receipt.Content.ReadAsStringAsync());
        Assert.Contains("AeroMILA Halcón P2", WebUtility.HtmlDecode(await (await ana.GetAsync("/MisPedidos")).Content.ReadAsStringAsync()));

        using var luis = CreateClient();
        var luisLogin = await luis.GetAsync("/Cuenta/Entrar");
        var luisHtml = await luisLogin.Content.ReadAsStringAsync();
        var luisToken = Extract(luisHtml, "__RequestVerificationToken");
        var luisSignedIn = await luis.PostAsync("/Cuenta/Entrar", Form(luisToken, ("Email", "admin@doceolab.com"), ("Password", "Pass@word1"), ("ReturnUrl", "/")));
        Assert.Equal(HttpStatusCode.Redirect, luisSignedIn.StatusCode);
        var ownOrders = await luis.GetAsync("/MisPedidos");
        Assert.Equal(HttpStatusCode.OK, ownOrders.StatusCode);
        Assert.Contains("Aún no tienes pedidos", await ownOrders.Content.ReadAsStringAsync());
    }

    private HttpClient CreateClient() => _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static FormUrlEncodedContent Form(string token, params (string Key, string Value)[] fields) =>
        new(fields.Select(field => new KeyValuePair<string, string>(field.Key, field.Value)).Append(new("__RequestVerificationToken", token)));

    private static string Extract(string html, string inputName)
    {
        var match = Regex.Match(html, $"<input[^>]*name=\\\"{Regex.Escape(inputName)}\\\"[^>]*value=\\\"([^\\\"]+)\\\"", RegexOptions.IgnoreCase);
        if (!match.Success) match = Regex.Match(html, $"<input[^>]*value=\\\"([^\\\"]+)\\\"[^>]*name=\\\"{Regex.Escape(inputName)}\\\"", RegexOptions.IgnoreCase);
        Assert.True(match.Success, $"No se encontró el campo {inputName}.");
        return WebUtility.HtmlDecode(match.Groups[1].Value);
    }

    private static string ExtractInput(string html, string inputName) => Extract(html, inputName);
}
