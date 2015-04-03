using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BioLib.Ukkonen
{
    public class SuffixTree
    {
        public string Word;
        public int CurrentSuffixStartIndex;
        public int CurrentSuffixEndIndex;
        Node LastCreatedNodeInCurrentIteration;
        int UnresolvedSuffixes;
        public Node RootNode;
        Node ActiveNode;
        Edge ActiveEdge;
        int DistanceIntoActiveEdge;
        char LastCharacterOfCurrentSuffix;
        public int NextNodeNumber;
        public int NextEdgeNumber;

        public SuffixTree(string word)
        {
            Word = word;
            RootNode = new Node(this);
            ActiveNode = RootNode;
        }

        public event Action<string, object[]> Message;
        void Log(string format, params object[] args)
        {
            if (Message != null)
                Message(format, args);
        }

        public void Build()
        {
            for (CurrentSuffixEndIndex = 0; CurrentSuffixEndIndex < Word.Length; CurrentSuffixEndIndex++)
            {
                Log("LETTER: " + (CurrentSuffixEndIndex + 1));
                LastCreatedNodeInCurrentIteration = null;
                LastCharacterOfCurrentSuffix = Word[CurrentSuffixEndIndex];

                for (CurrentSuffixStartIndex = CurrentSuffixEndIndex - UnresolvedSuffixes; CurrentSuffixStartIndex <= CurrentSuffixEndIndex; CurrentSuffixStartIndex++)
                {
                    var wasImplicitlyAdded = !AddNextSuffix();
                    Log(ToString());
                    if (wasImplicitlyAdded)
                    {
                        UnresolvedSuffixes++;
                        break;
                    }
                    if (UnresolvedSuffixes > 0)
                        UnresolvedSuffixes--;
                }
            }
        }

        bool AddNextSuffix()
        {
            var suffix = string.Concat(Word.Substring(CurrentSuffixStartIndex, CurrentSuffixEndIndex - CurrentSuffixStartIndex), "{", Word[CurrentSuffixEndIndex], "}");
            Log("Suffix: {0}", 
                Word.Substring(0, CurrentSuffixStartIndex) + "{" + 
                Word.Substring(CurrentSuffixStartIndex, CurrentSuffixEndIndex - CurrentSuffixStartIndex + 1) + "}" +
                Word.Substring(CurrentSuffixEndIndex + 1));

            if (ActiveEdge != null)
                return AddCurrentSuffixToActiveEdge();

            if (GetExistingEdgeAndSetAsActive())
                return false;

            ActiveNode.AddNewEdge();

            UpdateActivePointAfterAddingNewEdge();
            return true;
        }

        bool GetExistingEdgeAndSetAsActive()
        {
            Edge edge;
            if (ActiveNode.Edges.TryGetValue(LastCharacterOfCurrentSuffix, out edge))
            {
                ActiveEdge = edge;
                DistanceIntoActiveEdge = 1;

                NormalizeActivePointIfNowAtOrBeyondEdgeBoundary(ActiveEdge.StartIndex);

                return true;
            }
            return false;
        }

        bool AddCurrentSuffixToActiveEdge()
        {
            var nextCharacterOnEdge = Word[ActiveEdge.StartIndex + DistanceIntoActiveEdge];
            if (nextCharacterOnEdge == LastCharacterOfCurrentSuffix)
            {
                DistanceIntoActiveEdge++;
                NormalizeActivePointIfNowAtOrBeyondEdgeBoundary(ActiveEdge.StartIndex);
                return false;
            }

            SplitActiveEdge();
            ActiveEdge.Tail.AddNewEdge();

            UpdateActivePointAfterAddingNewEdge();

            return true;
        }

        void UpdateActivePointAfterAddingNewEdge()
        {
            if (ReferenceEquals(ActiveNode, RootNode))
            {
                if (DistanceIntoActiveEdge > 0)
                {
                    DistanceIntoActiveEdge--;
                    ActiveEdge = DistanceIntoActiveEdge == 0 ? null : ActiveNode.Edges[Word[CurrentSuffixStartIndex + 1]];

                    NormalizeActivePointIfNowAtOrBeyondEdgeBoundary(CurrentSuffixStartIndex + 1);
                }
            }
            else
                UpdateActivePointToLinkedNodeOrRoot();
        }

        void NormalizeActivePointIfNowAtOrBeyondEdgeBoundary(int firstIndexOfOriginalActiveEdge)
        {
            var walkDistance = 0;
            while (ActiveEdge != null && DistanceIntoActiveEdge >= ActiveEdge.Length)
            {
                DistanceIntoActiveEdge -= ActiveEdge.Length;
                ActiveNode = ActiveEdge.Tail ?? RootNode;
                if (DistanceIntoActiveEdge == 0)
                    ActiveEdge = null;
                else
                {
                    walkDistance += ActiveEdge.Length;
                    var c = Word[firstIndexOfOriginalActiveEdge + walkDistance];
                    ActiveEdge = ActiveNode.Edges[c];
                }
            }
        }

        void SplitActiveEdge()
        {
            ActiveEdge = ActiveEdge.SplitAtIndex(ActiveEdge.StartIndex + DistanceIntoActiveEdge);

            if (LastCreatedNodeInCurrentIteration != null)
            {
                LastCreatedNodeInCurrentIteration.LinkedNode = ActiveEdge.Tail;
            }
            LastCreatedNodeInCurrentIteration = ActiveEdge.Tail;
        }

        void UpdateActivePointToLinkedNodeOrRoot()
        {
            ActiveNode = ActiveNode.LinkedNode != null ? ActiveNode.LinkedNode : RootNode;

            if (ActiveEdge != null)
            {
                var firstIndexOfOriginalActiveEdge = ActiveEdge.StartIndex;
                ActiveEdge = ActiveNode.Edges[Word[ActiveEdge.StartIndex]];
                NormalizeActivePointIfNowAtOrBeyondEdgeBoundary(firstIndexOfOriginalActiveEdge);
            }
        }

        public override string ToString()
        {
            return RootNode.RenderTree("");
        }

        void ExtractAllSubstrings(string str, HashSet<string> set, Node node)
        {
            foreach (var edge in node.Edges.Values)
            {
                var edgeStr = edge.StringWithoutCanonizationChar();
                var edgeLength = !edge.EndIndex.HasValue ? edge.Length - 1 : edge.Length;
                for (var length = 1; length <= edgeLength; length++)
                    set.Add(string.Concat(str, edgeStr.Substring(0, length)));
                if (edge.Tail != null)
                    ExtractAllSubstrings(string.Concat(str, edge.StringWithoutCanonizationChar()), set, edge.Tail);
            }
        }

        void ExtractSubstringsForIndexing(string str, List<string> list, int len, Node node)
        {
            foreach (var edge in node.Edges.Values)
            {
                var newstr = string.Concat(str, Word.Substring(edge.StartIndex, Math.Min(len, edge.Length)));
                if (len > edge.Length && edge.Tail != null)
                    ExtractSubstringsForIndexing(newstr, list, len - edge.Length, edge.Tail);
                else
                    list.Add(newstr);
            }
        }
    }
}