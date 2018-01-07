using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class FileOpenerFactory
    {
        private FileHandler _handler;
        private Dictionary<string, AbstractFileOpener> openers = new Dictionary<string, AbstractFileOpener>();

        public FileOpenerFactory(FileHandler handler)
        {
            _handler = handler;
            InitializeOpeners();
        }

        private void InitializeOpeners()
        {
            openers.Add(".ly", new LilyOpener(_handler));
            openers.Add(".mid", new MidiOpener(_handler));
        }

        public AbstractFileOpener GetOpener(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            if (openers.ContainsKey(extension))
            {
                return openers[extension];
            } else
            {
                throw new Exception("Extension `" + Path.GetExtension(fileName) + "` is not supported");
            }
        }
    }
}
