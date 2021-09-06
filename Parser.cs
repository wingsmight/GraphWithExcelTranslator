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
        }


        public void Parse(string excelFilePath, int tableIndex = 0, int startRowIndex = 0)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // Do you need it?

            using (var excelStream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(excelStream);
                var configuration = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };
                var dataSet = excelReader.AsDataSet(configuration);
                var dataTable = dataSet.Tables[tableIndex];
                for (var rowIndex = startRowIndex; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    var row = dataTable.Rows[rowIndex];

                    // events
                    string eventCell = row[3].ToString();
                    if (eventCell.StartsWith("Появляется"))
                    {
                        string emotionCell = row[2].ToString();
                        CharacterProperty appearingCharacter = new CharacterProperty(
                            name: eventCell.SubstringWithinSymbols("Появляется ", ","),
                            positionName: eventCell.Substring(eventCell.IndexOf("позиция ") + "позиция ".Length),
                            emotionName: emotionCell
                        );

                        CharacterNode characterNode = new CharacterNode(Vector2.Zero, 0, appearingCharacter);
                        graph.AddNode(characterNode);
                    }
                    else if (eventCell.StartsWith("Загрузка"))
                    {
                        var locationName = row[1].ToString();
                        LocationNode locationNode = new LocationNode(Vector2.Zero, 0, locationName);
                        graph.AddNode(locationNode);
                    }

                    // messages
                    string speakerName = row[4].ToString();
                    if (string.IsNullOrEmpty(speakerName))
                    {
                        continue;
                    }

                    List<string> messages = new List<string>()
                    {
                        row[5].ToString()
                    };

                    for (; rowIndex + 1 < dataTable.Rows.Count; rowIndex++)
                    {
                        bool isNextRowMessage = true;
                        var nextRow = dataTable.Rows[rowIndex + 1];

                        // Check
                        if (nextRow[4].ToString() != speakerName)
                        {
                            break;
                        }
                        for (int columnIndex = 1; columnIndex <= 3; columnIndex++)
                        {
                            if (!string.IsNullOrEmpty(nextRow[columnIndex].ToString()))
                            {
                                isNextRowMessage = false;
                                break;
                            }
                        }

                        // Insert to message list
                        if (isNextRowMessage)
                        {
                            messages.Add(nextRow[5].ToString());
                        }
                        else
                        {
                            break;
                        }
                    }

                    try
                    {
                        MonologueNode monologueNode = new MonologueNode(Vector2.Zero, 0, messages, speakerName);
                        graph.AddNode(monologueNode);
                    }
                    catch
                    {
                        Console.WriteLine();
                    }
                }
            }
        }
        public void Save()
        {
            string assetText = graph.ToString();
            File.WriteAllText(Path.Combine("Graphs", graph.Name + ".asset"), assetText);
        }
    }
}
