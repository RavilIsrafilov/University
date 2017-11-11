using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _Data.Math
{
    public class ExponentiationSquaring
    {
        public ExponentiationSquaring()
        { }
        public BigInteger ModExp(BigInteger b, BigInteger e, BigInteger m)
        {
            BigInteger lb = b;
            BigInteger le = e;
            BigInteger lm = m;

            BigInteger result = 1;

            while (le > 0)
            {
                if ((le & 1) == 1)
                    result = ((result % lm) * (lb % lm)) % lm;
                le = le >> 1;
                lb = ((lb % lm) * (lb % lm)) % lm;
            }

            return result;
        }
    }
}
