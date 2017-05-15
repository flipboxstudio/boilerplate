using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtension
    {
        public static string CalculateMD5(this string input)
        {
            // step 1, calculate MD5 hash from input

            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            var sb = new StringBuilder();

            foreach (var t in hash)
                sb.Append(t.ToString("X2"));

            return sb.ToString();
        }

        public static int ToInt(this string input)
        {
            return int.Parse(input);
        }
    }
}