// -----------------------------------------------------------------------
// <copyright file="PmlRunner.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Compiler
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Formatting;
    using Microsoft.CodeAnalysis.Formatting;
    using Perspex.Markup.Pml.Dom;
    using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    public class PmlCompiler
    {
        public void Compile(Dom.Document document)
        {
            var root = document.RootNode;

            if (document.RootNode.Type.Parts.Length < 2)
            {
                throw new Exception("Root node must be namespace qualified.");
            }

            var unit = SF.CompilationUnit().AddMembers(
                            SF.NamespaceDeclaration(SF.ParseName(root.Type.NamespaceName)).AddMembers(
                                SF.ClassDeclaration(SF.ParseToken(root.Type.Name))
                                    .AddBaseListTypes(SF.SimpleBaseType(SF.ParseTypeName("Window")))));

            var workspace = new AdhocWorkspace();
            var options = workspace.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.SpaceAfterMethodCallName, true);

            var output = Formatter.Format(unit, workspace, options);
        }
    }
}
