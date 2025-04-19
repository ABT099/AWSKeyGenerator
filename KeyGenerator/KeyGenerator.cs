using System.Security.Cryptography;
using System.Text;

namespace KeyGenerator;

public static class KeyGenerator
{
    
    public static (string privateKey, string publicKey) GenerateRsaKey(int defaultRsaKeySize)
    {
        using var rsa = RSA.Create(defaultRsaKeySize);
        var privateKeyPem = ExportToPem(rsa.ExportPkcs8PrivateKey(), "PRIVATE KEY");
        var publicKeyPem = ExportToPem(rsa.ExportSubjectPublicKeyInfo(), "PUBLIC KEY");

        return (privateKeyPem, publicKeyPem);
    }
    
    
    // Convert binary key data to PEM format
    private static string ExportToPem(byte[] keyData, string label)
    {
        const int lineLength = 64;

        var builder = new StringBuilder();
        builder.AppendLine($"-----BEGIN {label}-----");

        var base64 = Convert.ToBase64String(keyData);
        for (var i = 0; i < base64.Length; i += lineLength)
        {
            builder.AppendLine(base64.Substring(i, Math.Min(lineLength, base64.Length - i)));
        }

        builder.AppendLine($"-----END {label}-----");
        return builder.ToString();
    }
}