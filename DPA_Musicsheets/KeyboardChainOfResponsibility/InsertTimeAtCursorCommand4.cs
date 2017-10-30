using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardChainOfResponsibility;
using KeyboardChainOfResponsibility.KeyNames;

namespace DPA_Musicsheets.KeyboardChainOfResponsibility
{
    class InsertTimeAtCursorCommand4 : Command
    {
        private Command _nextHandler;

        public override void Handle(Modifiers modifier, Characters character, Numbers number)
        {
            if (modifier == Modifiers.ALT && character == Characters.T && number == Numbers.SIX)
            {
                //  ...met ALT + T + 6 een time 6/8 invoegen op de huidige plek

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
