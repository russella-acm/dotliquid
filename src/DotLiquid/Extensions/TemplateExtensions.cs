using System;
using System.Collections.Generic;
using System.Linq;
using DotLiquid.Tags;

namespace DotLiquid.Extensions
{
    public static class TemplateExtensions
    {
        public static bool HasComplexSymbols(this Template template)
        {
            var hasComplexSymbols = template.Root.NodeList.HasComplexSymbols();
            return hasComplexSymbols;
        }

        public static List<Symbol> GetSimpleSymbols(this Template template)
        {
            if (template.Root.NodeList.HasComplexSymbols())
                return null;

            var symbols = template.Root.NodeList.GetSimpleSymbols(new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase));
            return symbols.Values.ToList();
        }
    }
}
