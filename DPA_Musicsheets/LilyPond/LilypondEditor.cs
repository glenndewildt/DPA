using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Keycommands;
using DPA_Musicsheets.Lilypond;
using DPA_Musicsheets.Managers;
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
        private HotkeyChainOfResponsibility _hotkeyChain;

        private string text;
        private TextBox _textBox;

        private FileHandler _fileHandler;

        private LilypondEditorBookmarks _bookmarks = new LilypondEditorBookmarks();

        public LilypondEditor(HotkeyChainOfResponsibility hotkeyChain, TextBox textBox, FileHandler fileHandler)
        {
            _hotkeyChain = hotkeyChain;
            _fileHandler = fileHandler;
            _textBox = textBox;

            _bookmarks.AddBookmark(new LilypondEditorMemento(""));

            InitializeEditorHotkeys();
        }

        public string GetText()
        {
            return text;
        }

        public void SetText(string text)
        {
            this.text = text;
            _textBox.Text = text;
        }

        internal string GetSelectedText()
        {
            return _textBox.SelectedText;
        }

        private void InitializeEditorHotkeys()
        {
            ICommand_mb cmd1 = new InsertTime4_4Command(this);
            HotkeyHandler h1 = new InsertTime4_4Handler(cmd1);
            _hotkeyChain.AppendHandler(h1);

            ICommand_mb cmd2 = new InsertTrebleCleffCommand(this);
            HotkeyHandler h2 = new InsertTrebleCleffHandler(cmd2);
            _hotkeyChain.AppendHandler(h2);

            // don't really need to make a new command, can reuse cmd1
            ICommand_mb cmd3 = new InsertTime4_4Command(this);
            HotkeyHandler h3 = new InsertTime4_4Handler2(cmd3);
            _hotkeyChain.AppendHandler(h3);

            ICommand_mb cmd4 = new InsertTime3_4Command(this);
            HotkeyHandler h4 = new InsertTime3_4Handler(cmd4);
            _hotkeyChain.AppendHandler(h4);

            ICommand_mb cmd5 = new InsertTime6_8Command(this);
            HotkeyHandler h5 = new InsertTime6_8Handler(cmd5);
            _hotkeyChain.AppendHandler(h5);

            ICommand_mb cmd6 = new InsertTempoCommand(this);
            HotkeyHandler h6 = new InsertTempoHandler(cmd6);
            _hotkeyChain.AppendHandler(h6);

            ICommand_mb cmd7 = new InsertBarsWhereMissingCommand(this);
            HotkeyHandler h7 = new InsertBarsWhereMissingHandler(cmd7);
            _hotkeyChain.AppendHandler(h7);

            ICommand_mb cmd8 = new SaveAsLilyCommand(_fileHandler, this);
            HotkeyHandler h8 = new SaveAsLilyHandler(cmd8);
            _hotkeyChain.AppendHandler(h8);

            ICommand_mb cmd9 = new SaveAsPDFCommand(_fileHandler, this);
            HotkeyHandler h9 = new SaveAsPDFHandler(cmd9);
            _hotkeyChain.AppendHandler(h9);

            ICommand_mb cmd10 = new OpenFileCommand(_fileHandler);
            HotkeyHandler h10 = new OpenFileHandler(cmd10);
            _hotkeyChain.AppendHandler(h10);
        }

        private List<string> SplitByNewlines(string text)
        {
            List<string> lines = new List<string>();

            string[] _arrayLines = text.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            foreach (var line in _arrayLines)
            {
                lines.Add(line);
            }

            return lines;
        }

        public void ClearBookmarkHistory()
        {
            _bookmarks.Clear();
        }

        private string JoinByNewlines(List<string> lines)
        {
            return String.Join(Environment.NewLine, lines);
        }

        public void InsertTextAfterCursor(string text)
        {
            int beforeIndex = _textBox.SelectionStart;
            // textBox.Text = textBox.Text.Insert(beforeIndex, text);
            SetText(_textBox.Text.Insert(beforeIndex, text));
            _textBox.SelectionStart = beforeIndex;
        }

        public void SetSelectedText(string newText)
        {
            _textBox.SelectedText = newText;
        }

        internal void debugBookmarks()
        {
            Console.Out.WriteLine(_bookmarks.ToString());
        }

        #region Memento pattern
        public LilypondEditorMemento SaveStateToMemento()
        {
            return new LilypondEditorMemento(text);
        }

        public void LoadStateFromMemento(LilypondEditorMemento memento)
        {
            text = memento.text;
            _textBox.Text = text;
        }

        public void AddBookmark(LilypondEditorMemento memento)
        {
            _bookmarks.AddBookmark(memento);
        }

        public void Undo()
        {
            LoadStateFromMemento(_bookmarks.GoBackOne());
        }

        public bool CanUndo()
        {
            return _bookmarks.CanUndo();
        }

        public void Redo()
        {
            LoadStateFromMemento(_bookmarks.GoForwardOne());
        }

        public bool CanRedo()
        {
            return _bookmarks.CanRedo();
        }
        #endregion
    }
}
