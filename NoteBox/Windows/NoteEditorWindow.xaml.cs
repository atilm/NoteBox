using System.Windows;
using NoteBox.Controls;

namespace NoteBox.Windows
{
    public partial class NoteEditorWindow : Window
    {
        public NoteEditorWindow()
        {
            InitializeComponent();

            var viewModel = new NoteEditorWindowViewModel();
            DataContext = viewModel;
        }
    }
}