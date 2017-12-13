using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Keycommands
{
    class HotkeyChainOfResponsibility
    {
        private HotkeyHandler _firstHandler;
        private HotkeyHandler _lastAdded;

        public void AddHandler(HotkeyHandler handler)
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
    }
}
