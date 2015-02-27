// -----------------------------------------------------------------------
// <copyright file="PmlRunner.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Perspex.Markup.Pml.Dom;
    using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    public class PmlCompiler
    {
        public CompilationUnitSyntax Compile(Document document)
        {
            var root = document.RootNode;

            var unit = SF.CompilationUnit();

            unit = unit.AddMembers(
                this.Namespace(root).AddMembers(
                    this.Class(root).AddMembers(
                        this.InitializeComponentMethod()
                            .AddBodyStatements(this.RootPropertySetters(root).ToArray()))));

            return unit;
        }

        private NamespaceDeclarationSyntax Namespace(ObjectInstantiation o)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(o.Type.NamespaceName));
        }

        private ClassDeclarationSyntax Class(ObjectInstantiation o)
        {
            return SyntaxFactory.ClassDeclaration(SyntaxFactory.ParseToken(o.Type.Name))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddBaseListTypes(SF.SimpleBaseType(SF.ParseTypeName("Window")));
        }

        private MethodDeclarationSyntax InitializeComponentMethod()
        {
            return SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                "InitializeComponent");
        }

        private IEnumerable<StatementSyntax> RootPropertySetters(ObjectInstantiation o)
        {
            foreach (var child in o.Children)
            {
                var setter = child as PropertySetter;

                if (setter != null)
                {
                    yield return this.RootPropertySetter(setter);
                }
            }
        }

        private StatementSyntax RootPropertySetter(PropertySetter setter)
        {
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.ThisExpression(),
                        SyntaxFactory.IdentifierName(setter.PropertyName.Name)),
                    ((ExpressionValue)setter.Value).Statement.Expression));
        }
    }
}
