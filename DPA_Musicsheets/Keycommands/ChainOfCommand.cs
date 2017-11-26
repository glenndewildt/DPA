using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    class ChainOfCommand
    {
        List<CoCHandler> handlers = new List<CoCHandler>();

        public ChainOfCommand()
        {
            // define list of handlers
            handlers.Add(HandlerDefinitions.ExampleHandler);
        }

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
    }
}
