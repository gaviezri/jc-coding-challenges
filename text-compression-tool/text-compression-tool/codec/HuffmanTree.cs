using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_compression_tool.codec
{
    internal class HuffmanTree
    {
        private class TreeNode
        {
            private static LinkedList<Boolean> traversalPathForCode;

            public TreeNode? Left { get; set; }
            public TreeNode? Right { get; set; }
            public char? Token { get; set; }
            public int Frequency { get; set; }


            public TreeNode(char? token = null, int frequency = 0, TreeNode? left = null, TreeNode? right = null)
            {
                Left = left;
                Right = right;
                Token = token;
                Frequency = frequency;
            }

            static TreeNode()
            {
                traversalPathForCode = new();
                //traversalPathForCode.AddFirst(true);
            }

            public void TraverseAndCreatePrefixCode(Dictionary<char, BitArray> codeTable)
            {

                // false == going left
                if (Left is not null)
                {
                    traversalPathForCode.AddLast(false);
                    Left.TraverseAndCreatePrefixCode(codeTable);
                    traversalPathForCode.RemoveLast();
                }

                // true == going right
                if (Right is not null)
                {
                    traversalPathForCode.AddLast(true);
                    Right.TraverseAndCreatePrefixCode(codeTable);
                    traversalPathForCode.RemoveLast();
                }


                if (Token.HasValue)
                {
                    codeTable.Add(Token.Value, new BitArray(traversalPathForCode.ToArray()));
                }
            }
        }

        private TreeNode root;

        public HuffmanTree(Dictionary<char, int> i_freqTokens)
        {

            var forestOfLeaves = new PriorityQueue<TreeNode, int>();
            foreach (var token in i_freqTokens.Keys)
            {
                int priority = i_freqTokens[token];
                forestOfLeaves.Enqueue(new TreeNode(token, priority), priority);
            }

            for (int i = 1; i < i_freqTokens.Count; i++)
            {
                TreeNode node = new TreeNode();
                node.Left = forestOfLeaves.Dequeue();
                node.Right = forestOfLeaves.Dequeue();
                forestOfLeaves.Enqueue(node, node.Frequency = node.Right.Frequency + node.Left.Frequency);
            }

            root = forestOfLeaves.Dequeue();
        }


        public Dictionary<char, BitArray> GeneratePrefixCodeTable()
        {
            var codeTable = new Dictionary<char, BitArray>();
            root.TraverseAndCreatePrefixCode(codeTable);
            return codeTable;

        }
    }
}
