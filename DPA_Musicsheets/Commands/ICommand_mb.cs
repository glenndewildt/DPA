﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    public interface ICommand_mb
    {
        // the idea of an optional parameter is taken from the
        // actual ICommand interface.
        void Execute();
    }
}
