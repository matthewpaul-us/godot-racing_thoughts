using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingThoughts.misc
{
    public class RandomSingleton
    {
        private static AugmentedRandom _random;

        public static AugmentedRandom GetInstance(string seed = null)
        {
            if(_random == null)
            {
                if (seed != null)
                {
                    _random = new AugmentedRandom(seed);
                }
                else
                {
                    _random = new AugmentedRandom();
                }
            }

            return _random;
        }

        public static void SetInstance(AugmentedRandom random)
        {
            _random = random;
        }

    }
}
