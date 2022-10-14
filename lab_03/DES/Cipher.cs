#nullable disable

using System.Collections;

namespace DES
{
    class Cipher
    {
        private static string _root = @"tables\";
        
        private string _filenameB = _root + @"round_keys\B.txt";
        private string _filenameSi = _root + @"round_keys\Si.txt";
        private string _filenameCP = _root + @"round_keys\CP.txt";
        //
        private string _filenameIP = _root + @"cipher\IP.txt";
        private string _filenameE = _root + @"cipher\E.txt";
        private string _filenameSBlocks = _root + @"cipher\SBlocks.txt";
        private string _filenameP = _root + @"cipher\P.txt";
        private string _filenameIPInverse = _root + @"cipher\IPInverse.txt";
        

        private static BitArray _key;
        private static BitArray[] _keys_arr;

        static int[] prmIP;
        static int[] prmIPInverse;
        public static int[] moveSi;
        public static int[] prmCP;
        public static int[] prmB;
        public static int[] prmE;
        public static int[] prmP;
        public static int[][][] sBlocks;

        public Cipher()
        {
            FileReader.GetFromFile(_filenameIP, out prmIP);
            FileReader.GetFromFile(_filenameIPInverse, out prmIPInverse);
            FileReader.GetFromFile(_filenameSi, out moveSi);
            FileReader.GetFromFile(_filenameB, out prmB);
            FileReader.GetFromFile(_filenameCP, out prmCP);
            FileReader.GetFromFile(_filenameE, out prmE);
            FileReader.GetFromFile(_filenameP, out prmP);
            sBlocks = FileReader.GetSBlocksFromFile(_filenameSBlocks);

            RoundKeysProcessing.GetKey(out _key);
            RoundKeysProcessing.GetKeys(_key, out _keys_arr);
        }

        public int Encrypt(string inFile, string outFile)
        {
            if (!File.Exists(inFile))
            {
                return (int)Consts.Errors.ExistsErr;
            }

            FileReader reader = new FileReader(inFile);
            FileWriter writer = new FileWriter(outFile);

            BitArray s_block;
            int num = 0, size, prevsize = 0;

            while ((size = reader.GetBlock(num, out s_block)) > 0)
            {
                var es_block = DoEcryption(s_block);
                writer.SaveInFile(es_block);
                prevsize = size;

                num++;
            }

            writer.SaveSizeInFile(prevsize);

            reader.Close();
            writer.Close();

            return Consts.OK;
        }

        private static BitArray DoEcryption(BitArray block)
        {
            BitArray left, right;
                    
            BitArray prm_block = Encryption.Permutate(block, prmIP);
            Encryption.GetLeftPart(prm_block, out left);
            Encryption.GetRightPart(prm_block, out right);

            for (int i = 0; i < _keys_arr.Length; i++)
            {
                var temp_left = right;
                right = left.Xor(Encryption.FeistelFunc(right, _keys_arr[i]));
                left = temp_left;
            }

            prm_block = RoundKeysProcessing.JoinParts(left, right);
            prm_block = Encryption.Permutate(prm_block, prmIPInverse);

            return prm_block;
        }

        public int Decrypt(string inFile, string outFile)
        {
            if (!File.Exists(inFile))
            {
                return (int)Consts.Errors.ExistsErr;
            }

            FileReader reader = new FileReader(inFile);
            FileWriter writer = new FileWriter(outFile);

            BitArray s_block;
            int num = 0;

            while (reader.GetBlock(num, out s_block) == 8)
            {
                var encrypted_s_block = DoDecryption(s_block);
                writer.SaveInFile(encrypted_s_block);
                num++;
            }

            int[] temp = new int[2];
            s_block.CopyTo(temp, 0);

            if (temp[0] != 0)
                writer.CutFile(8 - temp[0]);

            reader.Close();
            writer.Close();

            return Consts.OK;
        }

        public static BitArray DoDecryption(BitArray block)
        {
            BitArray left, right;

            BitArray prm_block = Encryption.Permutate(block, prmIP);
            Encryption.GetLeftPart(prm_block, out left);
            Encryption.GetRightPart(prm_block, out right);

            for (int i = _keys_arr.Length - 1; i >= 0; i--)
            {
                var temp_right = left;
                left = right.Xor(Encryption.FeistelFunc(left, _keys_arr[i]));
                right = temp_right;
            }

            prm_block = RoundKeysProcessing.JoinParts(left, right);
            prm_block = Encryption.Permutate(prm_block, prmIPInverse);

            return prm_block;
        }
    }
}