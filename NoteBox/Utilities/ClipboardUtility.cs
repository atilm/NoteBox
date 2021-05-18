using System.Windows;

namespace NoteBox.Utilities
{
    public static class ClipboardUtility
    {
        public static void CopyToClipboard(string s)
        {
            Clipboard.SetText(s);
        }
    }
}