using System;

namespace NoteBox
{
    public static class TimeStampGenerator
    {
        public static string GenerateTimeStamp()
        {
            return DateTime.Now.ToString(App.Configuration["TimeSnippetFormat"]);
        }
    }
}