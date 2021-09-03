using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelToGraph
{
    class Graph
    {
        private const string EMPTY_GRAPH_PATH = "Graphs/Empty.asset";
        private static readonly Vector2 NODE_OFFSET = new Vector2(550, 0);


        private string name;
        private List<string> assetLines;
        private Node lastNode;
        private int elementsCount = 0;


        public Graph(string name)
        {
            this.name = name;

            assetLines = File.ReadAllText(EMPTY_GRAPH_PATH).Split('\n').ToList();

            SetProperty("m_Name", name);

            var entryNode = new EntryNode();
            lastNode = entryNode;
            AddNode(entryNode);

            SetProperty("entryGUID", entryNode.GUID);
        }


        public void AddNode(Node node)
        {
            node.ReferenceId = elementsCount++;
            node.Position = lastNode.Position + NODE_OFFSET;

            AddNodeLink(new NodeLinkData(0, lastNode, node));

            var nodeLinksIndex = assetLines.FindIndex(x => x.StartsWith(Node.TAB.Multiply(1) + "nodeLinks"));
            assetLines.Insert(nodeLinksIndex, Node.TAB.Multiply(1) + "- id: " + node.ReferenceId);

            string[] nodeTexts = node.ToString().Split('\n');
            foreach (var nodeText in nodeTexts)
            {
                assetLines.Add(nodeText);
            }

            lastNode = node;
        }
        public void AddNodeLink(NodeLinkData nodeLink)
        {
            nodeLink.ReferenceId = elementsCount++;

            var exposedVariablesIndex = assetLines.FindIndex(x => x.StartsWith(Node.TAB.Multiply(1) + "exposedVariables"));
            assetLines.Insert(exposedVariablesIndex, Node.TAB.Multiply(1) + "- id: " + nodeLink.ReferenceId);

            string[] nodeLinkTexts = nodeLink.ToString().Split('\n');
            foreach (var nodeLinkText in nodeLinkTexts)
            {
                assetLines.Add(nodeLinkText);
            }
        }
        public override string ToString()
        {
            string text = "";
            foreach (var assetLine in assetLines)
            {
                text += assetLine + '\n';
            }
            return text;
        }

        private void SetProperty(string key, string value)
        {
            string tab = "  ";
            key = tab + key;
            int lineIndex = assetLines.FindIndex(x => x.StartsWith(key));
            assetLines[lineIndex] = key + ": " + value;
        }


        public string Name => name;
    }
}
