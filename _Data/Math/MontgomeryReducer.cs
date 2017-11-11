using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _Data.Math
{
    class MontgomeryReducer
    {
        public static BigInteger n;
        public static BigInteger r;
        public static BigInteger k;
        public static BigInteger n1;
        
        public MontgomeryReducer(BigInteger mod)
        {
            n = mod;

            BitArray bitArr = new BitArray(n.ToByteArray());
            r = BigInteger.One << bitArr.Length;

            EGCD(r, n, ref n1);
            n1 *= -1;
            n1 &= (r - 1);
            k = bitArr.Length;

        }
        
        private void EGCD(BigInteger a, BigInteger b, ref BigInteger v)
        {
            BigInteger u = 1;
            v = 0;
            BigInteger g = a;
            BigInteger u1 = 0;
            BigInteger v1 = 1;
            BigInteger g1 = b;

            while (g1 != 0)
            {
                BigInteger q = g / g1;
                BigInteger t1 = u - q * u1;
                BigInteger t2 = v - q * v1;
                BigInteger t3 = g - q * g1;
                u = u1; v = v1; g = g1;
                u1 = t1; v1 = t2; g1 = t3;
            }

            if (g != 1)
            {
                u = 0;
            }
        }

        private BigInteger MonPro(BigInteger a, BigInteger b)
        {
            BigInteger t = a * b;
            BigInteger m = ((t & (r - 1)) * n1) & (r - 1);
            BigInteger u = (t + (m * n)) >> (int)k;
            if (u >= n)
                u -= n;

            return u;

        }

        public BigInteger MonExp(BigInteger a, BigInteger degree)
        {
            BitArray bitArr = new BitArray(degree.ToByteArray());
            int k1 = bitArr.Length - 1;
            while (bitArr[k1] == false)
                k1--;

            BitArray bt = bitArr;
            BigInteger x = r % n;
            BigInteger a1 = (a << (int)k) % n;

            for (int j = k1; j >= 0; j--)
            {
                x = MonPro(x, x);
                x = bt[j] ? MonPro(x, a1) : x;
            }
            a = MonPro(x, 1);

            return a;
        }

        public BigInteger MonBinExp(BigInteger a, BigInteger exp)
        {
            BitArray bitArr = new BitArray(n.ToByteArray());
            a %= n;
            BigInteger newa = (a << bitArr.Length) % n;
            BigInteger x = r % n;

            while (exp > 0)
            {
                if ((exp & 1) == 1) x = MonPro(x, newa);
                newa = MonPro(newa, newa);

                exp >>= 1;

            }
            return x = MonPro(x, 1);
        }
    }

}
