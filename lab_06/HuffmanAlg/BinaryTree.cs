#nullable disable

using System.Text.Json;

namespace HuffmanAlg
{
    public class TreeNode<T> where T : IComparable<T>
    {
        public T value { get; set; }
        public byte sign { get; set; }
        public TreeNode<T> left { get; set; }
        public TreeNode<T> right { get; set; }

        public TreeNode(byte s, T val)
        {
            sign = s;
            value = val;
        }

        public TreeNode(T val)
        {
            value = val;
        }

        public TreeNode() { }
    }

    public class BinaryTree<T> where T : IComparable<T>
    {
        public TreeNode<T> root { get; set; }

        public static void Create(List<TreeNode<int>> stat, out BinaryTree<int> tree)
        {
            tree = new BinaryTree<int>();

            while (stat.Count > 1)
            {
                MathFuncs.InsertionSort(ref stat);

                TreeNode<int> parent = new TreeNode<int>(stat[0].value + stat[1].value);
                parent.left = stat[0];
                parent.right = stat[1];

                stat.RemoveAt(0);
                stat.RemoveAt(0);

                stat.Add(parent);
            }

            tree.root = stat.First();
        }

        public void ConvertToJSON(string filename)
        {
            string jsonString = JsonSerializer.Serialize(this);

            File.WriteAllText(filename, jsonString);
        }

        public static BinaryTree<T> ConvertFromJSON(string filename)
        {
            string jsonString = File.ReadAllText(filename);

            return JsonSerializer.Deserialize<BinaryTree<T>>(jsonString);
        }

        public void ConsolePrintTree()
        {
            _ConsolePrintNode(root);
        }

        private void _ConsolePrintNode(TreeNode<T> startNode, string indent = "", string side = null)
        {
            if (startNode != null)
            {
                var nodeSide = side == null ? "+" : side;

                if (startNode.left != null && startNode.right != null)
                    Console.WriteLine($"{indent} [{nodeSide}]- {startNode.value}");
                else if (startNode.left == null && startNode.right == null)
                    Console.WriteLine($"{indent} [{nodeSide}]- {startNode.value}, {Convert.ToChar(startNode.sign)}");

                indent += new string(' ', 3);

                _ConsolePrintNode(startNode.left, indent, "L");
                _ConsolePrintNode(startNode.right, indent, "R");
            }
        }

        private static void _ConsolePrint(TreeNode<T> node)
        {
            if (node == null) 
                return;

            _ConsolePrint(node.left);

            Console.Write(node.sign + " : " + node.value + " ");

            if (node.right != null)
                _ConsolePrint(node.right);
        }

        public static void ConsolePrint(BinaryTree<T> tree)
        {
            _ConsolePrint(tree.root);
            
            Console.WriteLine();
        }
    }
}