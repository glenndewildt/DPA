using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class LilyOpener : AbstractFileOpener
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
