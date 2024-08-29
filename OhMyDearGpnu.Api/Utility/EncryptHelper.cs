using System.Security.Cryptography;
using System.Text;

namespace OhMyDearGpnu.Api.Utility;

public static class EncryptHelper
{
    public static byte[] EncryptPkcs1(byte[] content, byte[] exponent, byte[] modulus)
    {
        using var rsa = RSA.Create();
        var rsaParameters = new RSAParameters
        {
            Exponent = exponent,
            Modulus = modulus
        };
        
        rsa.ImportParameters(rsaParameters);
        return rsa.Encrypt(content, RSAEncryptionPadding.Pkcs1);
    }
    
    public static byte[] EncryptPkcs1(string content, byte[] exponent, byte[] modulus)
        => EncryptPkcs1(Encoding.UTF8.GetBytes(content), exponent, modulus);
}