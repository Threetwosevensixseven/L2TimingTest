using System.Collections.Generic;
using System.IO;

namespace PatternGen
{
    internal class Program
    {
        const string IMAGE_PATH = @"..\..\..\..\..\images\resized\";
        const string DATA_PATH = @"..\..\..\..\..\data\";
        const int SEED = 545345;

        const byte BLACK   = 0b00000000; // 0
        const byte BLUE    = 0b00000011; // 1
        const byte RED     = 0b11100000; // 2
        const byte MAGENTA = 0b11100011; // 3
        const byte GREEN   = 0b00011100; // 4
        const byte CYAN    = 0b00011111; // 5
        const byte YELLOW_ = 0b11111100; // 6
        const byte WHITE   = 0b11111111; // 7

        static void Main()
        {
            new Dictionary<int, byte[]>()
                .FillRandom8KbBanks(18, 20, SEED, BLACK, WHITE)
                .CreatePattern(18, 28, RED, MAGENTA, BLACK)
                .SaveAsPng24_320x256(18, Path.Combine(IMAGE_PATH, "pattern_{startBank}-{endBank}.png"))
                .SaveAsPng24_320x256(28, Path.Combine(IMAGE_PATH, "pattern_{startBank}-{endBank}.png"))
                .SaveAs8KbBanks(Path.Combine(DATA_PATH, "pattern_{bankNo}.bin"));
        }
    }
}
