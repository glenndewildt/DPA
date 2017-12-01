using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class SaveAslilyCommand : ICommand_mb
    {
        private FileHandler _fileHandler;
        private string _fileName;

        public SaveAslilyCommand(FileHandler handler, string fileName)
        {
            _fileHandler = handler;
            _fileName = fileName;
        }

        public void Execute()
        {
            _fileHandler.SaveToLilypond(_fileName);
        }
    }
}
