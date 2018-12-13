using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    static class RandomGenerator
    {
        static Random random;
        static RandomGenerator()
        {
            random = new Random();
        }

        public static int GetRandom(int min, int max)
        {
            return random.Next(min, max + 1);
        }
    }
}
