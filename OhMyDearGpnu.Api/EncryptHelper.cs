using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System.Text;

namespace OhMyDearGpnu.Api
{
    public static class EncryptHelper
    {
        public static string Encrypt(string content, string exponentBase64, string modulusBase64)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(content);
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            var exponent = new BigInteger(Convert.FromBase64String(exponentBase64));
            var modulus = new BigInteger(Convert.FromBase64String(modulusBase64));

            var keyParameter = new RsaKeyParameters(false, modulus, exponent);
            encryptEngine.Init(true, keyParameter);

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;
        }
    }
}
