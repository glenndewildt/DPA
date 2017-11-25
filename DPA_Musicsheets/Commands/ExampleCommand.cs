using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class ExampleCommand : ICommand_mb
    {
        public void Execute()
        {
            Console.Out.WriteLine("Example command executed!");
        }
    }
}
