using DPA_Musicsheets.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    public abstract class HotkeyHandler
    {
        public HotkeyHandler Successor { get; set; }

        public abstract void Handle(KeySequence keys);
        public virtual void AddNext(HotkeyHandler successor)
        {
            Successor = successor;
        }
        protected abstract bool CanHandle(KeySequence keys);
    }
}