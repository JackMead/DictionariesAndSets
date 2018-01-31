using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilters
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        public void Run()
        {
            Console.WriteLine("Loading words");
            var words = LoadWords();
            var bloomFilter = new BloomFilter();
            Console.WriteLine("Preparing BloomFilter");

            foreach (var word in words)
            {
                bloomFilter.SetBits(word);
            }

            Console.WriteLine("Checking if random strings are present in the dictionary:");
            var recognised = 0;
            var actual = 0;
            for (int i = 0; i < 10000; i++)
            {
                if (i % 1000 == 0)
                {
                    Console.WriteLine("{0}/10000",i);
                }
                var testWord = Guid.NewGuid().ToString().Substring(0, 5);

                if (bloomFilter.WasSet(testWord))
                {
                    recognised++;
                    if (words.Contains(testWord))
                    {
                        actual++;
                        Console.WriteLine(testWord);
                    }
                }
            }
            Console.WriteLine("The bloom filter accepted {0} whilst actually {1} were present",recognised, actual);

            Console.WriteLine("Checking actual words present");
            var random = new Random();
            var missed = 0;
            for (int i = 0; i < 10000; i++)
            {
                if (i % 1000 == 0)
                {
                    Console.WriteLine("{0}/10000", i);
                }
                var testWord = words[random.Next(words.Length)];

                if (!bloomFilter.WasSet(testWord))
                {
                    missed++;
                    Console.WriteLine("The algorithm denied having seen {0}", testWord);
                }
            }

            Console.WriteLine("Missed {0} words", missed);

        }

        public string[] LoadWords()
        {
            var listOfWords = new List<string>();
            var path = "Words.txt";
            using (var sr = new StreamReader(path))
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    listOfWords.Add(line);
                }
            }
            return listOfWords.ToArray();
        }
    }
}
