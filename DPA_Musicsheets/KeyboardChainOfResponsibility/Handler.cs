﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardChainOfResponsibility.KeyNames;

namespace KeyboardChainOfResponsibility
{
    public abstract class Handler
    {
        public abstract void Handle(Modifiers modifier, Characters character, Numbers number);
        public abstract void NextHandler(Handler next);
    }
}