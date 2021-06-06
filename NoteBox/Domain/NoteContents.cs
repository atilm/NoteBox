using System;
using System.Collections.Generic;

namespace NoteBox.Domain
{
    public class NoteContents
    {
        public NoteContents(string id, string title, string text, IEnumerable<HashTag> hashTags, IEnumerable<Reference> references)
        {
            Id = id;
            Title = title;
            Text = text;
            HashTags = new List<HashTag>(hashTags);
            References = new List<Reference>(references);
        }

        public string Id { get; }
        public string Title { get; }
        public string Text { get; }
        public IList<HashTag> HashTags { get; }
        public IList<Reference> References { get; }
    }
}
