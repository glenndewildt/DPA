﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class Measure
    {
        public LinkedList<Note> notes;
        public Tuple<int, int> timeSignature;
    }
}
