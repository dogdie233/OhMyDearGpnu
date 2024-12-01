using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace OhMyDearGpnu.Api.Utility;

public static partial class EncryptHelper
{
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
}