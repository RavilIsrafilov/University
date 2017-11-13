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
            BigInteger N = 8376451;
            BigInteger wit = WitnessesFinderTwoFactors.GetWitnesses(N);

            N = 2821;
            wit = WitnessesFinderThreeFactors.GetWitnesses(N);

            //Random qwe = new Random(DateTime.Now.Millisecond);

            //var doubleEulerN = Double.Parse(MathMethods.Euler(N).ToString());
            //var doubleWit = Double.Parse(wit.ToString());
            //var divide = (Double)Math.Round(doubleWit / doubleEulerN, 6);           
        }

        
    }

}
