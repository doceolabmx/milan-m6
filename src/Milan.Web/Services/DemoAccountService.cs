using System.Security.Cryptography;
using System.Text;

namespace Milan.Web.Services;

public sealed record DemoAccount(string Id, string Name, string Email);

public sealed class DemoAccountService
{
    private static readonly (DemoAccount Account, string Password)[] Accounts =
    [
        (new("cliente-1", "Cliente MILAN", "cliente@doceolab.com"), "Pass@word1"),
        (new("cliente-2", "Cliente MILAN 2", "admin@doceolab.com"), "Pass@word1")
    ];

    public DemoAccount? Validate(string email, string password)
    {
        foreach (var (account, expected) in Accounts)
        {
            var emailMatches = string.Equals(email, account.Email, StringComparison.OrdinalIgnoreCase);
            var passwordMatches = CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(expected));
            if (emailMatches & passwordMatches) return account;
        }
        return null;
    }
}
