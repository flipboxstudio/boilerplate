using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtension
    {
        /// <summary>
        /// Calculate MD5 from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CalculateMD5(this string input)
        {
            // step 1, calculate MD5 hash from input

            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            var StringBuilder = new StringBuilder();

            foreach (var character in hash)
                StringBuilder.Append(character.ToString("X2"));

            return StringBuilder.ToString();
        }

        /// <summary>
        /// Convert string to integer.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this string input)
        {
            return int.Parse(input);
        }

        /// <summary>
        /// Generate random string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Random(this string input, int length)
        {
            const string libraries = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            var result = new StringBuilder();
            var random = new Random();

            while (0 < length--)
            {
                result.Append(libraries[random.Next(libraries.Length)]);
            }

            return result.ToString();
        }
    }
}