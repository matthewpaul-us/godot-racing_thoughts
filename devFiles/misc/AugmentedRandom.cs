using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingThoughts.misc
{
    public class AugmentedRandom : Random
    {
        public string Seed { get; private set; }

        public AugmentedRandom()
            :base() { }

        public AugmentedRandom(string seed)
            :base(seed.GetHashCode())
        {
            Seed = seed;
        }

        public T Random<T>(IList<T> items)
        {
            var index = Next(items.Count);

            return items[index];
        }

        public string NextSeed()
        {
            var firstWords = new List<string>()
            {
                "Adorable",
                "Alive",
                "Angry",
                "Broad",
                "Big",
                "Deafening",
                "Ancient",
            };

            var secondWords = new List<string>()
            {
                "Backbone",
                "Hammer",
                "Kiss",
                "Surprise",
                "Father",
                "Dream",
                "Voices",
            };

            return $"{Random(firstWords)} {Random(secondWords)}";
        }
    }
}
