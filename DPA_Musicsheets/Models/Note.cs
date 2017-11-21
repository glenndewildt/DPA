using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Note : MusicalComponent
    {
        public int duration;
        public int pitch;

        public override void AddChild(MusicalComponent m)
        {
            throw new NotImplementedException("Notes can't have children");
        }
    }
}
