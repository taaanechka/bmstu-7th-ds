using System;

namespace EnigmaMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            string cipheredFilename = @"data/out/ciphered.txt";
            string decipheredFilename = @"data/out/deciphered.mp4"; //jpg"; //txt"; //mp4";
            string srcFilename = @"data/in/video.mp4"; // @"data/in/img.jpg"; @"data/in/text.txt"; @"data/in/video.mp4";
            string enigmaState = @"data/in/enigmaState.txt";

            Enigma machine1 = new Enigma();
            machine1.saveInFile(enigmaState);
            if (machine1.cipherFile(srcFilename, cipheredFilename) == (int)Consts.Errors.ExistsErr)
            {
                Console.WriteLine(@$"ERR: file '{srcFilename}' doesn't exist.");
            }
            else
            {
                Enigma machine2 = new Enigma();
                if (machine2.saveFromFile(enigmaState) == (int)Consts.Errors.ExistsErr)
                {
                    Console.WriteLine(@$"ERR: file '{enigmaState}' doesn't exist.");
                }
                else
                {
                    if (machine2.cipherFile(cipheredFilename, decipheredFilename) == (int)Consts.Errors.ExistsErr)
                    {
                        Console.WriteLine(@$"ERR: file '{cipheredFilename}' doesn't exist.");
                    }
                    else
                    {
                        Console.WriteLine("Done!");
                    }
                }
            }
        }
    }
}
