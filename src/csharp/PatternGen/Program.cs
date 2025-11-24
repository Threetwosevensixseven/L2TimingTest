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
        const byte MAGENTA = 0b11111111; // 3
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
                .Setup(bank++, BLACK)  // with alternate yellow/black stripes
                .Setup(bank++, YELLOW)
                .Setup(bank++, BLACK)
                .Setup(bank++, YELLOW)
                .Setup(bank++, BLACK)
                .Setup(bank++, YELLOW)
                .Setup(bank++, BLACK)
                .Setup(bank++, YELLOW)
                .Setup(bank++, BLACK);

            banks.Fill(18, 28, 1, 0, RED, RED, 2, 2, 3, 2);
            banks.Fill(18, 28, 2, 0, BLACK, BLACK, 2, 2, 3, 2);
            banks.Fill(18, 28, 3, 0, RED, RED, 0, 0, 2, 3);
            banks.Fill(18, 28, 4, 0, BLACK, BLACK, 0, 0, 2, 3);
            banks.Fill(18, 28, 5, 0, RED, RED, 0, 0, 3, 2);
            banks.Fill(18, 28, 6, 0, BLACK, BLACK, 0, 0, 3, 2);
            banks.Fill(18, 28, 7, 0, RED, RED, 0, 1, 2, 3);
            banks.Fill(18, 28, 8, 0, BLACK, BLACK, 0, 1, 2, 3);

            foreach (var key in banks.Keys)
            {
                string fn = Path.Combine(OUTPUT_PATH, $"pattern_{key}.bin");
                File.WriteAllBytes(fn, banks[key]);
            }
        }
    }
}
