using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NoteBox.Controls
{
    public partial class NoteEditor : UserControl
    {
        public NoteEditor()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NoteEditorViewModel;
            viewModel!.Init(textBox.Document);
        }

        private NoteEditorViewModel _viewModel;
    }
}