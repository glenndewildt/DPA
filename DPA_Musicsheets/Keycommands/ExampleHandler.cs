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

        public override void Handle(List<Key> keySequence)
        {
            Console.Out.WriteLine("Handling with example handler");
            if (keySequence[0] == Key.LeftCtrl &&
                keySequence[1] == Key.S)
            {
                exampleCommand.Execute();
            } else
            {
                NextHandler().Handle(keySequence);
            }
        }
    }
}
