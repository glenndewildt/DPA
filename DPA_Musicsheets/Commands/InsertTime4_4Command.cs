using DPA_Musicsheets.LilyPond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class InsertTime4_4Command : ICommand_mb
    {
        private string text = "time = 4 / 4";
        private LilypondEditor _lilyEditor;

        public InsertTime4_4Command(LilypondEditor lilyEditor)
        {
            _lilyEditor = lilyEditor;
        }

        public void Execute()
        {
            _lilyEditor.InsertTextAfterCursor(text);
        }
    }
}
