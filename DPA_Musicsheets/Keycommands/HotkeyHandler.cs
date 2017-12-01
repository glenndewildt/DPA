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

    public class InsertBlaCommand : ICommand_mb
    {
        public void Execute()
        {
            Console.Out.WriteLine("Bla command executed");
        }
    }

    public class InsertBlaHandler : HotkeyHandler
    {
        private KeySequence _keys;

        public override void Handle(KeySequence keys)
        {
            if (CanHandle(keys))
            {
                // executeCommand();
            }
        }

        protected override bool CanHandle(KeySequence keys)
        {
            return _keys.Equals(keys);
        }
    }
}