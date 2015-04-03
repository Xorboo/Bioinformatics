using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib.Ukkonen
{
    public class Node
    {
        SuffixTree _tree;

        public Node(SuffixTree tree)
        {
            _tree = tree;
            Edges = new Dictionary<char, Edge>();
            NodeNumber = _tree.NextNodeNumber++;
        }

        public Dictionary<char, Edge> Edges;
        public Node LinkedNode;
        public int NodeNumber;

        public void AddNewEdge()
        {
            var edge = new Edge(_tree, this);
            Edges.Add(_tree.Word[_tree.CurrentSuffixEndIndex], edge);
        }

        public string RenderTree(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            if (LinkedNode != null)
            {
                sb.Append(NodeNumber).Append("→").Append(LinkedNode.NodeNumber);
            }
            else
            {
                sb.Append(' ').Append(NodeNumber).Append(' ');
            }
            sb.Append(")");
            var indexLength = sb.Length;

            var edges = Edges.Select(kvp => kvp.Value).OrderBy(e => _tree.Word[e.StartIndex]).ToArray();
            if (edges.Any())
            {
                var prefixWithNodePadding = prefix + new string(' ', indexLength);
                var maxEdgeLength = edges.Max(e => (e.EndIndex ?? _tree.CurrentSuffixEndIndex) - e.StartIndex + 1);
                for (var i = 0; i < edges.Length; i++)
                {
                    char connector, extender = ' ';
                    if (i == 0)
                    {
                        if (edges.Length > 1)
                        {
                            connector = '┬';
                            extender = '│';
                        }
                        else
                            connector = '─';
                    }
                    else
                    {
                        sb.Append(prefixWithNodePadding);
                        if (i == edges.Length - 1)
                            connector = '└';
                        else
                        {
                            connector = '├';
                            extender = '│';
                        }
                    }
                    sb.Append(connector).Append('─');
                    sb.Append(edges[i].RenderTree(string.Concat(prefixWithNodePadding, extender, ' '), maxEdgeLength));
                }
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return string.Concat("node #", NodeNumber);
        }
    }
}
