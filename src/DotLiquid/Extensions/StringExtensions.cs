using System;
using System.Text.RegularExpressions;

namespace DotLiquid.Extensions
{
    public static class StringExtensions
    {
        private static Regex numberRegex = new Regex(@"^[0-9]+(\.[0-9]+)?$");

        public static bool IsSymbol(this string testString)
        {
            if (string.IsNullOrEmpty(testString))
                return false;

            if ((testString.StartsWith(".") || testString.EndsWith(".")))
                return false;

            if ((testString.StartsWith("\"") || testString.StartsWith("\'")) &&
                (testString.EndsWith("\"") || testString.EndsWith("\'")))
                return false;

            if (testString.ToLower().Equals("true") || testString.ToLower().Equals("false"))
                return false;

            if (numberRegex.IsMatch(testString))
                return false;

            return true;
        }

        public static SymbolType GetSymbolType(this string testString)
        {
            if (string.IsNullOrEmpty(testString))
                return SymbolType.Unknown;

            if ((testString.StartsWith(".") || testString.EndsWith(".")))
                return SymbolType.Unknown;

            if ((testString.StartsWith("\"") || testString.StartsWith("\'")) &&
                (testString.EndsWith("\"") || testString.EndsWith("\'")))
                return SymbolType.String;

            if (testString.ToLower().Equals("true") || testString.ToLower().Equals("false"))
                return SymbolType.Boolean;

            if (numberRegex.IsMatch(testString))
                return SymbolType.Number;

            return SymbolType.String;
        }

        public static Symbol GetSymbol(this string symbolName)
        {
            if (!symbolName.Contains("."))
            {
                return new Symbol(symbolName);
            }

            var symbolParts = symbolName.Split('.');

            return new Symbol(symbolParts[0], SymbolType.Complex);
        }
    }
}
