using System;

namespace DPA_Musicsheets.LilyPond
{
    public class LilypondEditorMemento
    {
        public string text;
        public DateTime CreationTime;
        public int id;

        public static int GlobalId = 0;

        public LilypondEditorMemento(string text)
        {
            this.CreationTime = DateTime.Now;
            this.text = text;
            this.id = LilypondEditorMemento.GlobalId;

            LilypondEditorMemento.GlobalId += 1;
        }
    }
}