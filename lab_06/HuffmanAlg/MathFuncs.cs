namespace HuffmanAlg
{
    public class MathFuncs
    {
        public static void InsertionSort(ref List<TreeNode<int>> nodesList)
        {
            TreeNode<int> key;

            int val, j;

            for (var i = 1; i < nodesList.Count; i++)
            {
                key = nodesList[i];
                val = nodesList[i].value;

                j = i;
                
                while ((j > 0) && (nodesList[j - 1].value > val))
                {
                    (nodesList[j - 1], nodesList[j]) = (nodesList[j], nodesList[j - 1]);

                    j--;
                }

                nodesList[j] = key;
            }
        }
    }
}