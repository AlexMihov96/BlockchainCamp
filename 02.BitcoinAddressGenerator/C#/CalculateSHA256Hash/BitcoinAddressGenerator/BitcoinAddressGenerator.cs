using System;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace BitcoinAddressGenerator
{
    public class BitcoinAddressGenerator
    {
        public static void Main()
        {
            string hexHash = "0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";

            byte[] publicKey = HexToByte(hexHash);
            Console.WriteLine("Public key: " + ByteToHex(publicKey));
            Console.WriteLine();

            byte[] pubKeySha = Sha256(publicKey);
            Console.WriteLine("Sha public key: " + ByteToHex(pubKeySha));
            Console.WriteLine();

            byte[] publicKeyShaRipe = Ripemd160(pubKeySha);
            Console.WriteLine("Ripe Sha Public key: " + ByteToHex(publicKeyShaRipe));
            Console.WriteLine();

            byte[] preHashNetwork = AppendBitcoinNetwork(publicKeyShaRipe, 0);
            byte[] publicHash = Sha256(preHashNetwork);
            Console.WriteLine("Public Hash: " + ByteToHex(publicHash));
            Console.WriteLine();

            byte[] publicHashHash = Sha256(publicHash);
            Console.WriteLine("Public Hash Hash: " + ByteToHex(publicHashHash));
            Console.WriteLine();

            byte[] hashAddress = ConcatAddress(preHashNetwork, publicHashHash);
            Console.WriteLine("Hash Address: " + ByteToHex(hashAddress));
            Console.WriteLine();

            string address = Base58Encode(hashAddress);
            Console.WriteLine("Address: " + address);
            Console.WriteLine();
        }

        private static byte[] HexToByte(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new Exception("Invalid HEX");
            }

            byte[] retArray = new byte[hexString.Length / 2];

            for (int i = 0; i < retArray.Length; ++i)
            {
                retArray[i] = byte.Parse(hexString.Substring(i * 2, 2), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture);
            }

            return retArray;
        }

        private static byte[] Sha256(byte[] array)
        {
            SHA256Managed hashString = new SHA256Managed();

            return hashString.ComputeHash(array);
        }

        private static byte[] Ripemd160(byte[] publicKeySha)
        {
            RIPEMD160Managed hashString = new RIPEMD160Managed();

            return hashString.ComputeHash(publicKeySha);
        }

        private static byte[] AppendBitcoinNetwork(byte[] ripeHash, byte network)
        {
            byte[] extended = new byte[ripeHash.Length + 1];
            extended[0] = (byte)network;

            Array.Copy(ripeHash, 0, extended, 1, ripeHash.Length);

            return extended;
        }

        private static byte[] ConcatAddress(byte[] ripeHash, byte[] checkSum)
        {
            byte[] ret = new byte[ripeHash.Length + 4];

            Array.Copy(ripeHash, ret, ripeHash.Length);
            Array.Copy(checkSum, 0, ret, ripeHash.Length, 4);

            return ret;
        }

        private static string ByteToHex(byte[] pubKeySha)
        {
            byte[] data = pubKeySha;

            string hex = BitConverter.ToString(data);

            return hex;
        }

        private static string Base58Encode(byte[] hashAddress)
        {
            const string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            string retString = string.Empty;

            BigInteger encodeSize = alphabet.Length;
            BigInteger arrayToInt = 0;

            for (int i = 0; i < hashAddress.Length; ++i)
            {
                arrayToInt = arrayToInt * 256 + hashAddress[i];
            }

            while (arrayToInt > 0)
            {
                int rem = (int)(arrayToInt % encodeSize);
                arrayToInt /= encodeSize;
                retString = alphabet[rem] + retString;
            }

            for (int i = 0; i < hashAddress.Length && hashAddress[i] == 0; ++i)
            {
                retString = alphabet[0] + retString;
            }

            return retString;
        }
    }
}
