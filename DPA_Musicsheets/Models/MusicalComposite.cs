using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class MusicalComposite : MusicalComponent
    {
        public List<MusicalComponent> children = new List<MusicalComponent>();
        protected Dictionary<string, MusicalComponentExtension> extensions = new Dictionary<string, MusicalComponentExtension>();

        public override string ComponentName => "MusicalComposite";

        public override void AddChild(MusicalComponent m)
        {
            children.Add(m);
        }

        public override void AddExtension(MusicalComponentExtension mce)
        {
            extensions.Add(mce.ExtensionName, mce);
        }

        public override MusicalComponentExtension GetExtensionByName(string name)
        {
            return extensions[name];
        }
    }
}
