using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardChainOfResponsibility;
using KeyboardChainOfResponsibility.KeyNames;

namespace DPA_Musicsheets.KeyboardChainOfResponsibility
{
    class SaveAsPDFCommand : Handler
    {
        private Handler _nextHandler;

        public override void Handle(Modifiers modifier, Characters character, Numbers number)
        {
            if (modifier == Modifiers.ALT && character == Characters.S)
            {
                //...met CTRL + S + P een bestand als PDF opslaan
                // original requirement was CTRL + S + P,
                // but our implementation doesn't support
                // using multiple characters, just
                // chains of <MODIFIER><CHARACTER><NUMBER>
                // se I changed it to
                // ALT + S
            }
            else
            {
                _nextHandler.Handle(modifier, character, number);
            }
        }

        public override void NextHandler(Handler next)
        {
            _nextHandler = next;
        }
    }
}
