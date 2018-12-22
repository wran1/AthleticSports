using System;
using System.Security.Cryptography;

namespace Services
{
    public static class CommonCodeGenerator
    {
        private static Random Random
        {
            get
            {
                var randomNumberBuffer = new byte[10];
                new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
                return new Random(BitConverter.ToInt32(randomNumberBuffer, 0));
            }
        }
        /// <summary>
        /// 获取随机整数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInt(int min = 0, int max = int.MaxValue)
        {
            return Random.Next(min, max);
        }
    }
}
