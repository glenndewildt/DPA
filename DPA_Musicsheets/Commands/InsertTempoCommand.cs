using DPA_Musicsheets.LilyPond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class InsertTempoCommand : ICommand_mb
    {
        private string text = @"\tempo 4=120";
        private LilypondEditor _lilyEditor;

        public InsertTempoCommand(LilypondEditor lilyEditor)
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
