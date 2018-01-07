using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class OpenFileCommand : ICommand_mb
    {
        FileHandler _fileHandler;

        public OpenFileCommand(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public void Execute()
        {
            _fileHandler.ShowOpenFileDialog();
        }
    }
}
