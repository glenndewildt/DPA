using DPA_Musicsheets.LilyPond;
using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DPA_Musicsheets.Commands
{
    class SaveAsPDFCommand : ICommand_mb
    {
        private FileHandler _fileHandler;
        private LilypondEditor _lilyEditor;

        public SaveAsPDFCommand(FileHandler handler, LilypondEditor lilyEditor)
        {
            _fileHandler = handler;
            _lilyEditor = lilyEditor;
        }

        public void Execute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "PDF files|*.pdf" };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _fileHandler.SaveToPDF(saveFileDialog.FileName, _lilyEditor.GetText());
            }
        }
    }
}
