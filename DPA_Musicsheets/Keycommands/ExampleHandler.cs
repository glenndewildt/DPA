using DPA_Musicsheets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    class ExampleHandler : CoCHandler
    {
        private ICommand_mb exampleCommand = new ExampleCommand();

        public override void Handle(KeySequence keys)
        {
            Console.Out.WriteLine("Handling with example handler");
            if (keys.Equals(KeysequenceDefinitions.CTRLS))
            {
                exampleCommand.Execute();
            } else
            {
                NextHandler().Handle(keys);
            }
        }
    }
}
