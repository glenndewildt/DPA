using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Keycommands
{
    public class HotkeyChainOfResponsibility
    {
        private HotkeyHandler _firstHandler;
        private HotkeyHandler _lastAdded;

        public HotkeyHandler FirstInChain { get => _firstHandler; }

        public void AddFirstHandler(HotkeyHandler handler)
        {
            if (_firstHandler != null)
            {
                HotkeyHandler previousFirst = _firstHandler;
                handler.AddNext(previousFirst);
            }

            _firstHandler = handler;
            _lastAdded = handler;
        }

        public void AppendHandler(HotkeyHandler handler)
        {
            if (_firstHandler == null)
            {
                _firstHandler = handler;
            } else
            {
                _lastAdded.AddNext(handler);
            }

            _lastAdded = handler;
        }

        internal void Handle(KeySequence seq)
        {
            if (_firstHandler != null)
            {
                _firstHandler.Handle(seq);
            }
        }
    }
}
