using System.Security.Cryptography;
using System.Text;

namespace OhMyDearGpnu.Api
{
    public static class EncryptHelper
    {
        public static string Encrypt(string content, string exponentBase64, string modulesBase64)
        {
            var chiper = new RSACryptoServiceProvider();
            chiper.ImportParameters(new RSAParameters()
            {
                Exponent = Convert.FromBase64String(exponentBase64),
                Modulus = Convert.FromBase64String(modulesBase64)
            });
            var data = chiper.Encrypt(Encoding.ASCII.GetBytes(content), RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(data);
        }
    }
}
