namespace NoteBox
{
    public static class StringExtensions
    {
        public static void CopyToClipboard(this string s)
        {
            ClipboardUtility.CopyToClipboard(s);
        }
    }
}