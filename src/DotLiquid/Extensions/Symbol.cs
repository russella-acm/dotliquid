using System;

namespace DotLiquid.Extensions
{
    public class Symbol
    {
        public string Name { get; set; }
        public bool IsComplexType 
        {
            get { return SymbolType == SymbolType.Complex; }
        }
        public SymbolType SymbolType { get; set; }

        public Symbol(string name, SymbolType symbolType = SymbolType.String)
        {
            Name = name;
            SymbolType = symbolType;
        }

        public override string ToString()
        {
            return String.Format("{0}|{1}", Name, SymbolType.ToString().ToLower());
        }
    }
}
