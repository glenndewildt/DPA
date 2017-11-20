using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public abstract class MusicalComposite
    {
        public abstract void AddChild(MusicalComposite m);
        public abstract void RemoveChild(MusicalComposite m);
    }
}
