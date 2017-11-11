using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using _Data.Math;
using _Data.Models;

namespace _Data.Encryption
{
    public class RSA
    {
        private BigInteger p;
        private BigInteger q;
        private BigInteger e;
        private BigInteger d;

        private BigInteger eyler;
        private BigInteger N;

        private Random random;

        private MontgomeryReducer montRed;
        private MillerRabin millerRabbin;
        private ExponentiationSquaring expSqr;
        int length;

        public RSA(int lenght)
        {
            this.length = lenght / 8;
            init();
        }

        private void init()
        {
            millerRabbin = new MillerRabin();
            random = new Random();
            
            p = GetSimpleNum(length);
            q = GetSimpleNum(length);

            eyler = (p - 1) * (q - 1);
            N = p * q;

            montRed = new MontgomeryReducer(N);
            expSqr = new ExponentiationSquaring();

            //byte[] arr = N.ToByteArray();
            e = GetOpenExponent(2 * length / 3);
            d = MathMethods.GCD(e, eyler);
        }
        private BigInteger GetOpenExponent(int length)
        {
            BigInteger num = N;
            while (eyler % num == 0 || num == p || num == q || num >= N)
                num = GetSimpleNum(length);

            return num;                
        }
        private BigInteger GetSimpleNum(int length)
        {            
            BigInteger bigInt = GetRandomNum(length);

            while (bigInt <= 0 || (bigInt % 2 == 0))
                bigInt = GetRandomNum(length);

            //while (!millerRabbin.TestPrimeNum(bigInt, length))
              //  bigInt += 2;

            //while (!millerRabbin.MillerRabinTestNew(bigInt, length))
              //  bigInt += 2;

            while (millerRabbin.MillerRabinTest(bigInt, length) != true)
              bigInt += 2;

            return bigInt;
        }
        private BigInteger GetRandomNum(int byteLenght)
        {
            byte[] data = new byte[byteLenght];
            random.NextBytes(data);
            return new BigInteger(data);
        }
        public Key GetPublicKey()
        {
            return new Key() { KeyFirst = e, KeySecond = N };
        }
        public Key GetCloseKey()
        {
            return new Key() { KeyFirst = d, KeySecond = N };
        }
        
    }
}
