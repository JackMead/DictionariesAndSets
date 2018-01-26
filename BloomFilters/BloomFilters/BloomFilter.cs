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

        public BloomFilter()
        {
            BitMap = new bool[short.MaxValue-short.MinValue+1];
        }

        public void SetBits(string word)
        {
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var initialHashBytes = Hash(inputBytes);
            
            for (int i = 0; i < 8; i++)
            { 
                var newHashBytes = Hash(Hash(initialHashBytes));
                var num = BitConverter.ToInt16(newHashBytes,i*2)-short.MinValue;
                BitMap[num] = true;
            }
        }

        public bool WasSet(string word)
        {
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var initialHashBytes = Hash(inputBytes);

            for (int i = 0; i < 8; i++)
            {
                var newHashBytes = Hash(Hash(initialHashBytes));
                var num = BitConverter.ToInt16(newHashBytes, i * 2) - short.MinValue;
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
