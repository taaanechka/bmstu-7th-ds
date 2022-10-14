using System;
using System.Collections;

namespace DES
{
    class RoundKeysProcessing
    {
        private static int _num_keys = 16;

        public static void GetKey(out BitArray key)
        {
            key = _GenerateKey();
            key = Encryption.Permutate(key, Cipher.prmB);
        }

        public static void GetKeys(BitArray key, out BitArray[] keys_arr)
        {
            BitArray c0, d0;

            Encryption.GetLeftPart(key, out c0);
            Encryption.GetRightPart(key, out d0);

            keys_arr = _CreateKeys(c0, d0);
        }

        private static BitArray[] _CreateKeys(BitArray c0, BitArray d0)
        {
            BitArray[] keys_arr = new BitArray[_num_keys];

            for (int i = 0; i < _num_keys; i++)
            {
                c0 = MoveLeft(c0, Cipher.moveSi[i]);
                d0 = MoveLeft(d0, Cipher.moveSi[i]);

                BitArray key_joined = JoinParts(c0, d0);

                keys_arr[i] = Encryption.Permutate(key_joined, Cipher.prmCP);
            }

            return keys_arr;
        }

        private static BitArray _GenerateKey()
        {
            Random rnd = new Random();

            byte[] key = new byte[sizeof(Int64)];
            rnd.NextBytes(key);

            return new BitArray(key);
        }

        public static BitArray MoveLeft(BitArray key, int step)
        {
            BitArray key_new = new BitArray(key.Count);

            for (int i = 0; i < key.Count; i++)
                key_new[i] = key[(i + step) % key.Count];

            return key_new;
        }

        public static BitArray JoinParts(BitArray key1, BitArray key2)
        {
            bool[] key_bool = new bool[key1.Count + key2.Count];

            key1.CopyTo(key_bool, 0);
            key2.CopyTo(key_bool, key1.Count);

            return new BitArray(key_bool);
        }
    }
}