using DPA_Musicsheets.Commands;
using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    public class CoCHandler
    {
        public CoCHandler Successor { get; set; }
        private KeySequence _keySeq;
        private ICommand_mb _command;

        public CoCHandler(KeySequence keySeq, ICommand_mb command)
        {
            _keySeq = keySeq;
            _command = command;
        }

        protected CoCHandler NextHandler()
        {
            if (Successor == null)
            {
                throw new System.Exception("No handler specified");
            } else
            {
                return Successor;
            }
        }

        public void Handle(KeySequence keys)
        {
            if (keys.Equals(_keySeq))
            {
                _command.Execute();
            }
            else
            {
                NextHandler().Handle(keys);
            }
        }
    }
}