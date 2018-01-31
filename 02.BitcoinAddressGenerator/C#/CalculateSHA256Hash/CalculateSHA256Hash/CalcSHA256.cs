using System;
using System.Security.Cryptography;
using System.Text;

namespace CalculateSHA256Hash
{
    public class CalcSHA256
    {
        public static void Main(string[] args)
        {
            string result = GetHashSha256("Hello_World");

            Console.WriteLine(result);
        }

        private static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);

            SHA256Managed hashString = new SHA256Managed();

            byte[] hash = hashString.ComputeHash(bytes);
            string hashed = null;

            foreach (var h in hash)
            {
                hashed += $"{h:x2}";
            }

            return hashed;
        }
    }
}
