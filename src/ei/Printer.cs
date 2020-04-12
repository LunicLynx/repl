using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.CodeAnalysis;
using Eagle.CodeAnalysis.Binding;
using Eagle.CodeAnalysis.Syntax;

namespace Eagle
{
    public class Printer
    {
        public void Print(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(indent);
            Console.Write(marker);

            Console.ForegroundColor = node is Token ? ConsoleColor.Blue : ConsoleColor.Cyan;

            if (node is Token t)
            {
                Console.Write(t);
            }
            else
            {
                Console.Write(node.GetType().Name);
            }

            Console.ResetColor();

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                Print(child, indent, child == lastChild);
        }

        private void Print(BoundNode node)
        {
            Print(node, "", true);
        }

        private void Print(BoundNode node, string indent, bool isLast)
        {
            var marker = isLast ? "└──" : "├──";

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(indent);
            Console.Write(marker);

            Console.ForegroundColor = GetColor(node);
            Console.Write(GetText(node));
            var firstProperty = true;
            foreach (var p in GetProperties(node))
            {
                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(",");
                }
                Console.Write(" ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(p.name);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" = ");

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                Console.Write(p.value);
            }

            Console.ResetColor();

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                Print(child, indent, child == lastChild);
        }

        private IEnumerable<(string name, object value)> GetProperties(BoundNode node)
        {
            var type = node.GetType();
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public |
                                                System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.Name == nameof(BoundBinaryExpression.Operator) ||
                    property.Name == nameof(BoundUnaryExpression.Operator))
                    continue;

                var propertyType = property.PropertyType;
                if (typeof(BoundNode).IsAssignableFrom(propertyType) ||
                    typeof(IEnumerable<BoundNode>).IsAssignableFrom(propertyType))
                    continue;

                var value = property.GetValue(node, null);
                if (value != null)
                    yield return (property.Name, value);
            }
        }

        private ConsoleColor GetColor(BoundNode node)
        {
            switch (node)
            {
                case BoundExpression _: return ConsoleColor.Blue;
                case BoundStatement _: return ConsoleColor.Cyan;
                default: return ConsoleColor.Yellow;
            }
        }

        private string GetText(BoundNode node)
        {
            switch (node)
            {
                case BoundBinaryExpression b: return b.Operator.Kind + "Expression";
                case BoundUnaryExpression u: return u.Operator.Kind + "Expression";
                default: return node.GetType().Name;
            }
        }

        internal void Print(Compilation compilation)
        {
            
            //compilation.Print(Print);
        }
    }
}