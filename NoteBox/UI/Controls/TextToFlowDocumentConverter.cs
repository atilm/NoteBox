using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Input;
using NoteBox.Domain;

namespace NoteBox.UI.Controls
{
    public static class TextToFlowDocumentConverter
    {
        private static readonly Regex LinkRegex = new(@"\(\s*(\d{12})\s*\|(.*)\)", RegexOptions.Compiled);
        private static readonly Regex HashTagRegex = new(@"#\w+", RegexOptions.Compiled);

        public static IEnumerable<Block> Convert(string text, ICommand navigateCommand)
        {
            var lines = Normalize(text).Split("\n");

            return lines.Select(l => ToParagraph(l, navigateCommand));
        }

        public static string BuildLinkString(string text, NoteFile noteFile)
        {
            return $"({noteFile.Id} | {text.Trim()})";
        }

        public static (bool success, string id, string name) ParseLinkText(string linkText)
        {
            var match = LinkRegex.Match(linkText);

            return !match.Success
                ? (false, String.Empty, String.Empty)
                : (true, match.Groups[1].Value, match.Groups[2].Value);
        }

        private static string Normalize(string line)
        {
            return line.Replace("\r\n", "\n");
        }

        private static Paragraph ToParagraph(string line, ICommand navigateCommand)
        {
            var linkMatches = LinkRegex.Matches(line);
            var hashTagMatches = HashTagRegex.Matches(line);

            var mergedMatches = linkMatches.Concat(hashTagMatches).OrderBy(m => m.Index).ToList();

            var paragraph = new Paragraph();

            var currentIndex = 0;
            foreach (var match in mergedMatches)
            {
                var run = line.Substring(currentIndex, match.Index - currentIndex);
                paragraph.Inlines.Add(new Run(run));

                var hyperlinkText = line.Substring(match.Index, match.Length);
                paragraph.Inlines.Add(CreateHyperlink(hyperlinkText, navigateCommand));
                currentIndex = match.Index + match.Length;
            }

            paragraph.Inlines.Add(new Run(line.Substring(currentIndex)));

            return paragraph;
        }

        public static Hyperlink CreateHyperlink(string s, ICommand navigateCommand, TextPointer? insertionPoint = null)
        {
            var hyperlink = new Hyperlink(new Run(s), insertionPoint)
            {
                IsEnabled = true,
                Command = navigateCommand,
                CommandParameter = s
            };
            return hyperlink;
        }
    }
}