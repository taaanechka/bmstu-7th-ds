#nullable disable

using System;
using System.IO;
using System.Collections;

namespace DES
{
    class FileWriter
    {
        private static FileStream _fs;

        public FileWriter(string filename)
        {
            _fs = new FileStream(filename, FileMode.Create);
        }

        public void SaveInFile(BitArray sBlock)
        {
            byte[] temp = new byte[sBlock.Length / 8];
            sBlock.CopyTo(temp, 0);

            for (int i = 0; i < temp.Length; i++)
                _fs.WriteByte(temp[i]);
        }

        public void SaveSizeInFile(int size)
        {
            byte add = (byte)size;

            _fs.WriteByte(add);
        }

        public void CutFile(int num)
        {
            _fs.SetLength(_fs.Length - num);
        }

        public void Close()
        {
            if (_fs != null)
                _fs.Close();
        }

        public void ShowArray(BitArray arr)
        {
            for (int i = 0; i < arr.Count; i++)
                Console.WriteLine($"{i}, {arr[i]}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public void ShowArray(int[] arr)
        {
            Console.WriteLine(String.Join(" ", arr));
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}