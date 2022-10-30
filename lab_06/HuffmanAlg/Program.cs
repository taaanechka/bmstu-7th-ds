namespace HuffmanAlg
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
                string srcFilename = args[0];   // @"data/in/img.jpg"; @"data/in/text.txt"; @"data/in/video.mp4";
                string[] subs = srcFilename.Split('.');
                string strEnd = ""; 

                if (srcFilename[0] == '.' || subs.Length > 1)
                {
                    strEnd = String.Concat(".", subs[subs.Length - 1]);
                }

                string compressedFilename = @$"data/out/compressed{strEnd}";
                string decompressedFilename = @$"data/out/decompressed{strEnd}"; //jpg"; //txt"; //mp4"; //gif;

                string fileTree = @"data/out/tree.json";

                Console.WriteLine("Wait, please.");

                BinaryTree<int> tree = Huffman.Compress(srcFilename, compressedFilename);
                
                tree.ConsolePrintTree();
                tree.ConvertToJSON(fileTree);

                Huffman.Decompress(compressedFilename, decompressedFilename, BinaryTree<int>.ConvertFromJSON(fileTree));
            }
        }
    }
}