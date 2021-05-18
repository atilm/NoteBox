using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace NoteBox.Windows
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel(SnippetContainerViewModel snippetViewModel)
        {
            SnippetContainerViewModel = snippetViewModel;
            NewNoteCommand = new DelegateCommand(NewNote);
        }

        public SnippetContainerViewModel SnippetContainerViewModel { get; }

        public ICommand NewNoteCommand { get; }

        private static void NewNote()
        {
            var window = new NoteEditorWindow();
            window.Show();
        }
    }
}