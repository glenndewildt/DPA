using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    class KeysequenceDefinitions
    {
        public static KeySequence CTRLS = new KeySequence("CTRL+S", new List<Key>() { Key.LeftCtrl, Key.S });
    }
}
