// -----------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Parsers
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Sprache;

    public class ExpressionParser
    {
        public static Parser<ExpressionStatementSyntax> Expression()
        {
            return input =>
            {
                if (!input.AtEnd)
                {
                    var expression = ParseExpression(input);

                    if (expression != null)
                    {
                        var remainder = input;

                        for (int c = 0; c < expression.Span.Length; ++c)
                        {
                            remainder = remainder.Advance();
                        }

                        return Result.Success(expression, remainder);
                    }
                }

                return Result.Failure<ExpressionStatementSyntax>(input, "Unexpected end of input", new[] { "Expression" });
            };
        }

        private static ExpressionStatementSyntax ParseExpression(IInput input)
        {
            var text = input.Source.Substring(input.Position);
            var options = new CSharpParseOptions(kind: SourceCodeKind.Interactive);
            var compilation = CSharpSyntaxTree.ParseText(text, options).GetRoot();
            var global = compilation?.DescendantNodes().FirstOrDefault() as GlobalStatementSyntax;
            var expression = global?.DescendantNodes().FirstOrDefault() as ExpressionStatementSyntax;
            return expression;
        }
    }
}
