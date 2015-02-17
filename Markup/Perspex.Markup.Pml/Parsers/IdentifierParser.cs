// -----------------------------------------------------------------------
// <copyright file="IdentifierParser.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Parsers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Perspex.Markup.Pml.Dom;
    using Sprache;

    public class IdentifierParser
    {
        public static readonly Parser<char> CombiningCharacter = Parse.Char(
            c =>
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(c);
                return cat == UnicodeCategory.NonSpacingMark || 
                       cat == UnicodeCategory.SpacingCombiningMark;
            },
            "Connecting Character");

        public static readonly Parser<char> ConnectingCharacter = Parse.Char(
            c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.ConnectorPunctuation,
            "Connecting Character");

        public static readonly Parser<char> FormattingCharacter = Parse.Char(
            c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.Format,
            "Connecting Character");

        public static readonly Parser<char> IdentifierStart = Parse.Letter.Or(Parse.Char('_'));

        public static readonly Parser<char> IdentifierPart = Parse
            .LetterOrDigit
            .Or(ConnectingCharacter)
            .Or(CombiningCharacter)
            .Or(FormattingCharacter);

        public static readonly Parser<Identifier> Identifier =
            from start in IdentifierStart.Once().Text()
            from part in IdentifierPart.Many().Text()
            select new Identifier(start + part);

        public static readonly Parser<Identifier> DottedIdentifier =
            from dot in Parse.Char('.')
            from identifier in IdentifierParser.Identifier
            select identifier;

        public static readonly Parser<Identifier> NamespacedIdentifier =
            from identifier in IdentifierParser.Identifier.Once()
            from dotted in DottedIdentifier.Many()
            select new Identifier(identifier.Concat(dotted));
    }
}
