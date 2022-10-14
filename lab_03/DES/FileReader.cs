#nullable disable

using System;
using System.Collections;
using System.IO;

namespace DES
{
    class FileReader
    {
        private static FileStream _fs;
        public static int sBlockSize = 8;
        public FileReader(string filename)
        {
            if (File.Exists(filename))
            {
                _fs = new FileStream(filename, FileMode.Open);
            }
        }

        public int GetBlock(int num_sBlock, out BitArray sBlock_bit)
        {
            byte[] sBlock = new byte[sBlockSize];
            int res, offset = num_sBlock * sBlockSize;

            _fs.Seek(offset, SeekOrigin.Begin);
            res = _fs.Read(sBlock, 0, sBlockSize);

            sBlock_bit = new BitArray(sBlock);

            return res;
        }

        public void Close()
        {
            if (_fs != null)
                _fs.Close();
        }

        public static void GetFromFile(string filename, out int[] data)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string text = reader.ReadToEnd();
                    string[] str_num = text.Split(' ');
                    data = new int[str_num.Length];

                    for (int i = 0; i < str_num.Length; i++)
                    {
                        data[i] = int.Parse(str_num[i]);
                    }
                }
            }
            catch (Exception exc)
            {
                data = null;

                Console.WriteLine(exc.Message);
            }
        }

        public static int[][][] GetSBlocksFromFile(string filename)
        {
            int num_lines = 4;
            int num_columns = 16;
            int[][][] data = new int[sBlockSize][][];

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string text = reader.ReadToEnd();
                    string[] str_sBlocks = text.Split('/');

                    for (int i = 0; i < sBlockSize; i++)
                    {
                        int[][] sBlock = new int[sBlockSize][];
                        string[] str_sBlocks_lines = str_sBlocks[i].Trim().Split('\n');

                        for (int j = 0; j < num_lines; j++)
                        {
                            string[] str_line = str_sBlocks_lines[j].Trim().Split(' ');
                            int[] line = new int[num_columns];

                            for (int k = 0; k < num_columns; k++)
                                line[k] = Convert.ToInt32(str_line[k]);
                            
                            sBlock[j] = line;
                        }

                        data[i] = sBlock;
                    }
                }
            }
            catch (Exception exc)
            {
                // data = null;

                Console.WriteLine(exc.Message);

                return null;
            }

            return data;
        }
    }
}