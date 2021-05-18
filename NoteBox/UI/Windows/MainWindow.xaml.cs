using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoteBox.Domain;

namespace NoteBox.UI.Windows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = m_viewModel = viewModel;
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var boxItem = sender as ListBoxItem;
            var noteFile = boxItem?.Content as NoteFile ?? new NullNoteFile();
            m_viewModel.OpenNote(noteFile);
        }

        private MainWindowViewModel m_viewModel;
    }
}