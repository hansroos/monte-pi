using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MonteCarlo
{
    class Program
    {
        private static int numberOfThreads = 64;
        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();
            
            var simulations = new List<Task<double>>();

            for (int i = 0; i < numberOfThreads; i++)
            {
                var task = Task<double>.Run(() => new MontePi().Run());
                simulations.Add(task);
            }

            Task.WaitAll(simulations.ToArray());

            double pi = 0.0;
            foreach(var s in simulations)
            {
                pi += s.Result;
            }
            pi = pi / numberOfThreads;

            timer.Stop();

            Console.WriteLine($"My Pi: {pi}");
            Console.WriteLine($"Real Pi: {Math.PI}");
            
            Console.WriteLine($"Time: {timer.ElapsedMilliseconds/1000.0}s");
        }
    }

    class MontePi
    {
        private static int iterations = 100000000;

        public double Run()
        {
            double inner = 0;
            double outer = 0;
            var r = new Random((int)(DateTime.Now.Ticks));
            
            for (int i = 0; i < iterations; i++)
            {
                if (InCircle(r.NextDouble(), r.NextDouble()))
                {
                    inner++;
                }

                outer++;
            }

            double pi = 4 * (inner) / (outer);

            return pi;
        }

        private static bool InCircle(double x, double y)
        {           
           var r = Math.Sqrt(x * x + y * y);
           return r <= 1.0;
        }
    }
}
