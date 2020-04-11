using System;
using System.Collections.Generic;
using System.Text;

namespace Repl.CodeAnalysis.Binding
{
    class Conversion
    {
        public static readonly Conversion None = new Conversion(exists: false, isIdentity: false, isImplicit: false);
        public static readonly Conversion Identity = new Conversion(exists: true, isIdentity: true, isImplicit: true);
        public static readonly Conversion Implicit = new Conversion(exists: true, isIdentity: false, isImplicit: true);
        public static readonly Conversion Explicit = new Conversion(exists: true, isIdentity: false, isImplicit: false);

        public bool IsImplicit { get; }
        public bool Exists { get; }
        public bool IsIdentity { get; }
        public bool IsExplicit => Exists && !IsImplicit;

        private Conversion(bool exists, bool isIdentity, bool isImplicit)
        {
            IsImplicit = isImplicit;
            Exists = exists;
            IsIdentity = isIdentity;
        }

        public static Conversion Classify(TypeSymbol from, TypeSymbol to)
        {
            if (from == to)
                return Identity;

            if (from != TypeSymbol.Void && to == TypeSymbol.Any)
            {
                return Implicit;
            }

            if (from == TypeSymbol.Any && to != TypeSymbol.Void)
            {
                return Explicit;
            }

            // TODO remove, if we want something like this
            // then we should have a ToString method or something like that
            // better would be a trait / interface
            if (from == TypeSymbol.Bool || from.IsInteger())
            {
                if (to == TypeSymbol.String)
                    return Explicit;
            }

            // TODO "same" as above. There should be a parse function or convertible trait / interface.
            if (from == TypeSymbol.String)
            {
                if (to == TypeSymbol.Bool || to.IsInteger())
                    return Explicit;
            }

            if (from.IsInteger() && to.IsInteger())
            {
                if (from.IsSigned() == to.IsSigned() || to.IsSigned())
                    return from.GetBits() < to.GetBits()
                        ? Implicit
                        : Explicit;
                return Explicit;
            }

            if (from.IsInteger() && to.IsFloat())
                return Implicit;
            if (from.IsFloat() && to.IsInteger())
                return Explicit;

            return None;
        }
    }
}
