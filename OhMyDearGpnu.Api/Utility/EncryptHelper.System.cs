using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace OhMyDearGpnu.Api.Utility;

public static partial class EncryptHelper
{
    private const string TeachEvalKeyText = "nfZYwnW2ppQc3CXr";
    private static readonly byte[] _teachEvalKey;

    static EncryptHelper()
    {
        _teachEvalKey = Encoding.UTF8.GetBytes(TeachEvalKeyText);
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
        aes.Key = _teachEvalKey;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        return Convert.ToBase64String(encryptedBytes);
    }

    public static string TeachEvalPayloadDecrypt(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        try
        {
            using var aes = Aes.Create();
            aes.Key = _teachEvalKey;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using var memoryStream = new MemoryStream(Convert.FromBase64String(input));
            using var cryptoStream = new CryptoStream(
                memoryStream,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream, Encoding.UTF8);

            return reader.ReadToEnd();
        }
        catch
        {
            return string.Empty;
        }
    }
}