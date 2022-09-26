using System;
using System.IO;

namespace EnigmaMachine
{
    public class Device
    {
        public static int bytesNum = 256;
        public int rotNum = 0;

        public int[] connArr = new int[bytesNum];
        
        public Device()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < bytesNum; i++)
            {
                connArr[i] = i;
            }

            connArr = connArr.OrderBy(el => rnd.Next()).ToArray();
        }

        public int getValue(int index)
        {
            return connArr[index];
        }

        public int getIndex(int value)
        {
            return Array.IndexOf(connArr, value);
        }

        public void saveInFile(FileStream f)
        {
            for (int i = 0; i < bytesNum; i++)
            {
                f.WriteByte((byte)connArr[i]);
            }
        }

        public void saveFromFile(FileStream f)
        {
            rotNum = 0;

            for (int i = 0; i < bytesNum; i++)
            {
                connArr[i] = f.ReadByte();
            }
        }

        public void show()
        {
            Console.WriteLine(String.Join(" ", connArr));
            Console.WriteLine();
        }
    }
}