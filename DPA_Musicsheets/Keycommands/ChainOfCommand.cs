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

        public void Handle(List<Key> keySequence)
        {
            try
            {
                handlers.First().Handle(keySequence);
            }
            catch (Exception)
            {
                Console.Out.WriteLine("CoCHandler exception");
            }
        }
    }
}
