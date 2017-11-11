using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using _Data.Encryption;
using System.Text.RegularExpressions;

namespace _Data.Models
{
    public class EncryptUser
    {
        public string Name { get; set; }
        public Key OpenKey { get; }
        private Key CloseKey { get; }
        public Key OpenKeyPartner { get; set; }

        private RSA myRSA;
        private MD5HashHelper hashHelper;
        private RSAMethods myRSAMethods;
        private int lenght;

        public EncryptUser(int lenght)
        {
            this.lenght = lenght;
            myRSA = new RSA(lenght);
            hashHelper = new MD5HashHelper();
            myRSAMethods = new RSAMethods(lenght);

            OpenKey = myRSA.GetPublicKey();
            CloseKey = myRSA.GetCloseKey();
        }
        public BigInteger EncryptMessage(byte[] message)
        {
            try
            {
                return myRSAMethods.EncryptExponentionSquaring(message, OpenKeyPartner);
                //return myRSAMethods.EncryptMontgomery(message, OpenKeyPartner);
                //return myRSAMethods.EncryptMontgomeryBin(message, OpenKeyPartner);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BigInteger.Zero;
            }
        }
        public byte[] DescryptMessage(BigInteger bigNum)
        {
            try
            {
                return myRSAMethods.DescryptExponentionSquaring(bigNum, CloseKey);
                //string messageWithEncryptedCMS = myRSAMethods.DescryptMontgomery(bigNum, CloseKey);
                //string messageWithEncryptedCMS = myRSAMethods.DescryptMontgomeryBin(bigNum, CloseKey);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetHash(string input)
        {
            return hashHelper.GetMd5Hash(input);
        }

        public BigInteger[] EncryptMessage(string message)
        {
            try
            {
                return myRSAMethods.EncryptExponentionSquaring(message, OpenKeyPartner);
                //return myRSAMethods.EncryptMontgomery(message, OpenKeyPartner);
                //return myRSAMethods.EncryptMontgomeryBin(message, OpenKeyPartner);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public string DescryptMessage(BigInteger[] bigNum)
        {
            try
            {
                return myRSAMethods.DescryptExponentionSquaring(bigNum, CloseKey);
                //string messageWithEncryptedCMS = myRSAMethods.DescryptMontgomery(bigNum, CloseKey);
                //string messageWithEncryptedCMS = myRSAMethods.DescryptMontgomeryBin(bigNum, CloseKey);
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }


        public BigInteger[] EncryptMessageWithCMS(string message)
        {
            try
            {
                string hash = hashHelper.GetMd5Hash(message);
                BigInteger[] encryptedHash = myRSAMethods.EncryptExponentionSquaring(hash, CloseKey);
                //BigInteger[] encryptedHash = myRSAMethods.EncryptMontgomery(hash, CloseKey);
                //BigInteger[] encryptedHash = myRSAMethods.EncryptMontgomeryBin(hash, CloseKey);

                string encryptedHashStr = "";
                for (int i = 0; i < encryptedHash.Length; i++)
                    encryptedHashStr += encryptedHash[i].ToString();

                string messageNew = message + "\nCMS = " + encryptedHashStr;
                return myRSAMethods.EncryptExponentionSquaring(messageNew, OpenKeyPartner);
                //return myRSAMethods.EncryptMontgomery(messageNew, OpenKeyPartner);
                //return myRSAMethods.EncryptMontgomeryBin(messageNew, OpenKeyPartner);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public string DescryptMessageWithCMS(BigInteger[] bigNum)
        {
            try
            {
                string messageWithEncryptedCMS = myRSAMethods.DescryptExponentionSquaring(bigNum, CloseKey);
                //string messageWithEncryptedCMS = myRSAMethods.DescryptMontgomery(bigNum, CloseKey);
                //string messageWithEncryptedCMS = myRSAMethods.DescryptMontgomeryBin(bigNum, CloseKey);

                if (messageWithEncryptedCMS != "")
                {
                    string pattern = @"CMS = (\w*)";
                    Regex regex = new Regex(pattern);

                    string encryptedHash = "";

                    foreach (Match match in regex.Matches(messageWithEncryptedCMS))
                        encryptedHash = match.Groups[1].Value;

                    string messageWithoutCMS = messageWithEncryptedCMS.Replace(encryptedHash, "");
                    messageWithoutCMS = messageWithoutCMS.Replace("\nCMS = ", "");

                    BigInteger[] arr = new BigInteger[1];
                    arr[0] = BigInteger.Parse(encryptedHash);

                    if (encryptedHash != "")
                    {
                        string hash = myRSAMethods.DescryptExponentionSquaring(arr, OpenKeyPartner);
                        //string hash = myRSAMethods.DescryptMontgomery(arr, OpenKeyPartner);
                        //string hash = myRSAMethods.DescryptMontgomeryBin(arr, OpenKeyPartner);

                        if (hashHelper.VerifyMd5Hash(messageWithoutCMS, hash))
                            return messageWithoutCMS;
                        else
                            return "Error in check CMS";
                    }

                    else
                        return messageWithoutCMS;
                }
                return null;
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }        
    }
}
