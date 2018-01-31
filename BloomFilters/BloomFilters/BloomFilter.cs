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
        public BloomFilter()
        {
            lengthOfBitMap = short.MaxValue;
            BitMap = new bool[lengthOfBitMap];
        }

        public void SetBits(string word)
        {
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var initialHashBytes = Hash(inputBytes);
            //Console.WriteLine(initialHashBytes.Length);
            for (int i = 0; i < 4; i++)
            { 
                var num = Math.Abs(BitConverter.ToInt32(initialHashBytes,i*4)%lengthOfBitMap);
                //Console.WriteLine(num);
                BitMap[num] = true;
            }
        }

        public bool WasSet(string word)
        {
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var initialHashBytes = Hash(inputBytes);

            for (int i = 0; i < 4; i++)
            {
                var num = Math.Abs(BitConverter.ToInt32(initialHashBytes, i * 4)%lengthOfBitMap);
                if (!BitMap[num])
                {
                    return false;
                } 
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
