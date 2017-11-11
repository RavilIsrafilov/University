using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using _Data.Math;
using _Data.Models;

namespace _Data.Encryption
{
    public class RSAMethods
    {
        private int lenghtString;

        private MontgomeryReducer montRed;
        private ExponentiationSquaring expSqr;

        public RSAMethods(int lenght)
        {
            lenghtString = lenght / 16;//2*len/8 -1
            expSqr = new ExponentiationSquaring();
        }
        public BigInteger EncryptExponentionSquaring(byte[] array, Key key)
        {
            BigInteger bigNumArray = new BigInteger();
            bigNumArray = expSqr.ModExp(new BigInteger(array), key.KeyFirst, key.KeySecond);
            return bigNumArray;
        }
        public byte[] DescryptExponentionSquaring(BigInteger encryptedMessage, Key key)
        {
            return expSqr.ModExp(encryptedMessage, key.KeyFirst, key.KeySecond).ToByteArray();
        }

        public BigInteger[] EncryptExponentionSquaring(string message, Key key)
        {
            int numArrayBlock = (message.Length / lenghtString) + 1;
            string[] arrayString = new string[numArrayBlock];
            BigInteger[] bigNumArray = new BigInteger[numArrayBlock];

            for (int i = 0; i < arrayString.Length; i++) 
            {
                if (numArrayBlock - i == 1)
                    arrayString[i] = message.Substring(i * lenghtString, message.Length - i * lenghtString);
                else
                    arrayString[i] = message.Substring(i * lenghtString, lenghtString);

                byte[] s = System.Text.Encoding.UTF8.GetBytes(arrayString[i]);
                bigNumArray[i] = expSqr.ModExp(new BigInteger(s), key.KeyFirst, key.KeySecond);
            }

            return bigNumArray;
        }
        public BigInteger[] EncryptMontgomeryBin(string message, Key key)
        {
            montRed = new MontgomeryReducer(key.KeySecond);

            int numArrayBlock = (message.Length / lenghtString) + 1;
            string[] arrayString = new string[numArrayBlock];
            BigInteger[] bigNumArray = new BigInteger[numArrayBlock];

            for (int i = 0; i < arrayString.Length; i++)
            {
                if (numArrayBlock - i == 1)
                    arrayString[i] = message.Substring(i * lenghtString, message.Length - i * lenghtString);
                else
                    arrayString[i] = message.Substring(i * lenghtString, lenghtString);

                byte[] s = System.Text.Encoding.UTF8.GetBytes(arrayString[i]);
                bigNumArray[i] = montRed.MonBinExp(new BigInteger(s), key.KeyFirst);
            }

            return bigNumArray;
        }
        public BigInteger[] EncryptMontgomery(string message, Key key)
        {
            montRed = new MontgomeryReducer(key.KeySecond);

            int numArrayBlock = (message.Length / lenghtString) + 1;
            string[] arrayString = new string[numArrayBlock];
            BigInteger[] bigNumArray = new BigInteger[numArrayBlock];

            for (int i = 0; i < arrayString.Length; i++)
            {
                if (numArrayBlock - i == 1)
                    arrayString[i] = message.Substring(i * lenghtString, message.Length - i * lenghtString);
                else
                    arrayString[i] = message.Substring(i * lenghtString, lenghtString);

                byte[] s = System.Text.Encoding.UTF8.GetBytes(arrayString[i]);
                bigNumArray[i] = montRed.MonExp(new BigInteger(s), key.KeyFirst);
            }


            return bigNumArray;
            
        }
        public string DescryptExponentionSquaring(BigInteger[] encryptedMessage, Key key)
        {
            string message = "";
            for (int i = 0; i < encryptedMessage.Length; i++)
            {
                message += System.Text.Encoding.UTF8.GetString
                    (expSqr.ModExp(encryptedMessage[i], key.KeyFirst, key.KeySecond).ToByteArray());
            }

            return message;
        }
        public string DescryptMontgomery(BigInteger[] encryptedMessage, Key key)
        {
            montRed = new MontgomeryReducer(key.KeySecond);

            string message = "";
            for (int i = 0; i < encryptedMessage.Length; i++)
            {
                message += System.Text.Encoding.UTF8.GetString
                    (montRed.MonExp(encryptedMessage[i], key.KeyFirst).ToByteArray());
            }

            return message;
        }
        public string DescryptMontgomeryBin(BigInteger[] encryptedMessage, Key key)
        {
            montRed = new MontgomeryReducer(key.KeySecond);

            string message = "";
            for (int i = 0; i < encryptedMessage.Length; i++)
            {
                message += System.Text.Encoding.UTF8.GetString
                    (montRed.MonBinExp(encryptedMessage[i], key.KeyFirst).ToByteArray());
            }

            return message;
        }

    }
}
