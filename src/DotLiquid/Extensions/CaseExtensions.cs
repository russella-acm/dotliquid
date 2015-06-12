using System;
using System.Collections.Generic;
using DotLiquid.Tags;

namespace DotLiquid.Extensions
{
    public static class CaseExtensions
    {
        public static bool HasComplexSymbols(this Case caseTag)
        {
            foreach (var block in caseTag.Blocks)
            {
                if (block.HasComplexSymbols())
                    return true;
            }

            if (caseTag.NodeList.HasComplexSymbols())
                return true;

            return false;
        }

        public static Dictionary<string, Symbol> GetSimpleSymbols(this Case caseTag, Dictionary<string, Symbol> symbols)
        {
            foreach (var block in caseTag.Blocks)
            {
                symbols = block.GetSimpleSymbols(symbols);
            }

            symbols = caseTag.NodeList.GetSimpleSymbols(symbols);

            return symbols;
        }
    }
}
