using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DigitalDesign
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "WarAndPeace.txt";
            string outputFile = $"CounterWords{inputFile}";

            Console.WriteLine($"Counting words in {inputFile}...");

            List<string> lines = ReadAllLines(inputFile);
            List<string> words = lines.AsParallel().SelectMany(line => GetWords(line)).ToList();
            Dictionary<string, int> wordCounts = new Dictionary<string, int>();
            
            foreach (string word in words)
            {
                if (wordCounts.ContainsKey(word))
                    wordCounts[word]++;
                else
                    wordCounts[word] = 1;
            }
            List<KeyValuePair<string, int>> wordPairs = wordCounts.ToList();
            wordPairs.Sort((x, y) => y.Value.CompareTo(x.Value));

            Console.WriteLine($"Writing results to {outputFile}...");
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                foreach (KeyValuePair<string, int> pair in wordPairs)
                {
                    writer.WriteLine($"{pair.Key}\t\t{pair.Value}");
                }
            }
            Console.WriteLine("Done");

        }
        static List<string> ReadAllLines(string inputFile)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }
        static List<string> GetWords(string line)
        {
            Regex regex = new Regex(@"\w+");
            return regex.Matches(line).Cast<Match>().Select(match => match.Value.ToLower()).ToList();
        }
    }
}
