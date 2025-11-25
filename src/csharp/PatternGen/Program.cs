using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternGen
{
    internal class Program
    {
        const string OUTPUT_PATH = @"..\..\..\..\..\data\";
        const byte BLACK   = 0b00000000; // 0
        const byte BLUE    = 0b00000011; // 1
        const byte RED     = 0b11100000; // 2
        const byte MAGENTA = 0b11100011; // 3
        const byte GREEN   = 0b00011100; // 4
        const byte CYAN    = 0b00011111; // 5
        const byte YELLOW  = 0b11111100; // 6
        const byte WHITE   = 0b11111111; // 7

        static void Main(string[] args)
        {
            var banks = new Dictionary<int, byte[]>();
            int bank = 18;
            banks.Setup(bank++, BLUE)  // Fill 10x 8K layer 2 banks
                .Setup(bank++, GREEN)  // with alternate blue / green stripes
                .Setup(bank++, BLUE)
                .Setup(bank++, GREEN)
                .Setup(bank++, BLUE)
                .Setup(bank++, GREEN)
                .Setup(bank++, BLUE)
                .Setup(bank++, GREEN)
                .Setup(bank++, BLUE)
                .Setup(bank++, GREEN)

                .Setup(bank++, YELLOW) // Fill 10x 8K layer 2 banks
                .Setup(bank++, WHITE)  // with alternate yellow/white stripes
                .Setup(bank++, YELLOW)
                .Setup(bank++, WHITE)
                .Setup(bank++, YELLOW)
                .Setup(bank++, WHITE)
                .Setup(bank++, YELLOW)
                .Setup(bank++, WHITE)
                .Setup(bank++, YELLOW)
                .Setup(bank++, WHITE);

            // 18, 28

            banks.Fill(18, 28, 00, 0, RED,   RED,   1, 3, 2, 3);
            banks.Fill(18, 28, 01, 0, BLACK, BLACK, 1, 3, 2, 3);
            banks.Fill(18, 28, 02, 0, RED,   RED,   2, 2, 3, 2);
            banks.Fill(18, 28, 03, 0, BLACK, BLACK, 2, 2, 3, 2);
            banks.Fill(18, 28, 04, 0, RED,   RED,   0, 0, 2, 3);
            banks.Fill(18, 28, 05, 0, BLACK, BLACK, 0, 0, 2, 3);
            banks.Fill(18, 28, 06, 0, RED,   RED,   0, 0, 3, 2);
            banks.Fill(18, 28, 07, 0, BLACK, BLACK, 0, 0, 3, 2);
            
            banks.Fill(18, 28, 08, 0, RED,   RED,   0, 1, 2, 3);
            banks.Fill(18, 28, 09, 0, BLACK, BLACK, 0, 1, 2, 3);
            banks.Fill(18, 28, 10, 0, RED,   RED,   0, 1, 3, 2);
            banks.Fill(18, 28, 11, 0, BLACK, BLACK, 0, 1, 3, 2);
            banks.Fill(18, 28, 12, 0, RED,   RED,   0, 2, 2, 3);
            banks.Fill(18, 28, 13, 0, BLACK, BLACK, 0, 2, 2, 3);
            banks.Fill(18, 28, 14, 0, RED,   RED,   0, 2, 3, 2);
            banks.Fill(18, 28, 15, 0, BLACK, BLACK, 0, 2, 3, 2);
            
            banks.Fill(18, 28, 16, 0, RED,   RED,   0, 3, 2, 3);
            banks.Fill(18, 28, 17, 0, BLACK, BLACK, 0, 3, 2, 3);
            banks.Fill(18, 28, 18, 0, RED,   RED,   1, 2, 3, 2);
            banks.Fill(18, 28, 19, 0, BLACK, BLACK, 1, 2, 3, 2);
            banks.Fill(18, 28, 20, 0, RED,   RED,   1, 3, 2, 3);
            banks.Fill(18, 28, 21, 0, BLACK, BLACK, 1, 3, 2, 3);
            banks.Fill(18, 28, 22, 0, RED,   RED,   2, 2, 3, 2);
            banks.Fill(18, 28, 23, 0, BLACK, BLACK, 2, 2, 3, 2);

            banks.Fill(18, 28, 24, 0, RED,   RED,   0, 0, 2, 3);
            banks.Fill(18, 28, 25, 0, BLACK, BLACK, 0, 0, 2, 3);
            banks.Fill(18, 28, 26, 0, RED,   RED,   0, 0, 3, 2);
            banks.Fill(18, 28, 27, 0, BLACK, BLACK, 0, 0, 3, 2);
            banks.Fill(18, 28, 28, 0, RED,   RED,   0, 1, 2, 3);
            banks.Fill(18, 28, 29, 0, BLACK, BLACK, 0, 1, 2, 3);
            banks.Fill(18, 28, 30, 0, RED,   RED,   0, 1, 3, 2);
            banks.Fill(18, 28, 31, 0, BLACK, BLACK, 0, 1, 3, 2);

            // 19, 29

            banks.Fill(19, 29, 00, 0, MAGENTA, MAGENTA, 0, 2, 2, 3);
            banks.Fill(19, 29, 01, 0, BLACK,   BLACK,   0, 2, 2, 3);
            banks.Fill(19, 29, 02, 0, MAGENTA, MAGENTA, 0, 2, 3, 2);
            banks.Fill(19, 29, 03, 0, BLACK,   BLACK,   0, 2, 3, 2);
            banks.Fill(19, 29, 04, 0, MAGENTA, MAGENTA, 0, 3, 2, 3);
            banks.Fill(19, 29, 05, 0, BLACK,   BLACK,   0, 3, 2, 3);
            banks.Fill(19, 29, 06, 0, MAGENTA, MAGENTA, 1, 2, 3, 2);
            banks.Fill(19, 29, 07, 0, BLACK,   BLACK,   1, 2, 3, 2);

            banks.Fill(19, 29, 08, 0, MAGENTA, MAGENTA, 1, 3, 2, 3);
            banks.Fill(19, 29, 09, 0, BLACK,   BLACK,   1, 3, 2, 3);
            banks.Fill(19, 29, 10, 0, MAGENTA, MAGENTA, 2, 2, 3, 2);
            banks.Fill(19, 29, 11, 0, BLACK,   BLACK,   2, 2, 3, 2);
            banks.Fill(19, 29, 12, 0, MAGENTA, MAGENTA, 0, 0, 2, 3);
            banks.Fill(19, 29, 13, 0, BLACK,   BLACK,   0, 0, 2, 3);
            banks.Fill(19, 29, 14, 0, MAGENTA, MAGENTA, 0, 0, 3, 2);
            banks.Fill(19, 29, 15, 0, BLACK,   BLACK,   0, 0, 3, 2);

            banks.Fill(19, 29, 16, 0, MAGENTA, MAGENTA, 0, 1, 2, 3);
            banks.Fill(19, 29, 17, 0, BLACK,   BLACK,   0, 1, 2, 3);
            banks.Fill(19, 29, 18, 0, MAGENTA, MAGENTA, 0, 1, 3, 2);
            banks.Fill(19, 29, 19, 0, BLACK,   BLACK,   0, 1, 3, 2);
            banks.Fill(19, 29, 20, 0, MAGENTA, MAGENTA, 0, 2, 2, 3);
            banks.Fill(19, 29, 21, 0, BLACK,   BLACK,   0, 2, 2, 3);
            banks.Fill(19, 29, 22, 0, MAGENTA, MAGENTA, 0, 2, 3, 2);
            banks.Fill(19, 29, 23, 0, BLACK,   BLACK,   0, 2, 3, 2);

            banks.Fill(19, 29, 24, 0, MAGENTA, MAGENTA, 0, 3, 2, 3);
            banks.Fill(19, 29, 25, 0, BLACK,   BLACK,   0, 3, 2, 3);
            banks.Fill(19, 29, 26, 0, MAGENTA, MAGENTA, 1, 2, 3, 2);
            banks.Fill(19, 29, 27, 0, BLACK,   BLACK,   1, 2, 3, 2);
            banks.Fill(19, 29, 28, 0, MAGENTA, MAGENTA, 1, 3, 2, 3);
            banks.Fill(19, 29, 29, 0, BLACK,   BLACK,   1, 3, 2, 3);
            banks.Fill(19, 29, 30, 0, MAGENTA, MAGENTA, 2, 2, 3, 2);
            banks.Fill(19, 29, 31, 0, BLACK,   BLACK,   2, 2, 3, 2);

            foreach (var key in banks.Keys)
            {
                string fn = Path.Combine(OUTPUT_PATH, $"pattern_{key}.bin");
                File.WriteAllBytes(fn, banks[key]);
            }
        }
    }
}
