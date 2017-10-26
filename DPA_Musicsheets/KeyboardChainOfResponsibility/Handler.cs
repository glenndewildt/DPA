using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardChainOfResponsibility.KeyNames;

namespace KeyboardChainOfResponsibility
{
    public abstract class Handler
    {
        public abstract void Handle(KeyNames.Modifiers modifier, KeyNames.Characters character, KeyNames.Numbers number);
    }
}
