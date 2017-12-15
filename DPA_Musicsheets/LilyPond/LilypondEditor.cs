using DPA_Musicsheets.Commands;
using DPA_Musicsheets.ViewModels;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.LilyPond
{
    class LilypondEditor
    {
        private LilypondViewModel _lilypondViewModel;

        public LilypondEditor(LilypondViewModel lilypondViewModel)
        {
            _lilypondViewModel = lilypondViewModel;
        }

        public void InitializeEditorHotkeys()
        {
            InsertTime4_4Command timeCommand = new InsertTime4_4Command(this);

           
        }

        private List<string> SplitByNewlines(string text)
        {
            List<string> lines = new List<string>();

            string[] _arrayLines = text.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            foreach (var line in _arrayLines) {
                lines.Add(line);
            }

            return lines;
        }

        private string JoinByNewlines(List<string> lines)
        {
            return String.Join(Environment.NewLine, lines);
        }

        public void InsertTextBeforeLine(int i, string text)
        {
            string editorTxtBefore = _lilypondViewModel.LilypondText;

            List<string> lines = SplitByNewlines(text);
            // insert the new text
            lines.Insert(i, text);

            _lilypondViewModel.LilypondText = JoinByNewlines(lines);
        }

        public void InsertTextAfterCursor(string text)
        {
            //_lilyPondTextEditorXAML.Text.Insert(_lilyPondTextEditorXAML.CaretIndex, text);
        }
    }
}
