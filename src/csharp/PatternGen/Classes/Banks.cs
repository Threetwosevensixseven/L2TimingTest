using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;

namespace PatternGen.Classes
{
    public class Banks
    {
        const int BANK_SIZE = 8 * 1024;
        const int SEED = 545345;
        const int CODE_INDENT = 25;
        const int COMMENT_INDENT = 67;
        const int SPEED14_REG18_COUNT = 11676;

        private readonly Dictionary<int, byte[]> _banks = new Dictionary<int, byte[]>();
        private readonly StringBuilder _sb = new StringBuilder();
        private string _hPatternName = string.Empty;
        private int _reg18Count = 0;

        public Banks Create(int startBank, int bankCount)
        {
            for (int i = 0; i < bankCount; i++)
            {
                var bank = new byte[BANK_SIZE];
                _banks.Add(startBank++, bank);
            }
            return this;
        }

        public Banks FillSolid(int startBank, int bankCount, byte color)
        {
            for (int i = 0; i < bankCount; i++)
            {
                var bank = _banks[startBank + i];
                for (int j = 0; j < bank.Length; j++)
                    bank[j] = color;
            }
            return this;
        }

        public Banks FillRandom(params byte[] colours)
        {
            var rng = new Random(SEED);
            foreach (var bank in _banks)
            {
                for (int j = 0; j < bank.Value.Length; j++)
                    bank.Value[j] = colours[rng.Next(colours.Length)];
            }
            return this;
        }

        public Banks FillRow(int startBankA, int startBankB, int y, byte colorA, byte colorB, int firstA, int firstB, int repeatA, int repeatB)
        {
            int x = 0;
            int count = 1;
            while (x < 320 && count <= firstA)
            {
                int bank = startBankA + (x / 32);
                int addr = ((x % 32) * 256) + y;
                bool evenBank = x / 64 * 2 == x / 32;
                byte color = evenBank ? colorA : colorB;
                _banks[bank][addr] = color;
                count++;
                x++;
            }
            count = 1;
            while (x < 320 && count <= firstB)
            {
                int bank = startBankB + (x / 32);
                int addr = ((x % 32) * 256) + y;
                bool evenBank = x / 64 * 2 == x / 32;
                byte color = evenBank ? colorA : colorB;
                _banks[bank][addr] = color;
                count++;
                x++;
            }
            while(x < 320)
            {
                count = 1;
                while (x < 320 && count <= repeatA)
                {
                    int bank = startBankA + (x / 32);
                    int addr = ((x % 32) * 256) + y;
                    bool evenBank = x / 64 * 2 == x / 32;
                    byte color = evenBank ? colorA : colorB;
                    _banks[bank][addr] = color;
                    count++;
                    x++;
                }
                count = 1;
                while (x < 320 && count <= repeatB)
                {
                    int bank = startBankB + (x / 32);
                    int addr = ((x % 32) * 256) + y;
                    bool evenBank = x / 64 * 2 == x / 32;
                    byte color = evenBank ? colorA : colorB;
                    _banks[bank][addr] = color;
                    count++;
                    x++;
                }
            }
            return this;
        }

        public Banks FillHorizontal(int startBankA, int startBankB, byte colorA, byte colorB, byte ColorC)
        {
            var patterns = Patterns.Horizontal;
            for (int y = 0; y < 256; y++)
            {
                var pattern = patterns[y % patterns.Length];
                bool evenRow = y % 2 == 0;
                int x = 0;
                int count = 1;
                while (x < 320 && count <= pattern.FirstA)
                {
                    int bank = startBankA + (x / 32);
                    int addr = ((x % 32) * 256) + y;
                    bool evenBank = x / 64 * 2 == x / 32;
                    byte color = evenBank ? colorA : colorB;
                    color = evenRow ? color : ColorC;
                    _banks[bank][addr] = color;
                    count++;
                    x++;
                }
                count = 1;
                while (x < 320 && count <= pattern.FirstB)
                {
                    int bank = startBankB + (x / 32);
                    int addr = ((x % 32) * 256) + y;
                    bool evenBank = x / 64 * 2 == x / 32;
                    byte color = evenBank ? colorA : colorB;
                    color = evenRow ? color : ColorC;
                    _banks[bank][addr] = color;
                    count++;
                    x++;
                }
                while (x < 320)
                {
                    count = 1;
                    while (x < 320 && count <= pattern.RepeatA)
                    {
                        int bank = startBankA + (x / 32);
                        int addr = ((x % 32) * 256) + y;
                        bool evenBank = x / 64 * 2 == x / 32;
                        byte color = evenBank ? colorA : colorB;
                        color = evenRow ? color : ColorC;
                        _banks[bank][addr] = color;
                        count++;
                        x++;
                    }
                    count = 1;
                    while (x < 320 && count <= pattern.RepeatB)
                    {
                        int bank = startBankB + (x / 32);
                        int addr = ((x % 32) * 256) + y;
                        bool evenBank = x / 64 * 2 == x / 32;
                        byte color = evenBank ? colorA : colorB;
                        color = evenRow ? color : ColorC;
                        _banks[bank][addr] = color;
                        count++;
                        x++;
                    }
                }
            }
            return this;
        }

