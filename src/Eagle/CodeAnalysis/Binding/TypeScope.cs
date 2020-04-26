using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Eagle.CodeAnalysis.Binding
{
    public class TypeScope : IScope
    {
        public IScope Parent { get; }
        public TypeSymbol Type { get; }

        public TypeScope(IScope parent, TypeSymbol type)
        {
            Parent = parent;
            Type = type;
        }

        public bool TryDeclare(Symbol symbol)
        {
            var symbols = Type.Members;
            // applies to type scope
            // fields with same name can be declared only once 
            // property with same name can be declared only once
            // methods with same name and same parameters can only be declared once
            // indexer with same name and same parameters can only be declared once
            // constructor with same name and same parameters can only be declared once

            if (!(symbol is VariableSymbol))
            {
                var existingSymbols = symbols.Where(s => s.Name == symbol.Name).ToList();
                if (existingSymbols.Any())
                {
                    // only if the symbol has parameters we can declare it twice
                    ImmutableArray<ParameterSymbol>? parameters = symbol switch
                    {
                        MethodSymbol m => m.Parameters,
                        ConstructorSymbol c => c.Parameters,
                        IndexerSymbol i => i.Parameters,
                        _ => null
                    };
                    if (!parameters.HasValue)
                        return false;

                    // if any of the existing symbols is not of the same type we cant declare it
                    if (existingSymbols.Any(e => e.GetType() != symbol.GetType()))
                        return false;

                    // if all functions have different parameters we can declare it
                    var param = existingSymbols.Select(s =>
                    {
                        ImmutableArray<ParameterSymbol> mParameters = s switch
                        {
                            MethodSymbol m => m.Parameters,
                            ConstructorSymbol c => c.Parameters,
                            IndexerSymbol i => i.Parameters,
                            _ => throw new NotSupportedException()
                        };
                        return mParameters;
                    });
                    return param.All(p => !parameters.SequenceEqual(p));
                }
            }

            Type.Members = symbols.Add((MemberSymbol)symbol);
            return true;
        }

        public bool TryLookup(SymbolKind[] kinds, string name, out Symbol[] symbols)
        {
            var allKinds = !kinds.Any();
            symbols = Type.Members.Where(m => m.Name == name && (allKinds || kinds.Contains(m.Kind))).ToArray();
            if (symbols.Any())
                return true;

            if (Parent != null)
                return Parent.TryLookup(kinds, name, out symbols);

            return false;
        }

        public ImmutableArray<Symbol> GetDeclaredSymbols()
        {
            return Type.Members.ToImmutableArray<Symbol>();
        }
    }
}