// -----------------------------------------------------------------------
// <copyright file="Expressionvalue.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Dom
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class ExpressionValue : PropertyValue
    {
        public ExpressionValue(ExpressionStatementSyntax expression)
        {
            this.Statement = expression;
        }

        public ExpressionStatementSyntax Statement { get; }
    }
}