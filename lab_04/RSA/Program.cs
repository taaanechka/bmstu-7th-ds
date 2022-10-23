using System;

namespace RSA
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
                
                string srcFilename = args[0];   // @"data/in/img.jpg"; @"data/in/text.txt"; @"data/in/video.mp4";
                string[] subs = srcFilename.Split('.');
                string strEnd = ""; 

                if (srcFilename[0] == '.' || subs.Length > 1)
                {
                    strEnd = String.Concat(".", subs[subs.Length - 1]);
                }
                
                string decipheredFilename = @$"data/out/deciphered{strEnd}"; //jpg"; //txt"; //mp4";


                Console.WriteLine("Wait, please.");

                Cipher cipher = new Cipher();
                
                if (cipher.Encrypt(srcFilename, cipheredFilename) == (int)Consts.Errors.ExistsErr)
                {
                    Console.WriteLine(@$"ERR: file '{srcFilename}' doesn't exist.");
                }
                else if (cipher.Decrypt(cipheredFilename, decipheredFilename) == (int)Consts.Errors.ExistsErr)
                {
                    Console.WriteLine(@$"ERR: file '{cipheredFilename}' doesn't exist.");
                }
                else
                {
                    Console.WriteLine("Done!");
                }
            }
            
            // string fileNameScr = @"data/in/img.jpg";                //@"test_video.gif"; @"img.jpg"; @"text.txt";
            // string fileNameCipher = @"data/out/ciphered.jpg";
            // string fileNameResult = @"data/out/deciphered.jpg";

            // Cipher cipher = new Cipher();

            // cipher.Encrypt(fileNameScr, fileNameCipher);
            // cipher.Decrypt(fileNameCipher, fileNameResult);

            // Console.WriteLine("Done!!!");
            // // Console.ReadKey();
        }
    }
}