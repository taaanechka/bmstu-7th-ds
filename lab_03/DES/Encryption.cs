using System.Collections;

namespace DES
{
    class Encryption
    {
        public static BitArray Permutate(BitArray sBlock, int[] ind)
        {
            BitArray p_arr = new BitArray(ind.Length);

            for (int i = 0; i < ind.Length; i++)
                p_arr[i] = sBlock[ind[i] - 1];

            return p_arr;
        }

        public static void GetLeftPart(BitArray sBlock, out BitArray left)
        {
            int part_size = sBlock.Count / 2;

            left = new BitArray(part_size);

            for (int i = 0; i < part_size; i++)
                left[i] = sBlock[i];
        }

        public static void GetRightPart(BitArray sBlock, out BitArray right)
        {
            int part_size = sBlock.Count / 2;

            right = new BitArray(part_size);

            for (int i = part_size, i_pos = 0; i < sBlock.Count; i++, i_pos++)
                right[i_pos] = sBlock[i];
        }

        public static BitArray FeistelFunc(BitArray data, BitArray k_i)
        {
            // Расширяющая перестановка
            BitArray sBlock = Permutate(data, Cipher.prmE);

            // XOR(MR, K_i)
            sBlock.Xor(k_i);

            // SBlocks
            int[] bitRes = new int[8];

            for (int i = 0; i < 8; i++)
            {
                int cur_SBlock = i * 6;
                int x = 0, y = 0;

                if (sBlock[cur_SBlock])        x += 10;
                if (sBlock[cur_SBlock + 5])    x += 1;

                if (sBlock[cur_SBlock + 1])    y += 1000;
                if (sBlock[cur_SBlock + 2])    y += 100;
                if (sBlock[cur_SBlock + 3])    y += 10;
                if (sBlock[cur_SBlock + 4])    y += 1;

                x = Convert.ToInt32(x.ToString(), 2);
                y = Convert.ToInt32(y.ToString(), 2);

                bitRes[i] = Cipher.sBlocks[i][x][y];
            }

            sBlock = _GetBitArray(bitRes);

            // Перестановка
            return Permutate(sBlock, Cipher.prmP);
        }

        private static BitArray _GetBitArray(int[] arr)
        {
            int arrLen = arr.Length;
            BitArray bitRes = new BitArray(arrLen * 4);

            for (int i_sBlock = 0, i = 0; i_sBlock < arrLen; i_sBlock++, i += 4)
            {
                if (arr[i_sBlock] >= 8)
                {
                    bitRes[i + 3] = true;
                    arr[i_sBlock] -= 8;
                }

                if (arr[i_sBlock] >= 4)
                {
                    bitRes[i + 2] = true;
                    arr[i_sBlock] -= 4;
                }

                if (arr[i_sBlock] >= 2)
                {
                    bitRes[i + 1] = true;
                    arr[i_sBlock] -= 2;
                }

                if (arr[i_sBlock] == 1)
                    bitRes[i] = true;
            }

            return bitRes;
        }
        
    }
}