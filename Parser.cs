using System;
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
            graph.AddNode(new LocationNode(new Vector2(), 1, "VillageFarView"));
            graph.AddNode(new CharacterPositionNode(new Vector2(), 2, Position.Left));
            graph.AddNode(new StopNode(new Vector2(), 3));
        }


        public void Save()
        {
            string assetText = graph.ToString();
            File.WriteAllText(Path.Combine("Graphs", graph.Name + ".asset"), assetText);
        }
    }
}
