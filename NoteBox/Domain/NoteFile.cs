using System;
using System.IO;
using System.Text.RegularExpressions;
using NoteBox.Utilities;

namespace NoteBox.Domain
{
    public class NoteFile : IEquatable<NoteFile>
    {
        private readonly Regex _fileNameRegex = new(@"^(\d+)\s(.+)", RegexOptions.Compiled);

        public NoteFile()
        {
            Id = TimeStampGenerator.GenerateTimeStamp();
        }

        public NoteFile(string filePath) : this()
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            SetFileNameWithoutExtension(fileName);
        }

        public string Id { get; private set; }

        public string Name { get; set; } = String.Empty;

        public string FileName
        {
            get { return $"{Id} {Name}.txt"; }
        }

        public bool Equals(NoteFile? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return String.Equals(Id, other.Id, StringComparison.InvariantCulture) &&
                   String.Equals(Name, other.Name, StringComparison.InvariantCulture);
        }

        public static NoteFile FromIdAndTitle(string id, string title)
        {
            var noteFile = new NoteFile {Id = id.Trim(), Name = title.Trim()};
            return noteFile;
        }

        public static NoteFile FromId(string id)
        {
            return FromIdAndTitle(id, String.Empty);
        }

        public static NoteFile FromTitle(string title)
        {
            return FromIdAndTitle(String.Empty, title);
        }

        public void SetFileNameWithoutExtension(string fileName)
        {
            var match = _fileNameRegex.Match(fileName);

            if (!match.Success)
                throw new InvalidDataException($"File name {fileName} does not match expected pattern.");

            Id = match.Groups[1].Value;
            Name = match.Groups[2].Value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NoteFile) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id, StringComparer.InvariantCulture);
            hashCode.Add(Name, StringComparer.InvariantCulture);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(NoteFile? left, NoteFile? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NoteFile? left, NoteFile? right)
        {
            return !Equals(left, right);
        }
    }
}