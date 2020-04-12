namespace Eagle.CodeAnalysis.Binding
{
    public static class ScopeExtensions
    {
        public static TypeSymbol GetTypeSymbol(this IScope scope, string name)
        {
            scope.TryLookup(name, out var symbol);
            return symbol as TypeSymbol;
        }
    }
}