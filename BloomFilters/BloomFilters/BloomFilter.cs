using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace BloomFilters
{
    class BloomFilter
    {
        public bool[] BitMap { get; set; }
        private int lengthOfBitMap;
        private int numberOfHashes;

        public BloomFilter()
        {
            numberOfHashes = 4;
            lengthOfBitMap = (int)Math.Pow(2, 19);
            BitMap = new bool[lengthOfBitMap];
        }

        public void SetBits(string word)
        {
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var initialHashBytes = Hash(inputBytes);
            for (int hashNumber = 0; hashNumber < numberOfHashes; hashNumber++)
            {
                var num = Math.Abs(BitConverter.ToInt32(initialHashBytes, 0) % lengthOfBitMap);
                BitMap[num] = true;
                initialHashBytes = Hash(initialHashBytes);
            }
        }

        public bool WasSet(string word)
        {
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var initialHashBytes = Hash(inputBytes);

            for (int hashNumber = 0; hashNumber < numberOfHashes; hashNumber++)
            {
                var num = Math.Abs(BitConverter.ToInt32(initialHashBytes, 0) % lengthOfBitMap);
                if (!BitMap[num])
                {
                    return false;
                }
                initialHashBytes = Hash(initialHashBytes);

            }

            return true;
        }

        private byte[] Hash(byte[] inputBytes)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(inputBytes);
            }
        }

    }
}
