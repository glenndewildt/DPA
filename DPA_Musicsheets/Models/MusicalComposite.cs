using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class MusicalComposite : MusicalComponent
    {
        protected LinkedList<MusicalComponent> _children = new LinkedList<MusicalComponent>();

        public override void AddChild(MusicalComponent m)
        {
            _children.AddLast(m);
        }
    }
}
