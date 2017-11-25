using DPA_Musicsheets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Keycommands
{
    class HandlerDefinitions
    {
        public static CoCHandler ExampleHandler = new CoCHandler(KeysequenceDefinitions.CTRLS, new ExampleCommand());
    }
}
