using System;

public static class StringExt
{
    public static string SubstringWithinSymbols(this string text, char startSymbol, char finishSymbol, int searchIndex = 0)
    {
        return SubstringWithinSymbols(text, startSymbol.ToString(), finishSymbol.ToString(), searchIndex);
    }
    public static string SubstringWithinSymbols(this string text, string startSymbols, string finishSymbols, int searchIndex = 0)
    {
        int startSymbolsLength = startSymbols.Length;
        var startIndex = text.IndexOf(startSymbols, searchIndex);
        var finishIndex = text.IndexOf(finishSymbols, startIndex + startSymbolsLength);
        return text.Substring(startIndex + startSymbolsLength, finishIndex - startIndex - startSymbolsLength);
    }
}