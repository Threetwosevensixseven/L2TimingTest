using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PatternGen
{
    public static class BankExtensions
    {
        const int BANK_SIZE = 8 * 1024;

        public static Dictionary<int, byte[]> Setup(this Dictionary<int, byte[]> banks, int bankNo, byte colour)
        {
            var bytes = new byte[BANK_SIZE];
            for (int i = 0; i < BANK_SIZE; i++)
                bytes[i] = colour;
            banks.Add(bankNo, bytes);
            return banks;
        }

        public static Dictionary<int, byte[]> FillRandom8KbBanks(this Dictionary<int, byte[]> banks, int startBank, int bankCount, 
            int seed, params byte[] colours)
        {
            var rng = new Random(seed);
            for (int i = 0; i < bankCount; i++)
            {
                var bank = new byte[BANK_SIZE];
                for(int j = 0; j < bank.Length; j++)
                    bank[j] = colours[rng.Next(colours.Length)];
                banks.Add(startBank++, bank);
            }
            return banks;
        }

        public static Dictionary<int, byte[]> Fill(this Dictionary<int, byte[]> banks, int bankA, int bankB,
            int x, int y, byte colorA, byte colorB, int firstA, int firstB, int repeatA, int repeatB)
        {
            int addr = (x * 256) + y;
            int end = addr + 256;
            for (int i = 0; i < firstA; i++)
            {
                banks[bankA][addr++] = colorA;
            }
            for (int i = 0; i < firstB; i++)
            {
                banks[bankB][addr++] = colorB;
            }
            bool done = false;
            while (!done)
            {
                for (int i = 0; i < repeatA; i++)
                {
                    if (addr >= end)
                    {
                        done = true;
                        break;
                    }
                    banks[bankA][addr++] = colorA;
                }
                for (int i = 0; i < repeatB; i++)
                {
                    if (addr >= end)
                    {
                        done = true;
                        break;
                    }
                    banks[bankB][addr++] = colorB;
                }
            }
            return banks;
        }

        public static Dictionary<int, byte[]> CreatePattern(this Dictionary<int, byte[]> banks, int startBankA, int startBankB,
            byte colorA, byte colorB, byte colorC)
        {
            var patterns = Pattern.Create();
            for (int x = 0; x < 320; x++)
            {
                int y = 0;
                int bankA = startBankA + (x / 32);
                int bankB = startBankB + (x / 32);
                int addr = ((x % 32) * 256) + y;
                int end = addr + 256;
                var pattern = patterns[(x / 2) % patterns.Length];
                int bankSeq = x / 32;
                byte colorEven = ((bankSeq / 2) * 2) == bankSeq ? colorA : colorB;
                byte color = ((x / 2) * 2) == x ? colorEven : colorC;

                for (int i = 0; i < pattern.FirstA; i++)
                {
                    banks[bankA][addr++] = color;
                }
                for (int i = 0; i < pattern.FirstB; i++)
                {
                    banks[bankB][addr++] = color;
                }
                bool done = false;
                while (!done)
                {
                    for (int i = 0; i < pattern.RepeatA; i++)
                    {
                        if (addr >= end)
                        {
                            done = true;
                            break;
                        }
                        banks[bankA][addr++] = color;
                    }
                    for (int i = 0; i < pattern.RepeatB; i++)
                    {
                        if (addr >= end)
                        {
                            done = true;
                            break;
                        }
                        banks[bankB][addr++] = color;
                    }
                }
            }
            return banks;
        }

        public static Dictionary<int, byte[]> SaveAsPng24_320x256(this Dictionary<int, byte[]> banks, int startBank, string fileName)
        {
            var pal = new Dictionary<byte, Color>();
            using (var img = new Bitmap(320, 256))
            {
                FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
                int frameCount = img.GetFrameCount(dimension);
                for (int i = 0; i < frameCount;)
                {
                    img.SelectActiveFrame(dimension, i);

                    // A 320x256 layer 2 image is 80K in size, with 10x 8K stripes going left to right across the page.
                    // Within each stripe, the X axis of the oroginal image goes from top to bottom,
                    // and the Y axis goes from left to right. So it looks like the image is flopped horizonally, then rotated 90 anticlockwise.
                    // We need two loops to process the pixels. The outer loop should be the original image X axis,
                    for (int x = 0; x < img.Width; x++)
                    {
                        // and the inner loop should be the original image Y axis.
                        for (int y = 0; y < img.Height; y++)
                        {
                            // Get the bank and byte corresponding to the pixel
                            int bank = startBank + (x / 32);
                            int byteIndex = ((x % 32) * 256) + y;
                            byte pixel = banks[bank][byteIndex];
                            if (!pal.ContainsKey(pixel))
                            {
                                // Inside these two loops, we can set each pixel in turn
                                // In .NET, these coloura is ARGB888
                                // Assume pixel in bank is %RRRGGGBB if we don't have an explicit palette
                                int r3 = (pixel >> 5) & 0b111;
                                int g3 = (pixel >> 2) & 0b111;
                                int b3 = (pixel << 1) & 0b110;
                                b3 = b3 == 0 ? b3 : (b3 | 0b1);
                                int r8 = (r3 << 5) | (r3 << 2) | (r3 >> 1);
                                int g8 = (g3 << 5) | (g3 << 2) | (g3 >> 1);
                                int b8 = (b3 << 5) | (b3 << 2) | (b3 >> 1);
                                pal.Add(pixel, Color.FromArgb(r8, g8, b8));
                            }
                            img.SetPixel(x, y, pal[pixel]);
                        }
                    }
                    break;
                }
                fileName = fileName.Replace("{startBank}", startBank.ToString())
                    .Replace("{endBank}", (startBank + 9).ToString());
                img.Save(fileName, ImageFormat.Png);
            }
            return banks;
        }

        public static Dictionary<int, byte[]> SaveAs8KbBanks(this Dictionary<int, byte[]> banks, string fileName)
        {
            foreach (int bankNo in banks.Keys)
            {
                string fn = fileName.Replace("{bankNo}", bankNo.ToString());
                File.WriteAllBytes(fn, banks[bankNo]);
            }
            return banks;
        }
    }
}
