using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingThoughts.misc
{
    public class RandomSingleton
    {
        private static Random _random;

        public static Random GetInstance(string seed = null)
        {
            if(_random == null)
            {
                if (seed != null)
                {
                    _random = new Random(seed.GetHashCode());
                }
                else
                {
                    _random = new Random();
                }
            }

            return _random;
        }

    }
}
