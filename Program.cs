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
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser("test");
            parser.Save();
        }
    }
}
