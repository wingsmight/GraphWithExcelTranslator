using System;

namespace ExcelToGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser("test");
            parser.Save();
        }
    }
}
