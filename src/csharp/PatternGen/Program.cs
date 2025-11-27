using PatternGen.Classes;
using System.CodeDom;
using System.Drawing;
using System.IO;

namespace PatternGen
{
    internal class Program
    {
        const string IMAGE_PATH = @"..\..\..\..\..\images\resized\";
        const string DATA_PATH = @"..\..\..\..\..\data\";
        const string CODE_PATH = @"..\..\..\..\..\src\asm\";
       
        static void Main()
        {
            //StripesWithRandomBW();
            //StripesWithRandomColour();
            HorizontalDiagonalPattern();
            //HorizontalTestPattern();

        }

        static void StripesWithRandomBW()
        {
            new Banks()
                .Create(18, 20)
                .FillRandom(Patterns.RandomBW)
                .FillVerticalPattern(18, 28, Colour.Red, Colour.Magenta, Colour.Black)
                .SaveAsPng24_320x256(18, Path.Combine(IMAGE_PATH, "StripesWithRandomBW-A.png"))
                .SaveAsPng24_320x256(28, Path.Combine(IMAGE_PATH, "StripesWithRandomBW-B.png"))
                .SaveAs8KbBanks(Path.Combine(DATA_PATH, "pattern_{bankNo}.bin"))
                .FillHorizontalPatternAlternating(9, 14, "hpattern.asm")
                .SaveAsHorizonalPattern(Path.Combine(CODE_PATH));
        }

        static void StripesWithRandomColour()
        {
            new Banks()
                .Create(18, 20)
                .FillRandom(Patterns.RandomColour)
                .FillVerticalPattern(18, 28, Colour.Red, Colour.Magenta, Colour.Black)
                .SaveAsPng24_320x256(18, Path.Combine(IMAGE_PATH, "StripesWithRandomColour-A.png"))
                .SaveAsPng24_320x256(28, Path.Combine(IMAGE_PATH, "StripesWithRandomColour-B.png"))
                .SaveAs8KbBanks(Path.Combine(DATA_PATH, "pattern_{bankNo}.bin"))
                .FillHorizontalPatternAlternating(9, 14, "hpattern.asm")
                .SaveAsHorizonalPattern(Path.Combine(CODE_PATH));
        }

        static void HorizontalDiagonalPattern()
        {
            int blueBank = 18;
            int yellowBank = 28;
            new Banks()
                .Create(blueBank, 20)
                .FillSolid(blueBank, 10, Colour.Blue)
                .FillSolid(yellowBank, 10, Colour.Yellow)
              //.FillRow(blueBank, yellowBank, 0, Colour.Red, Colour.Red, 0, 8, 10, 10)
              //.FillRow(blueBank, yellowBank, 1, Colour.Cyan, Colour.Cyan, 2, 10, 10, 10)
              //.FillRow(blueBank, yellowBank, 2, Colour.Red, Colour.Red, 6, 10, 10, 10)
              //.FillRow(blueBank, yellowBank, 3, Colour.Cyan, Colour.Cyan, 0, 0, 10, 10)
              //.FillRow(blueBank, yellowBank, 4, Colour.Red, Colour.Red, 0, 4, 10, 10)
                .FillHorizontal(blueBank, yellowBank, Colour.Magenta, Colour.Cyan, Colour.Black)
                .SaveAsPng24_320x256(18, Path.Combine(IMAGE_PATH, "HorizontalDiagonalPattern-A.png"))
                .SaveAsPng24_320x256(28, Path.Combine(IMAGE_PATH, "HorizontalDiagonalPattern-B.png"))
                .SaveAs8KbBanks(Path.Combine(DATA_PATH, "pattern_{bankNo}.bin"))
                .StartHorizontalPatternTest("hpattern.asm")
                .AddHorizontalPatternAltTest(yellowBank, blueBank, 11676)
                .EndHorizontalPatternTest(blueBank)
                .SaveAsHorizonalPattern(Path.Combine(CODE_PATH));
        }

        static void HorizontalTestPattern()
        {
            int blueBank = 18;
            int yellowBank = 28;
            new Banks()
                .Create(blueBank, 20)
                .FillSolid(blueBank, 10, Colour.Blue)
                .FillSolid(yellowBank, 10, Colour.Yellow)
                .SaveAsPng24_320x256(18, Path.Combine(IMAGE_PATH, "HorizontalTestPattern-A.png"))
                .SaveAsPng24_320x256(28, Path.Combine(IMAGE_PATH, "HorizontalTestPattern-B.png"))
                .SaveAs8KbBanks(Path.Combine(DATA_PATH, "pattern_{bankNo}.bin"))
                .StartHorizontalPatternTest("hpattern.asm")
                .AddHorizontalPatternTest(blueBank, 10) // 10 puts us exactly at 0,0
              //.AddHorizontalPatternTest(yellow, 32)
                .AddHorizontalPatternAltTest(yellowBank, blueBank, 33)
                .EndHorizontalPatternTest(blueBank)
                .SaveAsHorizonalPattern(Path.Combine(CODE_PATH));
        }
    }
}
