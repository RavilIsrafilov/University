using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using _Data.Math;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger N = 961792614553;
            BigInteger wit = WitnessesFinder.GetWitnesses(N);

            var doubleEulerN = Double.Parse(MathMethods.Euler(N).ToString());
            var doubleWit = Double.Parse(wit.ToString());
            var divide = (Double)Math.Round(doubleWit / doubleEulerN, 6);           
        }

        
    }

}
