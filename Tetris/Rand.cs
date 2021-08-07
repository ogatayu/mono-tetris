// Make random number.

namespace Tetris
{
    class Rand
    {
        private const uint multiplier = 8513;
        private const uint addend = 179;

        public static uint NextRandInt(ref uint v)
        {
            v = v * multiplier + addend;
            return v;
        }

    }
}
