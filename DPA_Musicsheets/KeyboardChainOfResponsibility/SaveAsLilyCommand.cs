using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets;
using KeyboardChainOfResponsibility.KeyNames;

namespace KeyboardChainOfResponsibility
{
    class SaveAsLilyCommand : Command
    {
        private Command _nextHandler;

        public override void Handle(Modifiers modifier, Characters character, Numbers number)
        {
            if (modifier == Modifiers.CTRL && character == Characters.S)
            {
                // ...met CTRL + S een bestand kunnen opslaan (als lilypondbestand)

            }
            else
            {
                _nextHandler.Handle(modifier, character, number);
            }
        }

        public override void NextHandler(Command next)
        {
            _nextHandler = next;
        }
    }
}
