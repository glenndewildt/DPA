﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public abstract class AbstractFileOpener
    {
        public abstract void Open(string filename);
    }
}