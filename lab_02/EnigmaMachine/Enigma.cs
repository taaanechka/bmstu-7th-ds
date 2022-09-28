using System;
using System.IO;

namespace EnigmaMachine
{
    public class Enigma
    {
        static int rotorsNum = 3;

        public Rotor[] rotorsArr = new Rotor[rotorsNum];
        Reflector reflector;

        public Enigma()
        {
            for (int i = 0; i < rotorsNum; i++)
            {
                rotorsArr[i] = new Rotor();
            }

            reflector = new Reflector();
        }

        public int cipherFile(string srcFilename, string dstFilename)
        {
            int tmp;

            if (!File.Exists(srcFilename))
            {
                return (int)Consts.Errors.ExistsErr;
            }
            
            using (FileStream fRead = new FileStream(srcFilename, FileMode.Open))
            using (FileStream fWrite = new FileStream(dstFilename, FileMode.Create))
            {
                while (fRead.CanRead && (tmp = fRead.ReadByte()) != -1)
                {                    
                    fWrite.WriteByte((byte)cipherSign(tmp));
                }
            }

            return Consts.OK;
        }

        int cipherSign(int sign)
        {
            int resSign = sign;

            foreach (Rotor rotor in rotorsArr)
            {
                resSign = rotor.getValue(resSign);
            }

            resSign = reflector.getValue(resSign);

            for (int i = rotorsArr.Length - 1; i >= 0; i--)
            {
                resSign = rotorsArr[i].getIndex(resSign);
            }

            for (int i = 0; rotorsArr[i].rotate() == 0 && i < rotorsNum - 1; i++)
                ;

            return resSign;
        }

        public void saveInFile(string filename)
        {
            using (FileStream f = new FileStream(filename, FileMode.OpenOrCreate))
            {
                reflector.saveInFile(f);

                for (int i = 0; i < rotorsNum; i++)
                {
                    rotorsArr[i].saveInFile(f);
                }
            }
        }

        public int saveFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return (int)Consts.Errors.ExistsErr;
            }

            using (FileStream f = new FileStream(filename, FileMode.Open))
            {
                reflector.saveFromFile(f);

                for (int i = 0; i < rotorsNum; i++)
                {
                    rotorsArr[i].saveFromFile(f);
                }
            }

            return Consts.OK;
        }

        public void show()
        {
            Console.WriteLine("-----REFLECTOR-----");
            reflector.show();

            for (int i = 0; i < rotorsNum; i++)
            {
                Console.WriteLine($"------ROTOR {i+1}------");
                rotorsArr[i].show();
            }
        }
    }
}