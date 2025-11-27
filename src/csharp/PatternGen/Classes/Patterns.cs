using System;
using System.Collections.Generic;

namespace PatternGen.Classes
{
    public class Patterns
    {
        private static Pattern[] _vertical = null;
        private static Pattern[] _horizontal = null;
        private static byte[] _randomBW = null;
        private static byte[] _randomColour = null;

        public static Pattern[] Vertical
        {
            get
            {
                if (_vertical == null)
                {
                    _vertical = new List<Pattern>
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
                return _vertical;
            }
        }

        public static Pattern[] Horizontal
        {
            get
            {
                if (_horizontal == null)
                {
                    _horizontal = new List<Pattern>
                    {
                        new Pattern(0, 8, 10, 10),
                        new Pattern(2, 10, 10, 10),
                        new Pattern(6, 10, 10, 10),
                        new Pattern(0, 0, 10, 10),
                        new Pattern(0, 4, 10, 10)
                    }.ToArray();
                }
                return _horizontal;
            }
        }

        public static byte[] RandomBW
        {
            get
            {
                if (_randomBW == null)
                {
                    _randomBW = new byte[] { Colour.Black, Colour.White };
                }
                return _randomBW;
            }
        }

        public static byte[] RandomColour
        {
            get
            {
                if (_randomColour == null)
                {
                    var list = new List<byte>();
                    for (int i = 0; i < 256; i++)
                    {
                        list.Add(Convert.ToByte(i));
                    }
                    _randomColour = list.ToArray();
                }
                return _randomColour;
            }
        }
    }
}
