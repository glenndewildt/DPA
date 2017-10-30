using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardChainOfResponsibility;
using KeyboardChainOfResponsibility.KeyNames;

namespace DPA_Musicsheets.KeyboardChainOfResponsibility
{
    class OpenFileCommand : Command
    {
        private Command _nextHandler;

public override void Handle(Modifiers modifier, Characters character, Numbers number)
        {
            if (modifier == Modifiers.CTRL && character == Characters.O)
            {
                // met CTRL + O een bestand openen

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
