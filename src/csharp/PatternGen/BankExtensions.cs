using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternGen
{
    public static class BankExtensions
    {
        const int BANK_SIZE = 8 * 1024;

        public static Dictionary<int, byte[]> Setup(this Dictionary<int, byte[]> dic, int bankNo, byte colour)
        {
            var bytes = new byte[BANK_SIZE];
            for (int i = 0; i < BANK_SIZE; i++)
                bytes[i] = colour;
            dic.Add(bankNo, bytes);
            return dic;
        }

        public static Dictionary<int, byte[]> Fill(this Dictionary<int, byte[]> dic, int bankA, int bankB,
            int x, int y, byte colorA, byte colorB, int firstA, int firstB, int repeatA, int repeatB)
        {
            int addr = (x * 256) + y;
            int end = addr + 256;
            for (int i = 0; i < firstA; i++)
            {
                dic[bankA][addr++] = colorA;
            }
            for (int i = 0; i < firstB; i++)
            {
                dic[bankB][addr++] = colorB;
            }
            bool done = false;
            while(!done)
            {
                for (int i = 0; i < repeatA; i++)
                {
                    if (addr >= end)
                    {
                        done = true;
                        break;
                    }
                    dic[bankA][addr++] = colorA;
                }
                for (int i = 0; i < repeatB; i++)
                {
                    if (addr >= end)
                    {
                        done = true;
                        break;
                    }
                    dic[bankB][addr++] = colorB;
                }
            }

            return dic;
        }
    }
}
