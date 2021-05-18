using System.Windows;
using NoteBox.Domain;

namespace NoteBox.UI.Windows
{
    public partial class NoteEditorWindow : Window
    {
        public NoteEditorWindow(NoteFile noteFile, NotesRepository repository)
        {
            InitializeComponent();

            var viewModel = new NoteEditorWindowViewModel(noteFile, repository);
            DataContext = viewModel;
        }
    }
}