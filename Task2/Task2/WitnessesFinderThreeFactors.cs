using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using _Data.Math;

namespace Task2
{
    public class WitnessesFinderThreeFactors
    {
        const int iterations = 100;
        public static BigInteger GetWitnesses(BigInteger N)
        {
            List<BigInteger>[] listSet = GetFirstPoint(N);
            Dictionary<int, Subset>[] dictionaryBin = GetSecondPoint(listSet);

            //third  point
            //foreach (var _item in dictionaryBin)
            //{
            //    _item.Value.EylerValue = GetEulerSum(_item.Value.Values);
            //}

            ////fourth point
            BigInteger witness = 0;
            //foreach (var _item in dictionaryBin)
            //    witness += _item.Value.EylerValue * _item.Value.EylerValue;

            return witness;
        }
        private static List<BigInteger>[] GetFirstPoint(BigInteger N)
        {
            List<BigInteger> listNumsN = FactorizationPollard.FactoryToList(N, iterations);
            BigInteger p = 0;
            BigInteger q = 0;
            BigInteger k = 0;

            if (listNumsN.Count == 3)
            {
                p = listNumsN[0];
                q = listNumsN[1];
                k = listNumsN[2];
            }

            List<BigInteger> listNumsPFull = GetAllDivisiors(p - 1);
            List<BigInteger> listNumsQFull = GetAllDivisiors(q - 1);
            List<BigInteger> listNumsKFull = GetAllDivisiors(k - 1);

            List<BigInteger> listNumsPQ = GetAllDivisiors(N/k - 1);
            List<BigInteger> listNumsPK = GetAllDivisiors(N/q - 1);
            List<BigInteger> listNumsKQ = GetAllDivisiors(N/p - 1);

            listNumsPQ = listNumsPQ.Intersect(listNumsKFull).ToList();
            listNumsPK = listNumsPK.Intersect(listNumsQFull).ToList();
            listNumsKQ = listNumsKQ.Intersect(listNumsKFull).ToList();

            return new List<BigInteger>[] { listNumsPQ, listNumsPK, listNumsKQ };
        }
        private static List<BigInteger> GetAllDivisiors(BigInteger num)
        {
            List<BigInteger> listNums = FactorizationPollard.FactoryToList(num, iterations);
            List<BigInteger> listNumsFull = new List<BigInteger>();
            listNumsFull.AddRange(listNums);

            BigInteger _result = 1;
            for (int j = 0; j < listNums.Count - 1; j++)
            {
                for (int i = j + 1; i < listNums.Count; i++)
                {
                    BigInteger mult = listNums[j] * listNums[i];
                    _result = _result * mult;

                    if (!listNumsFull.Contains(mult) && mult <= num && num % mult == 0)
                        listNumsFull.Add(mult);

                    if (!listNumsFull.Contains(_result) && _result <= num && num % _result == 0)
                        listNumsFull.Add(_result);
                    else
                        _result = 1;
                }
            }

            for (int j = 0; j < listNumsFull.Count - 1; j++)
            {
                for (int i = j + 1; i < listNumsFull.Count; i++)
                {
                    _result = listNumsFull[j] * listNumsFull[i];
                    if (!listNumsFull.Contains(_result) && _result <= num && num % _result == 0)
                        listNumsFull.Add(_result);
                }
            }
            listNumsFull.Add(1);
            listNumsFull.Sort();
            return listNumsFull.Distinct().ToList(); 
        }

        private static Dictionary<int, Subset>[] GetSecondPoint(List<BigInteger>[] listSet)
        {
            Dictionary<int, Subset>[] result = new Dictionary<int, Subset>[3];
            for (int i = 0; i < 3; i++)
                result[i] = GetGroupedDictionaryByBin(listSet[i]);
            return result;
        }
        private static Dictionary<int, Subset> GetGroupedDictionaryByBin(List<BigInteger> listSet)
        {
            Dictionary<int, Subset> dictionaryBin = new Dictionary<int, Subset>();
            int maxBin = 0;
            foreach (var item in listSet)
            {
                var currentBin = MathMethods.Bin(item);
                if (dictionaryBin.ContainsKey(currentBin))
                    dictionaryBin[currentBin].Values.Add(item);
                else
                {
                    Subset _subset = new Subset();
                    _subset.Values = new List<BigInteger>();
                    _subset.Values.Add(item);
                    dictionaryBin.Add(currentBin, _subset);
                }
                if (maxBin < currentBin)
                    maxBin = currentBin;
            }
            return dictionaryBin;
        }

        private static BigInteger GetEulerSum(List<BigInteger> _list)
        {
            BigInteger result = 0;
            foreach (var _item in _list)
                result += MathMethods.Euler(_item);
            return result;
        }
    }
}
