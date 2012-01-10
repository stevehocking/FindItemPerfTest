using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FindItemPerfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration config = CreateConfigurationClass(args);

            if (config == null)
            {
                return;
            }

            TestClass testClass = CreateProgramClass(config);

            Stopwatch stopwatch = new Stopwatch();

            List<long> findTimes = new List<long>();
            List<long> containsTimes = new List<long>();
            List<long> whereTimes = new List<long>();
            List<long> anyTimes = new List<long>();
            List<FindResults> results = new List<FindResults>();

            for (int i = 0; i < config.Repetitions; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();

                results.Add(testClass.DoFind());

                stopwatch.Stop();

                findTimes.Add(stopwatch.ElapsedTicks);

                stopwatch.Reset();
                stopwatch.Start();

                results.Add(testClass.DoContains());

                stopwatch.Stop();

                containsTimes.Add(stopwatch.ElapsedTicks);

                stopwatch.Reset();
                stopwatch.Start();

                results.Add(testClass.DoWhere());

                stopwatch.Stop();

                whereTimes.Add(stopwatch.ElapsedTicks);

                stopwatch.Reset();
                stopwatch.Start();

                results.Add(testClass.DoAny());

                stopwatch.Stop();

                anyTimes.Add(stopwatch.ElapsedTicks);
            }

            Console.WriteLine("Found {0}", results.Average(r => r.FoundPercent).ToString("0%"));
            Console.WriteLine();

            var ticksPerMs = (decimal)Stopwatch.Frequency / 1000;

            Console.WriteLine("ticks per second,{0}", Stopwatch.Frequency);

            Console.WriteLine("test,total,shortest,longest,average");

            Console.WriteLine("find,{0},{1},{2},{3}", findTimes.Sum(), findTimes.Min(), findTimes.Max(), findTimes.Average());
            Console.WriteLine("contains,{0},{1},{2},{3}", containsTimes.Sum(), containsTimes.Min(), containsTimes.Max(), containsTimes.Average());
            Console.WriteLine("where,{0},{1},{2},{3}", whereTimes.Sum(), whereTimes.Min(), whereTimes.Max(), whereTimes.Average());
            Console.WriteLine("any,{0},{1},{2},{3}", anyTimes.Sum(), anyTimes.Min(), anyTimes.Max(), anyTimes.Average());

#if DEBUG
            Console.ReadKey();
#endif
        }

        #region Config load

        protected static TestClass CreateProgramClass(Configuration config)
        {
            return new TestClass(config.ToSearch, config.ToFind, config.SearchCount);
        }

        protected static void DisplayCommandLine()
        {
            Console.WriteLine("{0} <configuration file>", Assembly.GetExecutingAssembly().FullName);
        }

        protected static Configuration CreateConfigurationClass(string[] args)
        {
            if (args.Length != 1 || !IsAConfigFile(args[0]))
            {
                DisplayCommandLine();
                return null;
            }

            return Configuration.LoadConfiguration(args[0]);
        }

        protected static bool IsAConfigFile(string path)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(path)) { }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
