﻿using DPA_Musicsheets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Keycommands
{
    class InsertTime3_4Handler : HotkeyHandler
    {
        private KeySequence _pattern = KeysequenceDefinitions.ALT_T_3;
        private ICommand_mb _cmd;

        public InsertTime3_4Handler(ICommand_mb cmd)
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