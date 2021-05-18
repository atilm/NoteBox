using NoteBox.Utilities;
using Prism.Commands;
using Prism.Mvvm;

namespace NoteBox.UI.Windows
{
    public class SnippetContainerViewModel : BindableBase
    {
        public SnippetContainerViewModel()
        {
            CopyTimeStampToClipboardCommand = new DelegateCommand(() =>
                TimeStampGenerator.GenerateTimeStamp().CopyToClipboard());
        }

        public DelegateCommand CopyTimeStampToClipboardCommand { get; }
    }
}