using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        private static float gaus(float x, float y, float GaussSigma)
        {
            return (float)(1 / (2 * Math.PI * Math.Pow(GaussSigma, 2))) * (float)Math.Pow(Math.E, -(Math.Pow(x, 2) + Math.Pow(y, 2)) / (2 * Math.Pow(GaussSigma, 2)));
        }

        private static float gaus1D(float x, float GaussSigma)
        {
            return (float)(1 / Math.Sqrt(2 * Math.PI) * GaussSigma) * (float)Math.Pow(Math.E, -Math.Pow(x, 2) / (2 * Math.Pow(GaussSigma, 2)));
        }

        static void Main(string[] args)
        {

            while (Console.Read() == 0)
            {
                Thread.Sleep(100);
            }

            int size = 18;
            for (int i = 0 - (int)Math.Floor(size / 2.0); i <= (int)Math.Floor(size / 2.0); i++)
            {
                for (int k = 0 - (int)Math.Floor(size / 2.0); k <= (int)Math.Floor(size / 2.0); k++)
                {
                    Console.Write(gaus(k, i, 5.5f) + "; \t");
                }
                Console.Write("\n\n");
            }

            Console.Write("\n---\n");

            for (int i = 0 - (int)Math.Floor(size / 2.0); i <= (int)Math.Floor(size / 2.0); i++)
            {
                Console.Write(gaus1D(i, 5.5f) + "; \t");
            }
            Thread.Sleep(500);

            while (Console.Read() != 0) { }
            while (Console.Read() == 0)
            {
                Thread.Sleep(100);
            }
        }
    }
}
