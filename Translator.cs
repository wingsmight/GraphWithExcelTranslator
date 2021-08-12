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
        private const string FINISH_PHRASE_SYMBOLS = "\"";

        private const string START_CHOICE_SYMBOLS = "sourcePortName: \"";
        private const string FINISH_CHOICE_SYMBOLS = "\"";

        private const string START_RESPONSE_DATA_SYMBOLS = "text: \"";
        private const string FINISH_RESPONSE_DATA_SYMBOLS = "\"";

        private const string GRAPH_FILE_EXTENSION = ".asset";
        private const string UNESCAPED_SUFFIX = "_unescaped";
        private const string TRANSLATED_SUFFIX = "_translated";

        private const string CHOICE_PHRASE_TRIGGER = "Предоставляется выбор";


        List<Phrase> phrases = new List<Phrase>();
        List<ChoiceSet> choices = new List<ChoiceSet>();


        public Translator(string excelFilePath, int tableIndex, int originalPhraseColumnIndex, int translatedPhraseColumnIndex, int actionColumnIndex)
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

                    string actionText = dataTable.Rows[i][actionColumnIndex].ToString();
                    if (actionText.StartsWith(CHOICE_PHRASE_TRIGGER))
                    {
                        var newChoiceSet = new ChoiceSet();
                        var originalChoices = originalText.Split('\n');
                        var translatedChoices = translatedText.Split('\n');

                        for (int choiceIndex = 0; choiceIndex < originalChoices.Length && choiceIndex < translatedChoices.Length; choiceIndex++)
                        {
                            newChoiceSet.choices.Add(new Phrase(originalChoices[choiceIndex], translatedChoices[choiceIndex]));
                        }

                        choices.Add(newChoiceSet);
                    }
                }
            }
        }


        public void Translate(string graphFilePath, string translatedSuffix = TRANSLATED_SUFFIX)
        {
            string graphFileExtension = Path.GetExtension(graphFilePath);
            string graphFilePathWithoutExtension = Path.ChangeExtension(graphFilePath, "");
            graphFilePathWithoutExtension = graphFilePathWithoutExtension.Substring(0, graphFilePathWithoutExtension.Length - 1);
            string unescapedGraphFilePath = graphFilePathWithoutExtension + UNESCAPED_SUFFIX + graphFileExtension;
            string translatedGraphFilePath = graphFilePathWithoutExtension + translatedSuffix + graphFileExtension;

            var unescapedText = Regex.Unescape(File.ReadAllText(graphFilePath));
            File.WriteAllText(unescapedGraphFilePath, unescapedText);

            var graphText = File.ReadAllText(graphFilePath);

            TranslatePhrases(ref graphText);
            TranslatePorts(ref graphText);

            File.WriteAllText(translatedGraphFilePath, graphText);

            Console.WriteLine($"\"{graphFilePath}\" was successfuly translated");
        }
        public void AddPhrase(Phrase phrase)
        {
            phrases.Add(phrase);
        }

        private void TranslatePhrases(ref string graphText)
        {
            int startPhraseSymbolsLength = START_PHRASE_SYMBOLS.Length;
            int searchIndex = 0;
            while (searchIndex < graphText.Length)
            {
                var startIndex = graphText.IndexOf(START_PHRASE_SYMBOLS, searchIndex);
                var finishIndex = graphText.IndexOf(FINISH_PHRASE_SYMBOLS, startIndex + startPhraseSymbolsLength);
                var nodeText = Regex.Unescape(graphText.SubstringWithinSymbols(START_PHRASE_SYMBOLS, FINISH_PHRASE_SYMBOLS, searchIndex));

                var comparer = new StringCompIgnoreWhiteSpace();
                for (int i = 0; i < phrases.Count; i++)
                {
                    string testString = "Такие поселения";
                    if (phrases[i].original.Length > testString.Length && phrases[i].original.Substring(0, testString.Length) == testString)
                        if (nodeText.Length > testString.Length && nodeText.Substring(0, testString.Length) == testString)
                        {

                        }
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
        }
        private void TranslatePorts(ref string graphText)
        {
            int startChoiceSymbolsLength = START_CHOICE_SYMBOLS.Length;
            int searchIndex = 0;
            while (searchIndex < graphText.Length)
            {
                var startIndex = graphText.IndexOf(START_CHOICE_SYMBOLS, searchIndex);
                var finishIndex = graphText.IndexOf(FINISH_CHOICE_SYMBOLS, startIndex + startChoiceSymbolsLength);
                var nodeText = Regex.Unescape(graphText.SubstringWithinSymbols(START_CHOICE_SYMBOLS, FINISH_CHOICE_SYMBOLS, searchIndex));

                var comparer = new StringCompIgnoreWhiteSpace();
                for (int i = 0; i < choices.Count; i++)
                {
                    foreach (var choice in choices[i].choices)
                    {
                        if (comparer.Equals(choice.original, nodeText))
                        {
                            var graphTextBuilder = new StringBuilder(graphText);
                            graphTextBuilder.Remove(startIndex + startChoiceSymbolsLength, finishIndex - startIndex - startChoiceSymbolsLength);
                            graphTextBuilder.Insert(startIndex + startChoiceSymbolsLength, choice.translated);
                            graphText = graphTextBuilder.ToString();

                            // DialogueResponseData
                            int startResponseDataSymbolsLength = START_RESPONSE_DATA_SYMBOLS.Length;
                            int searchIndex1 = 0;
                            while (searchIndex1 < graphText.Length)
                            {
                                var startSubIndex = graphText.IndexOf(START_RESPONSE_DATA_SYMBOLS, searchIndex1);
                                var finishSubIndex = graphText.IndexOf(FINISH_RESPONSE_DATA_SYMBOLS, startSubIndex + startResponseDataSymbolsLength);
                                var nodeSubText = Regex.Unescape(graphText.Substring(startSubIndex + startResponseDataSymbolsLength, finishSubIndex - startSubIndex - startResponseDataSymbolsLength));

                                if (comparer.Equals(choice.original, nodeSubText))
                                {
                                    graphTextBuilder.Remove(startSubIndex + startResponseDataSymbolsLength, finishSubIndex - startSubIndex - startResponseDataSymbolsLength);
                                    graphTextBuilder.Insert(startSubIndex + startResponseDataSymbolsLength, choice.translated);

                                    graphText = graphTextBuilder.ToString();

                                    searchIndex1 = startSubIndex + choice.translated.Length;

                                    break;
                                }
                                else
                                {
                                    searchIndex1 += startChoiceSymbolsLength;
                                }
                            }
                            //

                            graphText = graphTextBuilder.ToString();

                            searchIndex1 = startIndex + choice.translated.Length;

                            break;
                        }
                        else
                        {
                            searchIndex += startChoiceSymbolsLength;
                        }
                    }
                }
            }
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
    public class ChoiceSet
    {
        public List<Phrase> choices = new List<Phrase>();
    }
}
