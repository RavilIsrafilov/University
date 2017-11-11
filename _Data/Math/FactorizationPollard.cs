using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _Data.Math
{
    public class FactorizationPollard
    {
        public static List<BigInteger> FactoryToList(BigInteger N, int iterations)
        {
            MillerRabin millerRabin = new MillerRabin();
            List<BigInteger> factoryNums = new List<BigInteger>();

            if (millerRabin.MillerRabinTest(N, 50))
                return null;

            BigInteger del = N;
            BigInteger numForCycle = N;

            do
            {
                do
                {
                    do {
                        numForCycle = Factory(del, ref iterations);
                    }
                    while (!millerRabin.MillerRabinTest(numForCycle, 500));
                }
                while (numForCycle == del);

                factoryNums.Add(numForCycle);
                del = del / numForCycle;

            }
            while (!millerRabin.MillerRabinTest(del, 500));

            factoryNums.Add(del);

            return factoryNums;
        }
        public static BigInteger Factory(BigInteger N)
        {
            Random random = new Random();

            BigInteger x = new BigInteger(random.Next(1, 500000000));
            BigInteger y = 1; BigInteger i = 0; BigInteger stage = 2;
            while (BigInteger.GreatestCommonDivisor(N, ((x - y) > 0) ? (x - y) : (y - x)) == 1)
            {
                if (i == stage)
                {
                    y = x;
                    stage = stage * 2;
                }

                x = (x * x - 1) % N;
                i = i + 1;
            }
            return BigInteger.GreatestCommonDivisor(N, ((x - y) > 0) ? (x - y) : (y - x));
        }
        public static BigInteger Factory(BigInteger N, ref int iterations)
        {
            Random random = new Random();

            BigInteger x = new BigInteger(random.Next(1, 500000000));
            BigInteger y = 1; BigInteger i = 0; BigInteger stage = 2;
            while (BigInteger.GreatestCommonDivisor(N, ((x - y) > 0) ? (x - y) : (y - x)) == 1)
            {
                iterations++;
                if (i == stage)
                {
                    y = x;
                    stage = stage * 2;
                }
                x = (x * x - 1) % N;
                i = i + 1;
            }
            return BigInteger.GreatestCommonDivisor(N, ((x - y) > 0) ? (x - y) : (y - x));
        }
    }
}
