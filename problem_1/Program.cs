using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace problem_1
{
    class Solver
    {
        // Base case
        private static List<UInt32> Solve(UInt32 target, List<UInt32> values)
        {
            foreach (var a in values)
            {
                if (a > target)
                {
                    // All following values will be even larger - fail early
                    return null;
                }

                var b = target - a;

                // Short-circuit in the symmetric case
                if (a == b || values.BinarySearch(b) >= 0)
                {
                    // Success
                    return new List<UInt32> { b, a };
                }
            }

            return null;
        }

        // General case
        public static List<UInt32> Solve(UInt32 target, List<UInt32> values, int height)
        {
            if (height <= 2)
            {
                return Solve(target, values);
            }

            foreach (var a in values)
            {
                if (a >= target)
                {
                    // All following values will be even larger - fail early
                    return null;
                }

                // Note:
                // There is no symmetric case since we have at least 2 more
                // components left

                var b = target - a;
                var result = Solve(b, values, height - 1);
                if (result != null)
                {
                    // Success
                    result.Add(a);
                    return result;
                }
            }

            return null;
        }
    }

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

        static void PrintResult(IEnumerable<UInt32> result)
        {
            if (result is null)
            {
                Console.WriteLine("No solution!");
            }
            else
            {
                var product = result.Select(x => (UInt64)x).Aggregate((x, y) => x * y);
                Console.WriteLine(product);
            }

        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: <program> [problem_input]");
                return;
            }

            var int_list = ReadLines(args[0]).ConvertAll(UInt32.Parse);

            // Values must be sorted for the solver to work
            int_list.Sort();

            const UInt32 target = 2020;
            var part1 = Solver.Solve(target, int_list, 2);
            var part2 = Solver.Solve(target, int_list, 3);

            PrintResult(part1);
            PrintResult(part2);
        }
    }
}
