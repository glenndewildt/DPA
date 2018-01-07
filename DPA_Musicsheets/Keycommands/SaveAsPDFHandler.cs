using DPA_Musicsheets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Keycommands
{
    class SaveAsPDFHandler : HotkeyHandler
    {
        private KeySequence _pattern = KeysequenceDefinitions.CTRL_S_P;
        private ICommand_mb _cmd;

        public SaveAsPDFHandler(ICommand_mb cmd)
        {
            _cmd = cmd;
        }

        public override void Handle(KeySequence keys)
        {
            if (CanHandle(keys))
            {
                _cmd.Execute();
            }
            else
            {
                Successor?.Handle(keys);
            }
        }

        protected override bool CanHandle(KeySequence keys)
        {
            return keys.Equals(_pattern);
        }
    }
}
