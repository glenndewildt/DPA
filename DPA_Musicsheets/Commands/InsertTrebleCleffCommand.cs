using DPA_Musicsheets.LilyPond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class InsertTrebleCleffCommand : ICommand_mb
    {
        private string text = @"\clef treble";
        private LilypondEditor _lilyEditor;

        public InsertTrebleCleffCommand(LilypondEditor lilyEditor)
        {
            _lilyEditor = lilyEditor;
        }

        public void Execute()
        {
            _lilyEditor.InsertTextAfterCursor(text);
            _lilyEditor.AddBookmark(_lilyEditor.SaveStateToMemento());
        }
    }
}
