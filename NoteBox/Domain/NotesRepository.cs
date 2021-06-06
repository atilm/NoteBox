using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoteBox.Utilities;

namespace NoteBox.Domain
{
    public class NotesRepository
    {
        public NotesRepository(FulltextSearchEngine searchEngine)
        {
            SearchEngine = searchEngine;
        }

        public event EventHandler? FilesChanged;

        public IEnumerable<NoteFile> ListAllFiles()
        {
            return Directory.GetFiles(DirectoryPath)
                .Select(s => new NoteFile(s))
                .OrderByDescending(n => n.Id);
        }

        public IEnumerable<HashTag> ListAllTags()
        {
            return SearchEngine.StoredTags().OrderByDescending(t => t.Frequency);
        }

        public void RecreateSearchIndex()
        {
            SearchEngine.RecreateIndex(ListAllFiles());
        }

        public IEnumerable<NoteFile> FilterFiles(string searchPhrase)
        {
            return SearchEngine.Search(searchPhrase);
        }

        public bool Contains(NoteFile noteFile)
        {
            return ListAllFiles().Any(f => f.Id == noteFile.Id);
        }

        public NoteContents GetContents(NoteFile noteFile)
        {
            var fileById = ListAllFiles().First(f => f.Id == noteFile.Id);
            var filePath = Path.Join(DirectoryPath, fileById.FileName);
            var text = File.ReadAllText(filePath);

            return FileContentsParser.Parse(text);
        }

        public void Save(NoteFile noteFile, NoteContents contents)
        {
            UpdateSearchIndex(noteFile, contents);

            var preExistingFilesWithSameId = ListAllFiles()
                .Where(f => f.Id == noteFile.Id && f.FileName != noteFile.FileName);

            File.WriteAllText(FullPath(noteFile), contents.Text);

            foreach (var path in preExistingFilesWithSameId.Select(FullPath))
                File.Delete(path);

            FilesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateSearchIndex(NoteFile noteFile, NoteContents contents)
        {
            if (ListAllFiles().Contains(noteFile))
                SearchEngine.UpdateFile(noteFile, contents);
            else
                SearchEngine.AddFile(noteFile, contents);
        }

        private static string FullPath(NoteFile noteFile)
        {
            return Path.Join(DirectoryPath, noteFile.FileName);
        }
        
        private static string DirectoryPath
        {
            get { return App.Configuration["CardsDirectoryPath"]; }
        }

        private FulltextSearchEngine SearchEngine { get; }
    }
}