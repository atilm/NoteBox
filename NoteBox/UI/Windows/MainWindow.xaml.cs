using System;
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
        
        private void HandleTagClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            var hashTag = item?.Content as HashTag ?? new HashTag(String.Empty, 0);
            m_viewModel.FilterByTag(hashTag);
        }

        private MainWindowViewModel m_viewModel;
    }
}