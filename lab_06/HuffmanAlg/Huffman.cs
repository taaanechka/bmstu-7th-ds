using System.Collections;

namespace HuffmanAlg
{
    public class Huffman
    {
        private static void _prepareText(string filename, out BitArray bitsCut)
        {
            byte[] text = Reader.FileRead(filename);
            byte sizeUseless = text[text.Length - 1];

            Array.Resize(ref text, text.Length - 1);

            BitArray bitsFull = new BitArray(text);
            bitsCut = new BitArray(bitsFull.Length - sizeUseless);


            for (int i = 0; i < bitsCut.Length; i++)
                bitsCut[i] = bitsFull[i];
        }

        private static void _createFrequencyTable(byte[] text, out Dictionary<byte, int> dict)
        {
            dict = new Dictionary<byte, int>();
            int n = text.Length;
            byte sign;

            for (int i = 0; i < n; i++)
            {
                sign = text[i];

                if (dict.ContainsKey(sign))
                    dict[sign] += 1;
                else
                    dict.Add(sign, 1);
            }
        }

        private static void _getNodesList(Dictionary<byte, int> dict, out List<TreeNode<int>> nodesList)
        {
            nodesList = new List<TreeNode<int>>();

            foreach (byte key in dict.Keys)
            {
                TreeNode<int> node = new TreeNode<int>(key, dict[key]);
                nodesList.Add(node);
            }
        }

        private static void _createTree(Dictionary<byte, int> dict, out BinaryTree<int> tree)
        {
            List<TreeNode<int>> stat;

            _getNodesList(dict, out stat);
            BinaryTree<int>.Create(stat, out tree);

            //tree.ConsolePrintTree();
        }

        private static void _createCodesArr(BinaryTree<int> tree, ref Dictionary<byte, string> codeArr)
        {
            string temp = "";

            _createCodes(tree.root, ref codeArr, ref temp);
        }

        private static void _createCodes(TreeNode<int> node, ref Dictionary<byte, string> codeArr, ref string code)
        {
            if (node.left == null && node.right == null)
            {
                string result = code;

                codeArr.Add(node.sign, result);
                if (code.Length != 0)
                    code = code.Remove(code.Length - 1);
            }
            else
            {
                if (node.left != null)
                {
                    code += 0;
                    _createCodes(node.left, ref codeArr, ref code);
                }

                if (node.right != null)
                {
                    code += 1;
                    _createCodes(node.right, ref codeArr, ref code);
                }

                if (code.Length != 0)
                    code = code.Remove(code.Length - 1);
            }
        }

        public static BitArray Encode(byte[] text, Dictionary<byte, string> codeArr)
        {
            BitArray bits;
            string entext = "";

            for (int i = 0; i < text.Length; i++)
                entext += codeArr[text[i]];

            bits = new BitArray(entext.Select(c => c == '1').ToArray());

            return bits;
        }

        public static BinaryTree<int> Compress(string fileSrc, string fileDest)
        {
            Dictionary<byte, int> dict;
            Dictionary<byte, string> codeArr = new Dictionary<byte, string>();
            BinaryTree<int> tree;
            BitArray bits;

            byte[] text = Reader.FileRead(fileSrc);
            
            _createFrequencyTable(text, out dict);

            _createTree(dict, out tree);
            _createCodesArr(tree, ref codeArr);

            bits = Encode(text, codeArr);

            Writer.FileWrite(bits, fileDest);

            return tree;
        }

        public static void Decompress(string fileSrc, string fileDest, BinaryTree<int> tree)
        {
            List<byte> res = new List<byte>();
            BitArray bits;
            TreeNode<int> node = tree.root;

            _prepareText(fileSrc, out bits);

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (node.right != null)
                        node = node.right;
                }
                else if (!bit)
                {
                    if (node.left != null)
                        node = node.left;
                }

                if (node.left == null && node.right == null)
                {
                    res.Add(node.sign);
                    node = tree.root;
                }
            }

            Writer.FileWrite(res.ToArray(), fileDest);
        }
    }
}