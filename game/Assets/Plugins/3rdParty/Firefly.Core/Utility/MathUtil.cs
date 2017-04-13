using Firefly.Core.Data;
using System;
using System.Collections.Generic;

namespace Firefly.Core.Utility
{
    public static class MathUtil
    {
        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <param name=\"guid\"></param>  
        /// <returns></returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.ToUniversalTime().Ticks);
        }

        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        private static Random rnd = new Random(GetRandomSeed());

        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public const int MAX_RANDOM = 10000000;

        public static int GetRandom()
        {
            return rnd.Next(0, MAX_RANDOM);
        }

        public static bool CheckRandom(int num)
        {
            return rnd.Next(0, MAX_RANDOM) <= num;
        }

        public static int GetRandom(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public static List<int> GetRandoms(int min, int max, int count)
        {
            List<int> result = new List<int>();
            if (max - min + 1 <= count)
            {
                for (int i = min; i <= max; i++)
                {
                    result.Add(i);
                }
            }
            else if (max - min + 1 > count)
            {
                while (result.Count < count)
                {
                    int rnd = GetRandom(min, max);
                    if (!result.Contains(rnd))
                    {
                        result.Add(rnd);
                    }
                }
            }
            return result;
        }

        public static T CalculateRandomObject<T>(List<T> list)
        {
            if (list == null)
            {
                return default(T);
            }

            if (list.Count <= 0)
            {
                return default(T);
            }

            int result = rnd.Next(0, list.Count);

            return list[result];
        }

        public static T CalculateWeightObject<T>(List<Group<T, int>> list)
        {
            if (list == null)
            {
                return default(T);
            }

            if (list.Count <= 0)
            {
                return default(T);
            }

            int total = 0;
            foreach (Group<T, int> pair in list)
            {
                total += pair.Item2;
            }

            List<Group<int, int>> intervalList = new List<Group<int, int>>(list.Count);

            int count = 0;
            foreach (Group<T, int> group in list)
            {
                if (count == 0)
                {
                    intervalList.Add(new Group<int, int>(0, group.Item2));
                }
                else if (count > 0)
                {
                    int temp_value = intervalList[count - 1].Item2;
                    intervalList.Add(new Group<int, int>(temp_value + 1, temp_value + group.Item2));
                }

                count++;
            }

            int result = rnd.Next(0, total + 1);

            count = 0;
            foreach (Group<int, int> pair in intervalList)
            {
                if (result >= pair.Item1 && result <= pair.Item2)
                {
                    break;
                }

                count++;
            }

            return list[count].Item1;
        }
    }
}
