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

        public override string ComponentName => "MusicalComposite";

        public override void AddChild(MusicalComponent m)
        {
            children.Add(m);
        }
    }
}
