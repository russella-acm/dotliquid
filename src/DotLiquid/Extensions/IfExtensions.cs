using System;
using System.Collections.Generic;
using DotLiquid.Tags;

namespace DotLiquid.Extensions
{
    public static class IfExtensions
    {
        public static bool HasComplexSymbols(this If ifTag)
        {
            foreach (var block in ifTag.Blocks)
            {
                if (block.HasComplexSymbols())
                    return true;
            }

            if (ifTag.NodeList.HasComplexSymbols())
                return true;

            return false;
        }

        public static Dictionary<string, Symbol> GetSimpleSymbols(this If ifTag, Dictionary<string, Symbol> symbols)
        {
            foreach (var block in ifTag.Blocks)
            {
                symbols = block.GetSimpleSymbols(symbols);
            }

            symbols = ifTag.NodeList.GetSimpleSymbols(symbols);

            return symbols;
        }
    }
}
