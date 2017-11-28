using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    /// <summary>
    /// Represents "extensions" to musical components, so you can
    /// attach any kind of extra information you want to a musical component
    /// </summary>
    public abstract class MusicalComponentExtension
    {
        // only needs a name
        // subclasses are responsible for containing information
        public abstract string ExtensionName { get; }
    }
}
