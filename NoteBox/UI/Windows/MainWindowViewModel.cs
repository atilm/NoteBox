using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NoteBox.Domain;
using Prism.Commands;
using Prism.Mvvm;

namespace NoteBox.UI.Windows
{
    public class MainWindowViewModel : BindableBase
    {
        private IList<NoteFile> _notes = new List<NoteFile>();
        private IList<HashTag> _hashTags = new List<HashTag>();
        private string _searchPhrase = String.Empty;
        private readonly SnippetContainerViewModel _snippetContainerViewModel;
        private readonly NotesRepository _notesRepository;

        public MainWindowViewModel(
            SnippetContainerViewModel snippetViewModel,
            NotesRepository notesRepository)
        {
            _snippetContainerViewModel = snippetViewModel;
            _notesRepository = notesRepository;

            NewNoteCommand = new DelegateCommand(NewNote);
            CreateIndexCommand = new DelegateCommand(_notesRepository.RecreateSearchIndex);

            NotesRepository.FilesChanged += (sender, args) => LoadFiles();
            LoadFiles();
        }

        public IList<NoteFile> Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public IList<HashTag> HashTags
        {
            get { return _hashTags; }
            set { SetProperty(ref _hashTags, value); }
        }

        public string SearchPhrase
        {
            get { return _searchPhrase; }
            set
            {
                SetProperty(ref _searchPhrase, value);
                LoadFilteredFiles(_searchPhrase);
            }
        }

        public SnippetContainerViewModel SnippetContainerViewModel
        {
            get { return _snippetContainerViewModel; }
        }

        public NotesRepository NotesRepository
        {
            get { return _notesRepository; }
        }

        public ICommand CreateIndexCommand { get; }

        public ICommand NewNoteCommand { get; }

        public void OpenNote(NoteFile noteFile)
        {
            OpenNoteFile(noteFile);
        }

        private void LoadFiles()
        {
            Notes = new ObservableCollection<NoteFile>(
                _notesRepository.ListAllFiles());
            HashTags = new ObservableCollection<HashTag>(
                _notesRepository.ListAllTags());
        }
        
        private void LoadFilteredFiles(string searchPhrase)
        {
            var filteredFiles = searchPhrase.Trim() == String.Empty ?
                _notesRepository.ListAllFiles() :
                _notesRepository.FilterFiles(searchPhrase);
            
            Notes = new ObservableCollection<NoteFile>(filteredFiles);
        }

        private void NewNote()
        {
            OpenNoteFile(new NoteFile());
        }

        private void OpenNoteFile(NoteFile noteFile)
        {
            if (noteFile is NullNoteFile)
                return;
            
            var window = new NoteEditorWindow(noteFile, NotesRepository);
            window.Show();
        }

        public void FilterByTag(HashTag hashTag)
        {
            if (hashTag.Tag == String.Empty)
                return;
            
            SearchPhrase = hashTag.Tag.ToLowerInvariant();
        }
    }
}