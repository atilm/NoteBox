using System.Windows.Input;
using NoteBox.Domain;
using NoteBox.UI.Controls;
using Prism.Commands;
using Prism.Mvvm;

namespace NoteBox.UI.Windows
{
    public class NoteEditorWindowViewModel : BindableBase
    {
        private readonly NoteFile _noteFile;

        public NoteEditorWindowViewModel(NoteFile noteFile, NotesRepository repository)
        {
            _noteFile = noteFile;
            Repository = repository;
            var content = LoadFileContent(noteFile);
            EditorViewModel = new NoteEditorViewModel(content, Repository);
            SaveCommand = new DelegateCommand(SaveFileContent);
            InsertLinkCommand = new DelegateCommand(EditorViewModel.InsertLink);
        }

        public NoteEditorViewModel EditorViewModel { get; }
        public ICommand SaveCommand { get; }
        public ICommand InsertLinkCommand { get; }
        private NotesRepository Repository { get; }

        private string LoadFileContent(NoteFile noteFile)
        {
            return Repository.Contains(noteFile) ? Repository.GetContent(noteFile) : $"{noteFile.Id} {noteFile.Name}";
        }

        private void SaveFileContent()
        {
            _noteFile.SetFileNameWithoutExtension(EditorViewModel.GetTitle());
            Repository.Save(_noteFile, EditorViewModel.GetRawTextLines());
        }
    }
}