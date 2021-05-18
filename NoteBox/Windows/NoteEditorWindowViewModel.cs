using System.Windows.Input;
using NoteBox.Controls;
using Prism.Commands;
using Prism.Mvvm;

namespace NoteBox.Windows
{
    public class NoteEditorWindowViewModel : BindableBase
    {
        public NoteEditorWindowViewModel()
        {
            EditorViewModel = new NoteEditorViewModel();
            InsertLinkCommand = new DelegateCommand(InsertLink);
        }

        public NoteEditorViewModel EditorViewModel { get; set; }

        public ICommand InsertLinkCommand { get; }

        private void InsertLink()
        {
            EditorViewModel.InsertLink("Link title");
        }
    }
}