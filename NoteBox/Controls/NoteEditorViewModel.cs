using System;
using System.Windows.Documents;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace NoteBox.Controls
{
    public class NoteEditorViewModel : BindableBase
    {
        private FlowDocument Document { get; set; }

        public NoteEditorViewModel()
        {
            NavigateCommand = new DelegateCommand<object>(NavigateTo);
        }

        private void NavigateTo(object parameter)
        {
        }

        public void Init(FlowDocument document)
        {
            Document = document;

            Document.Blocks.Clear();
            Document.Blocks.Add(GenerateTitle());
        }

        public void InsertLink(string linkText)
        {
            Document.Blocks.Add(Hyperlink($"[{TimeStampGenerator.GenerateTimeStamp()}|{linkText}]"));
        }
        
        private static Paragraph GenerateTitle()
        {
            return CreateParagraph($"{TimeStampGenerator.GenerateTimeStamp()} Title");
        }

        private static Paragraph CreateParagraph(string s)
        {
            return new(new Run(s));
        }

        private readonly ICommand NavigateCommand;

        private Paragraph Hyperlink(string s)
        {
            var hyperlink = new Hyperlink(new Run(s));
            hyperlink.IsEnabled = true;
            hyperlink.NavigateUri = new Uri("https://www.codeproject.com/Articles/1166122/WPF-RichTextBox-containing-Hyperlinks");
            hyperlink.RequestNavigate += (sender, args) => { };
            hyperlink.Command = NavigateCommand;
            hyperlink.CommandParameter = s;
            return new Paragraph(hyperlink);
        }
    }
}