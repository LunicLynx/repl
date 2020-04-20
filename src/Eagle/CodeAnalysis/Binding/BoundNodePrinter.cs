using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using Eagle.CodeAnalysis.Syntax;
using Eagle.IO;

namespace Eagle.CodeAnalysis.Binding
{
    internal static class BoundNodePrinter
    {
        public static void WriteTo(this BoundNode node, TextWriter writer)
        {
            if (writer is IndentedTextWriter iw)
                WriteTo(node, iw);
            else
                WriteTo(node, new IndentedTextWriter(writer));
        }

        public static void WriteTo(this BoundNode node, IndentedTextWriter writer)
        {
            switch (node)
            {
                case BoundBlockStatement n:
                    WriteBlockStatement(n, writer);
                    break;
                case BoundVariableDeclaration n:
                    WriteVariableDeclaration(n, writer);
                    break;
                case BoundIfStatement n:
                    WriteIfStatement(n, writer);
                    break;
                case BoundWhileStatement n:
                    WriteWhileStatement(n, writer);
                    break;
                //case BoundDoWhileStatement:
                //    WriteDoWhileStatement(n, writer);
                //    break;
                case BoundForStatement n:
                    WriteForStatement(n, writer);
                    break;
                case BoundLabelStatement n:
                    WriteLabelStatement(n, writer);
                    break;
                case BoundGotoStatement n:
                    WriteGotoStatement(n, writer);
                    break;
                case BoundConditionalGotoStatement n:
                    WriteConditionalGotoStatement(n, writer);
                    break;
                case BoundReturnStatement n:
                    WriteReturnStatement(n, writer);
                    break;
                case BoundExpressionStatement n:
                    WriteExpressionStatement(n, writer);
                    break;
                case BoundErrorExpression n:
                    WriteErrorExpression(n, writer);
                    break;
                case BoundLiteralExpression n:
                    WriteLiteralExpression(n, writer);
                    break;
                case BoundVariableExpression n:
                    WriteVariableExpression(n, writer);
                    break;
                case BoundAssignmentExpression n:
                    WriteAssignmentExpression(n, writer);
                    break;
                case BoundUnaryExpression n:
                    WriteUnaryExpression(n, writer);
                    break;
                case BoundBinaryExpression n:
                    WriteBinaryExpression(n, writer);
                    break;
                case BoundMethodCallExpression n:
                    WriteCallExpression(n, writer);
                    break;
                case BoundConstructorCallExpression n:
                    WriteCallExpression(n, writer);
                    break;
                case BoundFunctionCallExpression n:
                    WriteCallExpression(n, writer);
                    break;
                case BoundConversionExpression n:
                    WriteConversionExpression(n, writer);
                    break;
                case BoundArrayIndexExpression a:
                    WriteArrayIndexExpression(a, writer);
                    break;
                case BoundThisExpression t:
                    WriteThisExpression(t, writer);
                    break;
                case BoundNewArrayExpression n:
                    WriteNewArrayExpression(n, writer);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.GetType()}");
            }
        }

        private static void WriteNewArrayExpression(BoundNewArrayExpression node, IndentedTextWriter writer)
        {
            writer.WriteKeyword(TokenKind.NewKeyword);
            writer.WriteSpace();
            writer.WriteIdentifier(node.Type.Name);

            var arguments = node.Arguments;
            writer.WritePunctuation(TokenKind.OpenBracket);

            var isFirst = true;
            foreach (var argument in arguments)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    writer.WritePunctuation(TokenKind.Comma);
                    writer.WriteSpace();
                }

