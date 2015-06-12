using System;
using System.Collections.Generic;
using DotLiquid.Tags;

namespace DotLiquid.Extensions
{
    public static class NodeListExtensions
    {
        public static bool HasComplexSymbols(this List<object> nodeList)
        {
            foreach (var node in nodeList)
            {
                var variable = node as Variable;
                if (variable != null)
                {
                    if (!variable.Name.IsSymbol())
                        continue;

                    var variableSymbol = variable.Name.GetSymbol();
                    if (variableSymbol.IsComplexType)
                        return true;
                    
                    continue;
                }

                var @for = node as For;
                if (@for != null)
                {
                    return true;
                }

                var @if = node as If;
                if (@if != null)
                {
                    if (@if.HasComplexSymbols())
                        return true;
                }

                var @case = node as Case;
                if (@case != null)
                {
                    if (@case.HasComplexSymbols())
                        return true;
                }
            }

            return false;
        }

        public static Dictionary<string, Symbol> GetSimpleSymbols(this List<object> nodeList, Dictionary<string, Symbol> symbols)
        {
            if (nodeList.HasComplexSymbols())
                return null;

            foreach (var node in nodeList)
            {
                var variable = node as Variable;
                if (variable != null)
                {
                    if (!variable.Name.IsSymbol())
                        continue;

                    var variableSymbol = variable.Name.GetSymbol();
                    if (!symbols.ContainsKey(variableSymbol.Name))
                    {
                        symbols.Add(variableSymbol.Name, variableSymbol);
                    }
                }

                var @if = node as If;
                if (@if != null)
                {
                    symbols = @if.GetSimpleSymbols(symbols);
                }

                var @case = node as Case;
                if (@case != null)
                {
                    symbols = @case.GetSimpleSymbols(symbols);
                }
            }

            return symbols;
        }
    }
}
