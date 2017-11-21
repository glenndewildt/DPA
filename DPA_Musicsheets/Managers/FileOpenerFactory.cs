using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public partial class FileOpenerFactory
    {
        private FileHandler _handler;

        public FileOpenerFactory(FileHandler handler)
        {
            _handler = handler;
        }

        public abstract class FileOpener
        {
            public abstract void Open(string filename);
        }

        public FileOpener GetOpener(string fileName)
        {
            FileOpener opener;
            string extension = Path.GetExtension(fileName);

            if (extension.EndsWith(".mid"))
            {
                opener = new MidiOpener(_handler);
            }
            else if (extension.EndsWith(".ly"))
            {
                opener = new LilyOpener(_handler);
            }
            else
            {
                throw new Exception("Extension `" + Path.GetExtension(fileName) + "` is not supported");
            }

            return opener;
        }

        public class LilyOpener : FileOpener
        {
            private FileHandler _handler;

            public LilyOpener(FileHandler handler)
            {
                _handler = handler;
            }

            public override void Open(string fileName)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var line in File.ReadAllLines(fileName))
                {
                    sb.AppendLine(line);
                }

                _handler.LilypondText = sb.ToString();

                _handler.LoadLilypond(sb.ToString());
            }
        }
    }
}
