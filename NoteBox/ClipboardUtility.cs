using System.Windows;

namespace NoteBox
{
    public static class ClipboardUtility
    {
        public static void CopyToClipboard(string s)
        {
            Clipboard.SetText(s);
        }
    }
}