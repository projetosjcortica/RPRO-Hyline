using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCortica_RPRO.Leonardo.Services
{
    public class RandomGenerator
    {
        private static Random _random = new Random();

        public static int GetRandomNumber()
        {
            return _random.Next();
        }

        public static int GetRandomNumberInRange(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
