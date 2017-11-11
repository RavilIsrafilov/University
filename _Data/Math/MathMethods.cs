using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _Data.Math
{
    public static class MathMethods
    {
        public static BigInteger GCD(BigInteger e, BigInteger m)
        {
            BigInteger i = m, v = 0, d = 1;
            while (e > 0)
            {
                BigInteger t = i / e, x = e;
                e = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= m;
            if (v < 0) v = (v + m) % m;
            return v;
        }

        public static BigInteger GCD(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
        {
            if (a == 0)
            {
                y = 0; x = 1;
                return b;
            }

            BigInteger x1 = 0, y1 = 1;
            BigInteger d = GCD(b % a, a, ref x1, ref y1);
            x = y1;
            y = x1 - (b / a) * y1;

            return d;

        }

        public static int Bin(BigInteger N)
        {
            int result = 0;
            for (int i = 2; true; i = i * 2) 
            {
                if (N % i != 0)
                    return result;
                result ++;
            }
        }

        public static BigInteger Euler(BigInteger n)
        {
            if (n == 1)
                return 1;
            int i, m = 1;
            for (i = 2; i <= n / 2; i++)
                if (n % i == 0)
                {
                    n /= i;
                    while (n % i == 0)
                    {
                        m *= i;
                        n /= i;
                    }
                    if (n == 1) return m * (i - 1);
                    else return m * (i - 1) * Euler(n);
                }
            return n - 1;
        }
    }
}
