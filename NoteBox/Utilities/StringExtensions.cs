namespace NoteBox.Utilities
{
    public static class StringExtensions
    {
        public static void CopyToClipboard(this string s)
        {
            ClipboardUtility.CopyToClipboard(s);
        }
    }
}