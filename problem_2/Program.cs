using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace problem_2
{
    class Program
    {
        private static List<string> ReadLines(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                var result = new List<string>();
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    result.Add(s);
                }
                return result;
            }
        }

        private class Entry
        {
            public string Data { get; }
            public int First { get; }
            public int Second { get; }
            public char C { get; }

            public Entry(int first, int second, char c, string data)
            {
                First = first;
                Second = second;
                C = c;
                Data = data;
            }
        }

        private static Entry ParseEntry(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return null;
            }

            // (%d-%d %c: %s)
            const string format = @"([0-9]+)-([0-9]+) ([a-zA-Z]): (.+)";

            var match = Regex.Match(s, format);
            if (!match.Success)
            {
                return null;
            }

            var g = match.Groups;
            return new Entry(int.Parse(g[1].Value), int.Parse(g[2].Value), g[3].Value[0], g[4].Value);
        }

        private static bool ValidatePart1(Entry e)
        {
            var min = e.First;
            var max = e.Second;

            var n_instances = e.Data.Where(x => x == e.C).Count();

            return (min <= n_instances) && (n_instances <= max);
        }

        private static bool ValidatePart2(Entry e)
        {
            if (e.First <= 0 || e.Second <= 0 || e.First > e.Second ||
            e.Second > e.Data.Length)
            {
                return false;
            }

            var first = e.First - 1;
            var second = e.Second - 1;

            // Logical XOR
            return (e.Data[first] == e.C) != (e.Data[second] == e.C);
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: <program> [problem_input]");
                return;
            }

            var entries = ReadLines(args[0]).AsParallel().Select(ParseEntry).Where(x => x != null);

            var part1 = entries.Where(ValidatePart1).Count();
            var part2 = entries.Where(ValidatePart2).Count();

            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }
    }
}