                argument.WriteTo(writer);
            }

            writer.WritePunctuation(TokenKind.CloseBracket);
        }

        private static void WriteThisExpression(BoundThisExpression node, IndentedTextWriter writer)
        {
            writer.WriteKeyword(TokenKind.ThisKeyword);
        }

        private static void WriteArrayIndexExpression(BoundArrayIndexExpression node, IndentedTextWriter writer)
        {
            node.Target.WriteTo(writer);

            var arguments = node.Arguments;
            writer.WritePunctuation(TokenKind.OpenBracket);

            var isFirst = true;
            foreach (var argument in arguments)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    writer.WritePunctuation(TokenKind.Comma);
                    writer.WriteSpace();
                }

                argument.WriteTo(writer);
            }

            writer.WritePunctuation(TokenKind.CloseBracket);
        }

        private static void WriteNestedStatement(this IndentedTextWriter writer, BoundStatement node)
        {
            var needsIndentation = !(node is BoundBlockStatement);

            if (needsIndentation)
                writer.Indent++;

            node.WriteTo(writer);

            if (needsIndentation)
                writer.Indent--;
        }

        private static void WriteNestedExpression(this IndentedTextWriter writer, int parentPrecedence, BoundExpression expression)
        {
            if (expression is BoundUnaryExpression unary)
                writer.WriteNestedExpression(parentPrecedence, SyntaxFacts.GetUnaryOperatorPrecedence(unary.Operator.TokenKind), unary);
            else if (expression is BoundBinaryExpression binary)
                writer.WriteNestedExpression(parentPrecedence, SyntaxFacts.GetBinaryOperatorPrecedence(binary.Operator.TokenKind), binary);
            else
                expression.WriteTo(writer);
        }

        private static void WriteNestedExpression(this IndentedTextWriter writer, int parentPrecedence, int currentPrecedence, BoundExpression expression)
        {
            var needsParenthesis = parentPrecedence >= currentPrecedence;

            if (needsParenthesis)
                writer.WritePunctuation(TokenKind.OpenParenthesis);

            expression.WriteTo(writer);

            if (needsParenthesis)
                writer.WritePunctuation(TokenKind.CloseParenthesis);
        }

        private static void WriteBlockStatement(BoundBlockStatement node, IndentedTextWriter writer)
        {
            writer.WritePunctuation(TokenKind.OpenBrace);
            writer.WriteLine();
            writer.Indent++;

            foreach (var s in node.Statements)
                s.WriteTo(writer);

            writer.Indent--;
            writer.WritePunctuation(TokenKind.CloseBrace);
            writer.WriteLine();
        }

        private static void WriteVariableDeclaration(BoundVariableDeclaration node, IndentedTextWriter writer)
        {
            writer.WriteKeyword(node.Variable.IsReadOnly ? TokenKind.LetKeyword : TokenKind.VarKeyword);
            writer.WriteSpace();
            writer.WriteIdentifier(node.Variable.Name);
            writer.WriteSpace();
            writer.WritePunctuation(TokenKind.Equals);
            writer.WriteSpace();
            node.Initializer.WriteTo(writer);
            writer.WriteLine();
        }

        private static void WriteIfStatement(BoundIfStatement node, IndentedTextWriter writer)
        {
            writer.WriteKeyword(TokenKind.IfKeyword);
            writer.WriteSpace();
            node.Condition.WriteTo(writer);
            writer.WriteLine();
            writer.WriteNestedStatement(node.ThenStatement);

            if (node.ElseStatement != null)
            {
                writer.WriteKeyword(TokenKind.ElseKeyword);
                writer.WriteLine();
                writer.WriteNestedStatement(node.ElseStatement);
            }
        }

        private static void WriteWhileStatement(BoundWhileStatement node, IndentedTextWriter writer)
        {
            writer.WriteKeyword(TokenKind.WhileKeyword);
            writer.WriteSpace();
            node.Condition.WriteTo(writer);
            writer.WriteLine();
            writer.WriteNestedStatement(node.Body);
        }

        //private static void WriteDoWhileStatement(BoundDoWhileStatement node, IndentedTextWriter writer)
        //{
        //    writer.WriteKeyword(TokenKind.DoKeyword);
        //    writer.WriteLine();
        //    writer.WriteNestedStatement(node.Body);
        //    writer.WriteKeyword(TokenKind.WhileKeyword);
        //    writer.WriteSpace();
        //    node.Condition.WriteTo(writer);
        //    writer.WriteLine();
        //}

        private static void WriteForStatement(BoundForStatement node, IndentedTextWriter writer)
        {
            writer.WriteKeyword(TokenKind.ForKeyword);
            writer.WriteSpace();
            writer.WriteIdentifier(node.Variable.Name);
            writer.WriteSpace();
            writer.WritePunctuation(TokenKind.Equals);
            writer.WriteSpace();
            node.LowerBound.WriteTo(writer);
            writer.WriteSpace();
            writer.WriteKeyword(TokenKind.ToKeyword);
            writer.WriteSpace();
            node.UpperBound.WriteTo(writer);
            writer.WriteLine();
            writer.WriteNestedStatement(node.Body);
        }

        private static void WriteLabelStatement(BoundLabelStatement node, IndentedTextWriter writer)
        {
            var unindent = writer.Indent > 0;
            if (unindent)
                writer.Indent--;

            writer.WritePunctuation(node.Label.Name);
            writer.WritePunctuation(TokenKind.Colon);
            writer.WriteLine();

            if (unindent)
                writer.Indent++;
        }

        private static void WriteGotoStatement(BoundGotoStatement node, IndentedTextWriter writer)
        {
            writer.WriteKeyword("goto"); // There is no TokenKind for goto
            writer.WriteSpace();
            writer.WriteIdentifier(node.Label.Name);
            writer.WriteLine();
        }

        private static void WriteConditionalGotoStatement(BoundConditionalGotoStatement node, IndentedTextWriter writer)
        {
            writer.WriteKeyword("goto"); // There is no TokenKind for goto
            writer.WriteSpace();
            writer.WriteIdentifier(node.Label.Name);
            writer.WriteSpace();
            writer.WriteKeyword(node.JumpIfTrue ? "if" : "unless");
            writer.WriteSpace();
            node.Condition.WriteTo(writer);
            writer.WriteLine();
        }

        private static void WriteReturnStatement(BoundReturnStatement node, IndentedTextWriter writer)
        {
            writer.WriteKeyword(TokenKind.ReturnKeyword);
            if (node.Expression != null)
            {
                writer.WriteSpace();
                node.Expression.WriteTo(writer);
            }
            writer.WriteLine();
        }

        private static void WriteExpressionStatement(BoundExpressionStatement node, IndentedTextWriter writer)
        {
            node.Expression.WriteTo(writer);
            writer.WriteLine();
        }

        private static void WriteErrorExpression(BoundErrorExpression node, IndentedTextWriter writer)
        {
            writer.WriteKeyword("?");
        }

        private static void WriteLiteralExpression(BoundLiteralExpression node, IndentedTextWriter writer)
        {
            var value = node.Value.ToString();

            if (node.Type == TypeSymbol.Bool)
            {
                writer.WriteKeyword((bool)node.Value ? TokenKind.TrueKeyword : TokenKind.FalseKeyword);
            }
            else if (node.Type == TypeSymbol.Int)
            {
                writer.WriteNumber(value);
            }
            else if (node.Type == TypeSymbol.String)
            {
                value = "\"" + value.Replace("\"", "\"\"") + "\"";
                writer.WriteString(value);
            }
            else if (node.Type == TypeSymbol.Char)
            {
                value = "'" + value
                    .Replace("\0", "\\0")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t")
                            + "'";
                writer.WriteChar(value);
            }
            else
            {
                throw new Exception($"Unexpected type {node.Type}");
            }
        }

        private static void WriteVariableExpression(BoundVariableExpression node, IndentedTextWriter writer)
        {
            writer.WriteIdentifier(node.Variable.Name);
        }

        private static void WriteAssignmentExpression(BoundAssignmentExpression node, IndentedTextWriter writer)
        {
            node.Target.WriteTo(writer);
            writer.WriteSpace();
            writer.WritePunctuation(TokenKind.Equals);
            writer.WriteSpace();
            node.Expression.WriteTo(writer);
        }

        private static void WriteUnaryExpression(BoundUnaryExpression node, IndentedTextWriter writer)
        {
            var precedence = SyntaxFacts.GetUnaryOperatorPrecedence(node.Operator.TokenKind);

            writer.WritePunctuation(node.Operator.TokenKind);
            writer.WriteNestedExpression(precedence, node.Operand);
        }

        private static void WriteBinaryExpression(BoundBinaryExpression node, IndentedTextWriter writer)
        {
            var precedence = SyntaxFacts.GetBinaryOperatorPrecedence(node.Operator.TokenKind);

            writer.WriteNestedExpression(precedence, node.Left);
            writer.WriteSpace();
            writer.WritePunctuation(node.Operator.TokenKind);
            writer.WriteSpace();
            writer.WriteNestedExpression(precedence, node.Right);
        }

        private static void WriteCallExpression(BoundMethodCallExpression node, IndentedTextWriter writer)
        {
            var name = node.Method.Name;
            var arguments = node.Arguments;

            WriteCallExpression(name, arguments, writer);
        }

        private static void WriteCallExpression(BoundConstructorCallExpression node, IndentedTextWriter writer)
        {
            var name = node.Constructor.Name;
            var arguments = node.Arguments;

            WriteCallExpression(name, arguments, writer);
        }

        private static void WriteCallExpression(BoundFunctionCallExpression node, IndentedTextWriter writer)
        {
            var name = node.Function.Name;
            var arguments = node.Arguments;

            WriteCallExpression(name, arguments, writer);
        }

        private static void WriteCallExpression(string name, ImmutableArray<BoundExpression> arguments, IndentedTextWriter writer)
        {
            writer.WriteIdentifier(name);
            writer.WritePunctuation(TokenKind.OpenParenthesis);

            var isFirst = true;
            foreach (var argument in arguments)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    writer.WritePunctuation(TokenKind.Comma);
                    writer.WriteSpace();
                }

                argument.WriteTo(writer);
            }

            writer.WritePunctuation(TokenKind.CloseParenthesis);
        }

        private static void WriteConversionExpression(BoundConversionExpression node, IndentedTextWriter writer)
        {
            writer.WriteIdentifier(node.Type.Name);
            writer.WritePunctuation(TokenKind.OpenParenthesis);
            node.Expression.WriteTo(writer);
            writer.WritePunctuation(TokenKind.CloseParenthesis);
        }
    }
}