        public Banks FillVerticalPattern(int startBankA, int startBankB, byte colorA, byte colorB, byte colorC)
        {
            var patterns = Patterns.Vertical;
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
                    _banks[bankA][addr++] = color;
                }
                for (int i = 0; i < pattern.FirstB; i++)
                {
                    _banks[bankB][addr++] = color;
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
                        _banks[bankA][addr++] = color;
                    }
                    for (int i = 0; i < pattern.RepeatB; i++)
                    {
                        if (addr >= end)
                        {
                            done = true;
                            break;
                        }
                        _banks[bankB][addr++] = color;
                    }
                }
            }
            return this;
        }

        public Banks FillHorizontalPatternAlternating(int bank16kA, int bank16kB, string hPatternName)
        {
            _hPatternName = hPatternName;
            _sb.Clear();
            _sb.AppendLine("; hpattern.asm");
            _sb.AppendLine($"; Generated programmatically by {Assembly.GetExecutingAssembly().GetName().Name + ".exe"}");
            _sb.AppendLine();
            _sb.Append(PadCode("REPT "));
            _sb.AppendLine((SPEED14_REG18_COUNT / 2).ToString());
            _sb.Append(PadCode("    nextreg 18, "));
            _sb.AppendLine(bank16kA.ToString());
            _sb.Append(PadCode("    nextreg 18, "));
            _sb.AppendLine(bank16kB.ToString());
            _sb.AppendLine(PadCode("ENDR"));
            return this;
        }

        public Banks StartHorizontalPatternTest(string hPatternName)
        {
            _hPatternName = hPatternName;
            _reg18Count = 0;
            _sb.Clear();
            _sb.AppendLine("; hpattern.asm");
            _sb.AppendLine($"; Generated programmatically by {Assembly.GetExecutingAssembly().GetName().Name + ".exe"}");
            _sb.AppendLine();
            return this;
        }

        public Banks AddHorizontalPatternTest(int bank8k, int count)
        {
            if (count > 0)
            {
                int bank16k = bank8k / 2;
                _sb.AppendLine(AddComment(PadCode("REPT " + count.ToString()), $"AddHorizontalPatternTest({bank8k}, {count})"));
                _sb.Append(PadCode("    nextreg 18, "));
                _sb.AppendLine(bank16k.ToString());
                _sb.AppendLine(PadCode("ENDR"));
            }
            _reg18Count += count;
            return this;
        }

        public Banks AddHorizontalPatternAltTest(int bank8kA, int bank8kB, int count)
        {
            if (count > 0)
            {
                int bank16Ak = bank8kA / 2;
                int bank16kB = bank8kB / 2;
                int pairs = count / 2;
                bool odd = pairs * 2 < count;
                _sb.AppendLine(AddComment(PadCode("REPT " + pairs.ToString()), $"AddHorizontalPatternAltTest({bank8kA}, {bank8kB}, {count})"));
                _sb.AppendLine(PadCode("    nextreg 18, " + bank16Ak.ToString()));
                _sb.AppendLine(PadCode("    nextreg 18, " + bank16kB.ToString()));
                _reg18Count += pairs * 2;
                _sb.AppendLine(PadCode("ENDR"));
                if (odd)
                {
                    _sb.AppendLine(PadCode("nextreg 18, " + bank16Ak.ToString()));
                    _reg18Count++;
                }
            }
            return this;
        }

        public Banks EndHorizontalPatternTest(int bank8k)
        {
            int remaining = SPEED14_REG18_COUNT - _reg18Count;
            if (remaining > 0)
            {
                int bank16k = bank8k / 2;
                _sb.AppendLine(AddComment(PadCode("REPT " + remaining.ToString()), $"EndHorizontalPatternTest({bank8k})"));
                _sb.Append(PadCode("    nextreg 18, "));
                _sb.AppendLine(bank16k.ToString());
                _sb.AppendLine(PadCode("ENDR"));
                _reg18Count += bank16k;
            }
            return this;
        }

        public Banks SaveAsPng24_320x256(int startBank, string fileName)
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
                            // Get the bank and byte corresponding to the pixel.
                            int bank = startBank + (x / 32);
                            int byteIndex = ((x % 32) * 256) + y;
                            byte pixel = _banks[bank][byteIndex];
                            if (!pal.ContainsKey(pixel))
                            {
                                // Inside these two loops, we can set each pixel in turn.
                                // Assume pixel in bank is %RRRGGGBB if we don't have an explicit palette.
                                // In .NET, these colours are ARGB888.
                                pal.Add(pixel, Colour.FromRgb332(pixel));
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
            return this;
        }

        public Banks SaveAs8KbBanks(string fileName)
        {
            foreach (int bankNo in _banks.Keys)
            {
                string fn = fileName.Replace("{bankNo}", bankNo.ToString());
                File.WriteAllBytes(fn, _banks[bankNo]);
            }
            return this;
        }

        public Banks SaveAsHorizonalPattern(string directory)
        {
            string fn = Path.Combine(directory, _hPatternName);
            File.WriteAllText(fn, _sb.ToString());
            _sb.Clear();
            return this;
        }

        private string PadCode(string text)
        {
            return new string(' ', CODE_INDENT) + (text ?? "");
        }

        private string AddComment(string text, string comment)
        {
            if (text == null)
                text = string.Empty;
            if (string.IsNullOrWhiteSpace(comment))
                return text;
            text = text.PadRight(COMMENT_INDENT);
            text += text.Length == COMMENT_INDENT ? "; " : " ; ";
            text += (comment ?? "");
            return text;
        }

    }
}
