using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternGen.Classes
{
    public static class Colour
    {
        public static byte Black { get; private set; }   = 0b00000000; // 0
        public static byte Blue { get; private set; }    = 0b00000011; // 1
        public static byte Red { get; private set; }     = 0b11100000; // 2
        public static byte Magenta { get; private set; } = 0b11100011; // 3
        public static byte Green { get; private set; }   = 0b00011100; // 4
        public static byte Cyan { get; private set; }    = 0b00011111; // 5
        public static byte Yellow { get; private set; }  = 0b11111100; // 6
        public static byte White { get; private set; }   = 0b11111111; // 7

        public static Color FromRgb332(byte color)
        {
            int r3 = (color >> 5) & 0b111;
            int g3 = (color >> 2) & 0b111;
            int b3 = (color << 1) & 0b110;
            b3 = b3 == 0 ? b3 : (b3 | 0b1);
            int r8 = (r3 << 5) | (r3 << 2) | (r3 >> 1);
            int g8 = (g3 << 5) | (g3 << 2) | (g3 >> 1);
            int b8 = (b3 << 5) | (b3 << 2) | (b3 >> 1);
            return Color.FromArgb(r8, g8, b8);
        }

    }
}
