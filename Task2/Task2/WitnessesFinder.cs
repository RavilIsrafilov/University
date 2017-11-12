using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using _Data.Math;

namespace Task2
{
    public static class WitnessesFinder
    {
        const int iterations = 100;
        public static BigInteger GetWitnesses(BigInteger N)
        {
            List<BigInteger> listSet = GetFirstPoint(N);
            Dictionary<int, Subset> dictionaryBin = GetSecondPoint(listSet);
           
            //third  point
            foreach (var _item in dictionaryBin)
            {
                _item.Value.EylerValue = GetEulerSum(_item.Value.Values);
            }

            //fourth point
            BigInteger witness = 0;
            foreach (var _item in dictionaryBin)
                witness += _item.Value.EylerValue * _item.Value.EylerValue;

            return witness;
        }
        private static List<BigInteger> GetFirstPoint(BigInteger N)
        {
            List<BigInteger> listNumsN = FactorizationPollard.FactoryToList(N, iterations);
            BigInteger p = 0;
            BigInteger q = 0;
            if (listNumsN.Count == 2)
            {
                p = listNumsN[0];
                q = listNumsN[1];
            }

            List<BigInteger> listNumsP = FactorizationPollard.FactoryToList(p - 1, iterations);
            List<BigInteger> listNumsPFull = new List<BigInteger>();
            listNumsPFull.AddRange(listNumsP);
            
            BigInteger _result = 1;
            for (int j = 0; j < listNumsP.Count - 1; j++)
            {
                for (int i = j + 1; i < listNumsP.Count; i++)
                {
                    BigInteger mult = listNumsP[j] * listNumsP[i];
                    _result = _result * mult;

                    if (!listNumsPFull.Contains(mult) && mult <= p - 1 && (p - 1) % mult == 0)
                        listNumsPFull.Add(mult);

                    if (!listNumsPFull.Contains(_result) && _result <= p - 1 && (p - 1) % _result == 0)
                        listNumsPFull.Add(_result);
                    else
                        _result = 1;  
                }
            }

            for (int j = 0; j < listNumsPFull.Count - 1; j++)
            {
                for (int i = j + 1; i < listNumsPFull.Count; i++)
                {
                    _result = listNumsPFull[j] * listNumsPFull[i];
                    if (!listNumsPFull.Contains(_result) && _result <= p - 1 && (p - 1) % _result == 0)
                        listNumsPFull.Add(_result);
                }
            }

            List<BigInteger> listNumsQ = FactorizationPollard.FactoryToList(q - 1, iterations);
            List<BigInteger> listNumsQFull = new List<BigInteger>();
            listNumsQFull.AddRange(listNumsQ);            
            _result = 1;
            for (int j = 0; j < listNumsQ.Count - 1; j++)
            {
                for (int i = j + 1; i < listNumsQ.Count; i++)
                {
                    BigInteger mult = listNumsQ[j] * listNumsQ[i];
                    _result = _result * mult;

                    if (!listNumsQFull.Contains(mult) && mult <= q - 1 && (q - 1) % mult == 0)
                        listNumsQFull.Add(mult);

                    if (!listNumsQFull.Contains(_result) && _result <= q - 1 && (q - 1) % _result == 0)
                        listNumsQFull.Add(_result);
                    else
                        _result = 1;
                }
            }
            _result = 1;
            for (int j = 0; j < listNumsQFull.Count - 1; j++)
            {
                for (int i = j + 1; i < listNumsQFull.Count; i++)
                {
                    _result = listNumsQFull[j] * listNumsQFull[i];
                    if (!listNumsQFull.Contains(_result) && _result <= q - 1 && (q - 1) % _result == 0)
                        listNumsQFull.Add(_result);
                }
            }

            List<BigInteger> listNumsIntersected = listNumsQFull.Intersect(listNumsPFull).ToList();
            listNumsIntersected.Add(1);
            return listNumsIntersected;
        }
        private static Dictionary<int, Subset> GetSecondPoint(List<BigInteger> listSet)
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
