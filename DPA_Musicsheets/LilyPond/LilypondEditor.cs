using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Keycommands;
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
        private HotkeyChainOfResponsibility _hotkeyChain;

        public LilypondEditor(LilypondViewModel lilypondViewModel, HotkeyChainOfResponsibility hotkeyChain)
        {
            _lilypondViewModel = lilypondViewModel;
            _hotkeyChain = hotkeyChain;

            InitializeEditorHotkeys();
        }

        private void InitializeEditorHotkeys()
        {
            ICommand_mb cmd1 = new InsertTime4_4Command(this);
            HotkeyHandler h1 = new InsertTime4_4Handler(cmd1);
            _hotkeyChain.AppendHandler(h1);           
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
