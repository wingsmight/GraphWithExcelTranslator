using System;

namespace ExcelToGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser("test");
            parser.Parse("/Users/user/Downloads/UNITY/пошаговый сценарий.xlsx", 0, 13);
            parser.Save();
        }
    }
}
