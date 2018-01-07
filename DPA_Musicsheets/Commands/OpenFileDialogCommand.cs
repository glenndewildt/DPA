using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    public class OpenFileDialogCommand : ICommand_mb
    {
        private FileHandler _fileHandler;

        public OpenFileDialogCommand(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public void Execute()
        {
            _fileHandler.ShowOpenFileDialog();
        }
    }
}
