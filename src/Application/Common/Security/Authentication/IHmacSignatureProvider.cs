namespace Application.Common.Security.Authentication;

/// <summary>
/// Defines a Hash-Based Message Authentication Codes (HMAC) signature provider interface.
/// </summary>
public interface IHmacSignatureProvider
{
    /// <summary>
    /// Computes an HMAC signature for a given payload.
    /// <para>
    /// This method takes a string payload (usually the body or content of a request) and computes
    /// an HMAC signature using a secret key. The computed signature is then returned as a Base64-encoded string.
    /// </para>
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <returns>
    /// A Base64-encoded string representing the HMAC signature computed for the payload.
    /// </returns>
    string ComputeHmac(string payload);

    /// <summary>
    /// Verifies if a provided signature matches the computed signature for a given payload.
    /// <para>
    /// This method compares a provided signature (usually from an HTTP request header) with the computed
    /// HMAC signature for the same payload.
    /// </para>
    /// </summary>
    /// <param name="computedSignature">The signature that was computed for the payload.</param>
    /// <param name="providedSignature">The signature that was provided by the sender (usually in an HTTP header).</param>
    /// <returns>
    /// A boolean value indicating whether the provided signature matches the computed signature.
    /// </returns>
    bool Verify(string computedSignature, string providedSignature);
}
