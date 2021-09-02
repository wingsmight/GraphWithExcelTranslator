using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
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
