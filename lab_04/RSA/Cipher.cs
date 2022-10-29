using System.Numerics;
using System.IO;

namespace RSA
{
    public class Cipher
    {
        static int publicKey, privateKey, n;
        AlgRSA rsa;

        public Cipher()
        {
            rsa = new AlgRSA();
            publicKey = rsa.publicKey;
            privateKey = rsa.privateKey;
            n = rsa.n;
        }

        public int Encrypt(string inFile, string outFile)
        {
            if (!File.Exists(inFile))
            {
                return (int)Consts.Errors.ExistsErr;
            }

            FileStream fsIn = new FileStream(inFile, FileMode.Open);
            FileStream fsOut = new FileStream(outFile, FileMode.Create);
            BinaryWriter binWriter = new BinaryWriter(fsOut);

            int cur;
            while (fsIn.CanRead)
            {
                cur = fsIn.ReadByte();
                if (cur == -1)
                    break;
                int res = DoEcryption(cur);
                //Console.WriteLine($"cur: {cur} res:{res}");
                binWriter.Write(res);
            }

            binWriter.Write(-1);
            binWriter.Close();

            fsOut.Close();
            fsIn.Close();

            return Consts.OK;
        }

        private static int DoEcryption(int blk)
        {
            return (int)BigInteger.ModPow(blk, publicKey, n);
        }

        public int Decrypt(string inFile, string outFile)
        {
            if (!File.Exists(inFile))
            {
                return (int)Consts.Errors.ExistsErr;
            }

            FileStream fsIn = new FileStream(inFile, FileMode.Open);
            FileStream fsOut = new FileStream(outFile, FileMode.Create);
            BinaryReader binReader = new BinaryReader(fsIn);

            int cur;
            while (fsIn.CanRead)
            {
                cur = binReader.ReadInt32();
                if (cur == -1)
                    break;
                int res = DoDecryption(cur);
                //Console.WriteLine($"cur: {cur} res:{res}");
                fsOut.WriteByte((byte)res);
            }

            binReader.Close();

            fsOut.Close();
            fsIn.Close();

            return Consts.OK;
        }

        private static int DoDecryption(int blk)
        {
            return (int)BigInteger.ModPow(blk, privateKey, n);
        }
    }
}