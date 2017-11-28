using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    /// <summary>
    /// Part of the MusicalComposite design pattern
    /// Rrepresent music as a composite tree of staffs, measures, notes and other potential elements
    /// </summary>
    public abstract class MusicalComponent
    {
        /// <summary>
        /// Each component must have a hardcoded name.
        /// Don't build a name, exclusively return a hardcoded string.
        /// </summary>
        public abstract string ComponentName { get; }

        public abstract void AddChild(MusicalComponent m);
        public abstract void AddExtension(MusicalComponentExtension mce);
        public abstract MusicalComponentExtension GetExtensionByName(string name);
    }
}
