using System.Collections.Generic;

namespace PatternGen
{
    public class Pattern
    {
        public int FirstA { get; private set; }
        public int FirstB { get; private set; }
        public int RepeatA { get; private set; }
        public int RepeatB { get; private set; }

        private Pattern(int firstA, int firstB, int repeatA, int repeatB)
        {
            FirstA = firstA;
            FirstB = firstB;
            RepeatA = repeatA;
            RepeatB = repeatB;
        }

        public static Pattern[] Create()
        {
            return new List<Pattern>
            {
                new Pattern(1, 3, 2, 3),
                new Pattern(2, 2, 3, 2),
                new Pattern(0, 0, 2, 3),
                new Pattern(0, 0, 3, 2),
                new Pattern(0, 1, 2, 3),
                new Pattern(0, 1, 3, 2),
                new Pattern(0, 2, 2, 3),
                new Pattern(0, 2, 3, 2),
                new Pattern(0, 3, 2, 3),
                new Pattern(1, 2, 3, 2)
            }.ToArray();
        }
    }
}