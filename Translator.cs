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
    class Translator
    {
        private const string START_PHRASE_SYMBOLS = "- \"";
        private const string FINISh_PHRASE_SYMBOLS = "\"";
        private const string GRAPH_FILE_EXTENSION = ".asset";
        private const string UNESCAPED_SUFFIX = "_unescaped";
        private const string TRANSLATED_SUFFIX = "_translated";


        List<Phrase> phrases = new List<Phrase>();


        public Translator(string excelFilePath, int tableIndex, int originalPhraseColumnIndex, int translatedPhraseColumnIndex)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

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
                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    var originalText = dataTable.Rows[i][originalPhraseColumnIndex].ToString();
                    var translatedText = dataTable.Rows[i][translatedPhraseColumnIndex].ToString();
                    phrases.Add(new Phrase(originalText, translatedText));
                }
            }
        }


        public void Translate(string graphFilePath, string translatedSuffix = TRANSLATED_SUFFIX)
        {
            string graphFileExtension = Path.GetExtension(graphFilePath);
            string graphFilePathWithoutExtension = Path.ChangeExtension(graphFilePath, "");
            string unescapedGraphFilePath = graphFilePathWithoutExtension + UNESCAPED_SUFFIX + graphFileExtension;
            string translatedGraphFilePath = graphFilePathWithoutExtension + translatedSuffix + graphFileExtension;

            var unescapedText = Regex.Unescape(File.ReadAllText(graphFilePath));
            File.WriteAllText(unescapedGraphFilePath, unescapedText);

            var graphText = File.ReadAllText(graphFilePath);
            int startPhraseSymbolsLength = START_PHRASE_SYMBOLS.Length;
            int searchIndex = 0;
            while (searchIndex < graphText.Length)
            {
                var startIndex = graphText.IndexOf(START_PHRASE_SYMBOLS, searchIndex);
                var finishIndex = graphText.IndexOf(FINISh_PHRASE_SYMBOLS, startIndex + startPhraseSymbolsLength);
                var nodeText = Regex.Unescape(graphText.Substring(startIndex + startPhraseSymbolsLength, finishIndex - startIndex - startPhraseSymbolsLength));

                var comparer = new StringCompIgnoreWhiteSpace();
                for (int i = 0; i < phrases.Count; i++)
                {
                    if (comparer.Equals(phrases[i].original, nodeText))
                    {
                        var graphTextBuilder = new StringBuilder(graphText);
                        graphTextBuilder.Remove(startIndex + startPhraseSymbolsLength, finishIndex - startIndex - startPhraseSymbolsLength);
                        graphTextBuilder.Insert(startIndex + startPhraseSymbolsLength, phrases[i].translated);
                        graphText = graphTextBuilder.ToString();

                        searchIndex = startIndex + phrases[i].translated.Length;

                        break;
                    }
                    else
                    {
                        searchIndex += startPhraseSymbolsLength;
                    }
                }
            }

            File.WriteAllText(translatedGraphFilePath, graphText);

            Console.WriteLine($"\"{graphFilePath}\" was successfuly translated");
        }
        public void AddPhrase(Phrase phrase)
        {
            phrases.Add(phrase);
        }
    }
    public struct Phrase
    {
        public string original;
        public string translated;


        public Phrase(string original, string translated)
        {
            this.original = original;
            this.translated = translated;
        }
    }
}
