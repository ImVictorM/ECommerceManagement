using Application.Common.Security.Authentication;

using Infrastructure.Security.Authentication.Settings;

using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Infrastructure.Security.Authentication;

internal sealed class HmacSignatureProvider : IHmacSignatureProvider
{
    private readonly string _secret;

    public HmacSignatureProvider(IOptions<HmacSignatureSettings> signatureOptions)
    {
        _secret = signatureOptions.Value.Secret;
    }

    /// <inheritdoc/>
    public string ComputeHmac(string payload)
    {
        var secretBytes = Encoding.UTF8.GetBytes(_secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(secretBytes);
        var hashBytes = hmac.ComputeHash(payloadBytes);

        return Convert.ToBase64String(hashBytes);
    }

    /// <inheritdoc/>
    public bool Verify(string computedSignature, string providedSignature)
    {
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computedSignature),
            Convert.FromBase64String(providedSignature)
        );
    }
}
