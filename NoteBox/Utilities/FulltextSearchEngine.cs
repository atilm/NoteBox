using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using NoteBox.Domain;

namespace NoteBox.Utilities
{
    public class FulltextSearchEngine
    {
        private const string FileNameId = "fileName";
        private const string ContentsId = "contents";
        private const LuceneVersion LuceneVersion = Lucene.Net.Util.LuceneVersion.LUCENE_48;

        private readonly string _baseDirectoryPath;
        private readonly string _indexPath;

        public FulltextSearchEngine()
        {
            _baseDirectoryPath = App.Configuration["CardsDirectoryPath"];
            _indexPath = Path.Combine(_baseDirectoryPath, "index");
        }

        public void RecreateIndex(IEnumerable<NoteFile> noteFiles)
        {
            using var indexWriter = CreateIndexWriter();
            
            indexWriter.DeleteAll();
            
            foreach (var noteFile in noteFiles)
            {
                var path = Path.Combine(_baseDirectoryPath, noteFile.FileName);
                var contents = File.ReadAllText(path);
                AddFile(indexWriter, noteFile, contents);
            }
        }
        
        public void AddFile(NoteFile noteFile, string contents)
        {
            using var indexWriter = CreateIndexWriter();

            AddFile(indexWriter, noteFile, contents);
        }

        private static void AddFile(IndexWriter indexWriter, NoteFile noteFile, string contents)
        {
            var document = CreateDocument(noteFile, contents);
            indexWriter.AddDocument(document);
            indexWriter.Flush(false, false);
        }

        public void UpdateFile(NoteFile noteFile, string contents)
        {
            using var indexWriter = CreateIndexWriter();

            var document = CreateDocument(noteFile, contents);
            var identifyingTerm = IdentifyingTerm(noteFile);

            indexWriter.UpdateDocument(identifyingTerm, document);
        }

        public void DeleteFile(NoteFile noteFile)
        {
            using var indexWriter = CreateIndexWriter();
            indexWriter.DeleteDocuments(IdentifyingTerm(noteFile));
        }

        private static Term IdentifyingTerm(NoteFile noteFile)
        {
            return new Term(FileNameId, noteFile.FileName);
        }

        private static Document CreateDocument(NoteFile noteFile, string contents)
        {
            return new Document
            {
                new StringField(FileNameId,
                    noteFile.FileName,
                    Field.Store.YES),
                new TextField(ContentsId,
                    contents,
                    Field.Store.NO)
            };
        }
        
        private IndexWriter CreateIndexWriter()
        {
            var dir = FSDirectory.Open(_indexPath);
            var analyzer = new StandardAnalyzer(LuceneVersion);
            var indexConfig = new IndexWriterConfig(LuceneVersion, analyzer);
            return new IndexWriter(dir, indexConfig);
        }

        public IEnumerable<NoteFile> Search(string searchPhrase)
        {
            var query = new MultiPhraseQuery();
            foreach (var word in searchPhrase.Split())
                query.Add(new Term(ContentsId, word));
            
            var dir = FSDirectory.Open(_indexPath);
            using var reader = DirectoryReader.Open(dir);
            var searcher = new IndexSearcher(reader);
            var hits = searcher.Search(query, 1000).ScoreDocs;

            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                var fileName = foundDoc.Get(FileNameId);
                yield return new NoteFile(fileName);
            }
        }
    }
}