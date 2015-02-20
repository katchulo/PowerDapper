using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Acto.API.Tests.Util
{
    public class GenSHA1
    {
        /// <summary>
        /// Recebe uma string e converte para SHA1
        /// </summary>
        /// <param name="text">String</param>
        /// <param name="enc">Codificação ex: System.Text.Encoding.UTF8</param>
        /// <returns></returns>
        public static string CalculateSHA1(string text, Encoding enc)
        {
            if (!String.IsNullOrEmpty(text))
            {
                byte[] buffer = enc.GetBytes(text);
                SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
                return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
            }
            else
            {
                return "";
            }
        }
    }
}
