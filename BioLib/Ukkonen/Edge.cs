using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib.Ukkonen
{
    public class Edge
    {
        SuffixTree _tree;

        public Edge(SuffixTree tree, Node head)
        {
            _tree = tree;
            Head = head;
            StartIndex = tree.CurrentSuffixEndIndex;
            EdgeNumber = _tree.NextEdgeNumber++;
        }

        public Node Head;
        public Node Tail;
        public int StartIndex;
        public int? EndIndex;
        public int EdgeNumber;
        public int Length { get { return (EndIndex ?? _tree.Word.Length - 1) - StartIndex + 1; } }

        public Edge SplitAtIndex(int index)
        {
            var newEdge = new Edge(_tree, Head);
            var newNode = new Node(_tree);
            newEdge.Tail = newNode;
            newEdge.StartIndex = StartIndex;
            newEdge.EndIndex = index - 1;
            Head = newNode;
            StartIndex = index;
            newNode.Edges.Add(_tree.Word[StartIndex], this);
            newEdge.Head.Edges[_tree.Word[newEdge.StartIndex]] = newEdge;
            return newEdge;
        }

        public override string ToString()
        {
            return string.Concat(_tree.Word.Substring(StartIndex, (EndIndex ?? _tree.CurrentSuffixEndIndex) - StartIndex + 1), "(",
                StartIndex, ",", EndIndex.HasValue ? EndIndex.ToString() : "#", ")");
        }

        public string StringWithoutCanonizationChar()
        {
            return _tree.Word.Substring(StartIndex, (EndIndex ?? _tree.CurrentSuffixEndIndex - 1) - StartIndex + 1);
        }

        public string RenderTree(string prefix, int maxEdgeLength)
        {
            var sb = new StringBuilder();
            var strEdge = _tree.Word.Substring(StartIndex, Math.Min(_tree.Word.Length - 1, (EndIndex ?? _tree.CurrentSuffixEndIndex)) - StartIndex + 1);
            sb.Append(strEdge);
            if (Tail == null)
                sb.AppendLine();
            else
            {
                var line = new string('─', maxEdgeLength - strEdge.Length + 1);
                sb.Append(line);
                sb.Append(Tail.RenderTree(string.Concat(prefix, new string(' ', strEdge.Length + line.Length))));
            }

            return sb.ToString();
        }
    }
}
