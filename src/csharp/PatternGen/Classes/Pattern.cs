using System.Collections.Generic;

namespace PatternGen.Classes
{
    public class Pattern
    {
        public int FirstA { get; private set; }
        public int FirstB { get; private set; }
        public int RepeatA { get; private set; }
        public int RepeatB { get; private set; }

        public Pattern(int firstA, int firstB, int repeatA, int repeatB)
        {
            FirstA = firstA;
            FirstB = firstB;
            RepeatA = repeatA;
            RepeatB = repeatB;
        }
    }
}