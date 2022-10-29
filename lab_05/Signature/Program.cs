using System;

namespace Signature
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
                var signer = new Signer(args[0]);

                signer.GenerateKeys();

                byte[] signature;

                if (signer.CreateSignature(out signature) == (int)Consts.Errors.ExistsErr)
                {
                    Console.WriteLine(@$"CreateSignature ERR: file '{args[0]}' doesn't exist.");
                }
                else
                {
                    File.WriteAllBytes(signer.SignatureFilename, signature);

                    int err;
                    string IsSignedMsg = signer.IsSigned(out err) 
                                        ? "The signature verification is successful." 
                                        : "The signature verification failed.";
                    string res = (err == Consts.OK) 
                                    ? IsSignedMsg 
                                    : @$"IsSigned ERR: file '{args[0]}' doesn't exist.";

                    Console.WriteLine(res);
                }
            }
        }
    }
}