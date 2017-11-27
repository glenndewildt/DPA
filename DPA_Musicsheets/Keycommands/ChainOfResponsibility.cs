using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    class ChainOfResponsibility
    {
        private List<HotkeyHandler> handlers = new List<HotkeyHandler>();

        public void Handle(KeySequence keys)
        {
            bool sequenceIsHandled = false;
            sequenceIsHandled = handlers.First().Handle(keys);

            if (!sequenceIsHandled)
            {
                Console.Out.WriteLine($"The sequence {keys} was not handled by any listeners");
            } else
            {
                Console.Out.WriteLine($"The sequence {keys} was handled.");
            }
        }

        private void RegisterHandler(HotkeyHandler handler)
        {
            handlers.Add(handler);
        }
    }
}
