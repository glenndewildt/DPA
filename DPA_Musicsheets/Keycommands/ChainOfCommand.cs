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
            CoCHandler exampleHandler = new ExampleHandler();
            handlers.Add(exampleHandler);
        }

        public void Handle(KeySequence keys)
        {
            try
            {
                handlers.First().Handle(keys);
            }
            catch (Exception)
            {
                Console.Out.WriteLine("CoCHandler exception");
            }
        }
    }
}
