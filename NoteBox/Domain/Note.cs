using System;
using System.Collections.Generic;

namespace NoteBox.Domain
{
    public class Note
    {
        public NoteIdentifier Id { get; set; } = new();
        public string Title { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
        public IList<HashTag> HashTags { get; set; } = new List<HashTag>();
        public IList<Reference> References { get; set; } = new List<Reference>();
    }
}
