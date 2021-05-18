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

        private static string DirectoryPath
        {
            get { return App.Configuration["CardsDirectoryPath"]; }
        }

        private FulltextSearchEngine SearchEngine { get; }

        public event EventHandler? FilesChanged;

        public IEnumerable<NoteFile> ListAllFiles()
        {
            return Directory.GetFiles(DirectoryPath)
                .Select(s => new NoteFile(s))
                .OrderByDescending(n => n.Id);
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

        public string GetContent(NoteFile noteFile)
        {
            var fileById = ListAllFiles().First(f => f.Id == noteFile.Id);
            var filePath = Path.Join(DirectoryPath, fileById.FileName);
            return File.ReadAllText(filePath);
        }

        public void Save(NoteFile noteFile, string[] rawTextLines)
        {
            UpdateSearchIndex(noteFile, rawTextLines);

            var preExistingFilesWithSameId = ListAllFiles()
                .Where(f => f.Id == noteFile.Id && f.FileName != noteFile.FileName);

            File.WriteAllLines(FullPath(noteFile), rawTextLines);

            foreach (var path in preExistingFilesWithSameId.Select(FullPath))
                File.Delete(path);

            FilesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateSearchIndex(NoteFile noteFile, string[] rawTextLines)
        {
            var text = String.Join("", rawTextLines);

            if (ListAllFiles().Contains(noteFile))
                SearchEngine.UpdateFile(noteFile, text);
            else
                SearchEngine.AddFile(noteFile, text);
        }

        private static string FullPath(NoteFile noteFile)
        {
            return Path.Join(DirectoryPath, noteFile.FileName);
        }
    }
}