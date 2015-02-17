// -----------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Parsers
{
    using Perspex.Markup.Pml.Dom;
    using Sprache;

    public class ExpressionParser
    {
        public static readonly Parser<Literal> BooleanLiteral = Parse
            .String("true").Return(new Literal(true))
            .Or(Parse.String("false").Return(new Literal(false)));

        public static readonly Parser<Literal> NumericLiteral =
            from integer in Parse.Digit.Many().Text()
            from point in Parse.Char('.').Optional()
            from fraction in Parse.Digit.Many().Text().Optional()
            select new Literal(double.Parse(integer + point.GetOrDefault() + fraction.GetOrDefault()));

        public static readonly Parser<Literal> StringLiteral =
            from open in Parse.Char('"')
            from text in Parse.AnyChar.Except(Parse.Char('"')).Many().Text()
            from close in Parse.Char('"')
            select new Literal(text);

        public static readonly Parser<Literal> Literal =
            BooleanLiteral
            .Or(StringLiteral)
            .Or(NumericLiteral);

        public static readonly Parser<Expression> Expression = 
            Literal
            .Or<Expression>(IdentifierParser.Identifier);
    }
}
