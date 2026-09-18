using System.Security.Cryptography;
using System.Text;

namespace MyApp.Features.Auth.Shared;

public static class RefreshTokenHasher
{
    public static string Hash(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var bytes = Encoding.UTF8.GetBytes(token);
        return Convert.ToHexString(SHA256.HashData(bytes)).ToUpperInvariant();
    }
}
