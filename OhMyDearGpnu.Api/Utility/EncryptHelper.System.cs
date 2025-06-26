using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace OhMyDearGpnu.Api.Utility;

public static partial class EncryptHelper
{
    private const string TeachEvalKeyText = "nfZYwnW2ppQc3CXr";
    private static readonly byte[] TeachEvalKey;

    static EncryptHelper()
    {
        TeachEvalKey = Encoding.UTF8.GetBytes(TeachEvalKeyText);
    }

    public static string CasPasswordEncrypt(string input, byte[] publicExponent, byte[] modulus)
    {
        var e = new BigInteger(publicExponent, true, true);
        var m = new BigInteger(modulus, true, true);
        var c = new BigInteger(Encoding.ASCII.GetBytes(input));
        c = BigInteger.ModPow(c, e, m);
        return Convert.ToHexString(c.ToByteArray(true, true));
    }

    public static string CasPasswordDecrypt(string input, byte[] privateExponent, byte[] modulus)
    {
        var d = new BigInteger(privateExponent, true, true);
        var m = new BigInteger(modulus, true, true);
        var c = new BigInteger(Convert.FromHexString(input), true, true);
        c = BigInteger.ModPow(c, d, m);
        return Encoding.ASCII.GetString(c.ToByteArray());
    }

    public static string TeachEvalPayloadEncrypt(string input)
    {
        using var aes = Aes.Create();
        aes.Key = TeachEvalKey;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        return Convert.ToBase64String(encryptedBytes);
    }

    public static string TeachEvalSaveEncrypt(string input, byte[] publicExponent, byte[] modulus)
    {
        var rsa = RSA.Create(new RSAParameters
        {
            Modulus = modulus,
            Exponent = publicExponent
        });
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var encryptedBytes = rsa.Encrypt(inputBytes, RSAEncryptionPadding.Pkcs1);
        return Convert.ToHexString(encryptedBytes).ToLower();
    }
}