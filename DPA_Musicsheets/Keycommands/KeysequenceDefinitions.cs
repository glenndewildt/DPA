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
        public static readonly KeySequence CTRL_S = new KeySequence("CTRL+S", new List<Key>() { Key.LeftCtrl, Key.S });
        public static readonly KeySequence CTRL_S_P = new KeySequence("CTRL+S+P", new List<Key>() { Key.LeftCtrl, Key.S, Key.P });
        public static readonly KeySequence CTRL_O = new KeySequence("CTRL+O", new List<Key>() { Key.LeftCtrl, Key.O });
        public static readonly KeySequence ALT_C = new KeySequence("ALT+C", new List<Key>() { Key.LeftAlt, Key.C });
        public static readonly KeySequence ALT_S = new KeySequence("ALT+S", new List<Key>() { Key.LeftAlt, Key.S });
        public static readonly KeySequence ALT_T = new KeySequence("ALT+T", new List<Key>() { Key.LeftAlt, Key.T });
        public static readonly KeySequence ALT_T_4 = new KeySequence("ALT+T+4", new List<Key>() { Key.LeftAlt, Key.T, Key.D4 });
        public static readonly KeySequence ALT_T_3 = new KeySequence("ALT+T+3", new List<Key>() { Key.LeftAlt, Key.T, Key.D3 });
        public static readonly KeySequence ALT_T_6 = new KeySequence("ALT+T+6", new List<Key>() { Key.LeftAlt, Key.T, Key.D6 });
        public static readonly KeySequence ALT_B = new KeySequence("ALT+B", new List<Key>() { Key.LeftAlt, Key.B });
        public static readonly KeySequence ALT_R = new KeySequence("ALT+R", new List<Key>() { Key.LeftAlt, Key.R });
        public static readonly KeySequence ALT_A = new KeySequence("ALT+A", new List<Key>() { Key.LeftAlt, Key.A });
    }
}
