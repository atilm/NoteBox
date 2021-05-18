using System.Windows;
using System.Windows.Controls;

namespace NoteBox.UI.Controls
{
    public partial class NoteEditor : UserControl
    {
        private NoteEditorViewModel _viewModel = null!;

        public NoteEditor()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as NoteEditorViewModel;
            _viewModel!.Init(textBox.Document);
        }

        private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var richTextBox = sender as RichTextBox;
            _viewModel.Selection = richTextBox.Selection;
        }
    }
}