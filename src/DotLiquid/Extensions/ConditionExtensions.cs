using System.Collections.Generic;
using System.Linq;

namespace DotLiquid.Extensions
{
    public static class ConditionExtensions
    {
        public static bool HasComplexSymbols(this Condition condition)
        {
            if (condition.Left.IsSymbol())
            {
                var leftSymbol = condition.Left.GetSymbol();
                if (leftSymbol.IsComplexType)
                    return true;
            }
            if (condition.Right.IsSymbol())
            {
                var rightSymbol = condition.Right.GetSymbol();
                if (rightSymbol.IsComplexType)
                    return true;
            }
            if (condition.Attachment != null && condition.Attachment.Any())
            {
                if (condition.Attachment.HasComplexSymbols())
                    return true;
            }
            if (condition.ChildCondition != null) // Recurse down child conditions
            {
                if (condition.ChildCondition.HasComplexSymbols())
                    return true;
            }

            return false;
        }

        public static Dictionary<string, Symbol> GetSimpleSymbols(this Condition condition, Dictionary<string, Symbol> symbols)
        {
            if (condition.Left.IsSymbol())
            {
                var leftSymbol = condition.Left.GetSymbol();
                var symbolType = string.IsNullOrEmpty(condition.Right) ? SymbolType.Boolean : condition.Right.GetSymbolType();

                if (!symbols.ContainsKey(leftSymbol.Name))
                    symbols.Add(leftSymbol.Name, leftSymbol);

                symbols[leftSymbol.Name].SymbolType = symbolType;

            }
            if (condition.Right.IsSymbol())
            {
                var rightSymbol = condition.Right.GetSymbol();
                var symbolType = string.IsNullOrEmpty(condition.Left) ? SymbolType.Boolean : condition.Left.GetSymbolType();

                if (!symbols.ContainsKey(rightSymbol.Name))
                    symbols.Add(rightSymbol.Name, rightSymbol);

                symbols[rightSymbol.Name].SymbolType = symbolType;
            }
            if (condition.Attachment != null && condition.Attachment.Any())
            {
                symbols = condition.Attachment.GetSimpleSymbols(symbols);
            }
            if (condition.ChildCondition != null) // Recurse down child conditions
            {
                symbols = condition.ChildCondition.GetSimpleSymbols(symbols);
            }

            return symbols;
        }
    }
}
