using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    public abstract class CoCHandler
    {
        public CoCHandler Successor { get; set; }

        protected CoCHandler NextHandler()
        {
            if (Successor == null)
            {
                throw new System.Exception("No handler specified");
            } else
            {
                return Successor;
            }
        }

        public abstract void Handle(KeySequence keys);
    }
}