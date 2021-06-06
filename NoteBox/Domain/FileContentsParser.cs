using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoteBox.Domain
{
    public static class FileContentsParser
    {
        public static NoteContents Parse(string rawTextLines)
        {
            var (id, title) = ExtractIdAndTitle(rawTextLines);
            var references = Extract(ReferenceRegex, rawTextLines).Select(s => new Reference(s));
            var hashTags = Extract(HashTagRegex, rawTextLines).Select(s => new HashTag(s, 1));

            return new NoteContents(id, title, rawTextLines, hashTags, references);
        }

        private static (string id, string Title) ExtractIdAndTitle(string rawText)
        {
            var firstLine = rawText.Replace("\r\n", "\n").Split("\n").First();

            var match = FirstLineRegex.Match(firstLine);

            return !match.Success ? (String.Empty, String.Empty) : (match.Groups[1].Value, match.Groups[2].Value);
        }

        private static IEnumerable<string> Extract(Regex regex, string rawText)
        {
            return regex.Matches(rawText).Select(m => m.Value);
        }

        private static readonly Regex FirstLineRegex = new Regex(@"(\d{12})(.*)$", RegexOptions.Compiled);
        private static readonly Regex ReferenceRegex = new(@"\(\(.+\)\)", RegexOptions.Compiled);
        private static readonly Regex HashTagRegex = new(@"#\w+", RegexOptions.Compiled);
    }
}