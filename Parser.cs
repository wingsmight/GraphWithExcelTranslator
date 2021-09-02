using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace ExcelToGraph
{
    class Parser
    {
        private Graph graph;


        public Parser(string graphName)
        {
            graph = new Graph(graphName);
            int elementsCount = 1;
            graph.AddNode(new LocationNode(new Vector2(), elementsCount++, "VillageFarView"));
            graph.AddNode(new CharacterPositionNode(new Vector2(), elementsCount++, Position.Left));
            graph.AddNode(new StopNode(new Vector2(), elementsCount++));
            graph.AddNode(new CharacterNode(new Vector2(), elementsCount++, new CharacterProperty("James", Position.Left, Emotion.Usual), Direction.FromLeft));
            graph.AddNode(new MonologueNode(new Vector2(), elementsCount++, new List<string>() { "test0", "test1" }, "James"));
            graph.AddNode(new SplitMessageNode(new Vector2(), elementsCount++, new List<string>() { "test0", "test1", "test2" }));
            graph.AddNode(new DialogueNode(new Vector2(), elementsCount++, "", "", new List<int>() { 311, 312, 313 }));

            graph.AddNodeLink(new NodeLinkData(elementsCount++, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }


        public void Save()
        {
            string assetText = graph.ToString();
            File.WriteAllText(Path.Combine("Graphs", graph.Name + ".asset"), assetText);
        }
    }
}
