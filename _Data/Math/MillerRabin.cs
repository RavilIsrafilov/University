using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _Data.Math
{
    public class MillerRabin
    {
        private ExponentiationSquaring expSqr;
        private Random random;
        public MillerRabin()
        {
            expSqr = new ExponentiationSquaring();
            random = new Random();
        }
        public bool MillerRabinTest(BigInteger n, int k)
        {
            if (n <= 1)
                return false;
            if (n == 2)
                return true;
            if (n % 2 == 0)
                return false;
            int s = 0;
            var d = n - 1;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            var rand = new Random();

            BigInteger a = GetRandomMax(n - 1);

            for (int i = 0; i < k; i++)
            {
                var x = expSqr.ModExp(a, d, n);

                if (x == 1 || x == n - 1)
                    continue;

                for (int j = 0; j < s - 1; j++)
                {
                    x = expSqr.ModExp(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;

                a = GetRandomMax(n - 1);    
            }
            return true;
        }
        private BigInteger GetRandomMax(BigInteger max) 
        {
            byte[] byteArr = max.ToByteArray();
            int length = byteArr.Length;

            int bytelength = random.Next(1, length);
            BigInteger p = GetRandomNum(bytelength);

            while (BigInteger.Abs(p) >= max)
                p = GetRandomNum(bytelength);

            if (p < 0) p *= -1;
            if (p == 0) p += 2;
            if (p == 1) p++;

            return p;
        }
        private BigInteger GetRandomNum(int byteLenght)
        {
            byte[] data = new byte[byteLenght];
            random.NextBytes(data);
            return new BigInteger(data);
        }
        public bool TestPrimeNum(BigInteger n, BigInteger a)
        {
            if (n == 1)
                return false;
            if (n == 2)
                return true;
            if (n == 3)
                return true;

            int s = 0;
            BigInteger d = n - 1;

            while (d % 2 == 0)
            {
                s++;
                d /= 2;
            }

            BigInteger x = expSqr.ModExp(a, d, n);

            if (x == 1)
                return true;

            for (int r = 0; r < s; r++)
                if (x == n - 1)
                    return true;
                else
                    x = expSqr.ModExp(x, 2, n);

            if (x == n - 1)
                return true;

            return false;
        }
        public bool MillerRabinTestNew(BigInteger n, int k)
        {
            if (n <= 1)
                return false;
            if (n == 2)
                return true;
            if (n % 2 == 0)
                return false;
            int s = 0;
            var d = n - 1;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            var rand = new Random();

            BigInteger a = GetRandomMax(n - 1);

            for (int i = 0; i < k; i++)
            {

                var x = expSqr.ModExp(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (int j = 0; j < s - 1; j++)
                {
                    x = expSqr.ModExp(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;

                a++;
            }

            return true;

        }

        //public bool MillerRabinTest(BigInteger num, int k)
        //{
        //    if (num <= 1)
        //        return false;
        //    if (num == 2)
        //        return true;
        //    if (num % 2 == 0)
        //        return false;

        //    var t = num - 1;
        //    int s = 0;
        //    while (t % 2 == 0)
        //    {
        //        t /= 2;
        //        s++;
        //    }

        //    for (int i = 0; i < k; i++)
        //    {

        //        var a = GetRandomMax(num - 1);
        //        if (num % a == 0) return false;

        //        var b = expSqr.ModExp(a, t, num);

        //        if ((b != 1) || (b != num - 1)) //продолжаем со следующим а 
        //        {
        //            for (int r = 0; r < s - 1; r++)
        //            {
        //                b = expSqr.ModExp(b, 2, num);
        //                if (b == 1) return false;
        //                if (b == num - 1) break;
        //                if (r == s - 2) return false;
        //            }
        //        }

        //        if (i == k - 1) return true;
        //    }

        //    return false;
        //}


        //public bool MillerRabinTest(BigInteger num, int k)
        //{
        //    if (num <= 1)
        //        return false;
        //    if (num == 2)
        //        return true;
        //    if (num % 2 == 0)
        //        return false;

        //    var t = num - 1;
        //    int s = 0;

        //    while (t % 2 == 0)
        //    {
        //        t /= 2;
        //        s++;
        //    }

        //    for (int i = 0; i < k; i++)
        //    {
        //        var a = GetRandomMax(num - 1);

        //        if (num % a == 0)
        //            return false;

        //        var b = expSqr.ModExp(a, t, num);
        //        if (b != 1)
        //        {
        //            for (int r = 0; r < s; r++)
        //            {
        //                b = expSqr.ModExp(b, 2, num);
        //                if (b == 1) return false;
        //                if (b == num - 1) break;
        //                if (r == s - 1) return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

    }
}
