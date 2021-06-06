using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using NoteBox.Domain;
using NoteBox.UI.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace NoteBox.UI.Controls
{
    public class NoteEditorViewModel : BindableBase
    {
        private readonly string _rawText;

        public NoteEditorViewModel(string content, NotesRepository repository)
        {
            _rawText = content;
            Repository = repository;
            NavigateCommand = new DelegateCommand<object>(NavigateTo);
        }

        private NotesRepository Repository { get; }

        public TextSelection Selection { get; set; }

        private ICommand NavigateCommand { get; }

        private FlowDocument Document { get; set; } = new();

        private void NavigateTo(object parameter)
        {
            if (parameter is not string linkText)
                return;

            var (success, id, _) = TextToFlowDocumentConverter.ParseLinkText(linkText);

            if (!success)
                return;

            var noteFile = NoteFile.FromId(id);

            var window = new NoteEditorWindow(noteFile, Repository);
            window.Show();
        }

        public string GetTitle()
        {
            return GetRawText().Split("\r\n").First();
        }

        public string GetRawText()
        {
            return new TextRange(Document.ContentStart, Document.ContentEnd).Text;
        }

        public void Init(FlowDocument document)
        {
            Document = document;

            Document.Blocks.Clear();
            Document.Blocks.AddRange(TextToFlowDocumentConverter.Convert(_rawText, NavigateCommand));
        }

        public void InsertLink()
        {
            var text = new TextRange(Selection.Start, Selection.End).Text;

            var noteFile = NoteFile.FromTitle(text);

            Selection.Start.DeleteTextInRun(text.Length);

            var linkText = TextToFlowDocumentConverter.BuildLinkString(text, noteFile);
            TextToFlowDocumentConverter.CreateHyperlink(linkText, NavigateCommand, Selection.Start);
        }
    }
}