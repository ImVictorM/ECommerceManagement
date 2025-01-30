using Application.Common.Security.Authentication;

using Infrastructure.Security.Authentication.Settings;

using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Infrastructure.Security.Authentication;

/// <summary>
/// Hash-Based Message Authentication Codes (HMAC) signature provider implementation.
/// </summary>
public sealed class HmacSignatureProvider : IHmacSignatureProvider
{
    private readonly string _secret;

    /// <summary>
    /// Initiates a new instance of the <see cref="HmacSignatureProvider"/> class.
    /// </summary>
    /// <param name="signatureOptions">The signature options.</param>
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
