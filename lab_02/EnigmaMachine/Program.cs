using System;

namespace EnigmaMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || String.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine($"ERR: {args.Length} - Invalid number of parameters");
            }
            else
            {
                string cipheredFilename = @"data/out/ciphered.txt";
                string enigmaState = @"data/in/enigmaState.txt";
                
                string srcFilename = args[0];   // @"data/in/img.jpg"; @"data/in/text.txt"; @"data/in/video.mp4";
                string[] subs = srcFilename.Split('.');
                string strEnd = ""; 

                if (srcFilename[0] == '.' || subs.Length > 1)
                {
                    strEnd = String.Concat(".", subs[subs.Length - 1]);
                }
                
                string decipheredFilename = @$"data/out/deciphered{strEnd}"; //jpg"; //txt"; //mp4";

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
}
