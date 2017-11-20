using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Staff : MusicalComposite
    { 
        public LinkedList<Measure> measures = new LinkedList<Measure>();

        // beats per minute
        public int bpm;

        // Beats
        // -----
        // BeatsPerMeasure
        public Tuple<int, int> timeSignature;

        public override void AddChild(MusicalComposite m)
        {
            throw new NotImplementedException();
        }

        public override void RemoveChild(MusicalComposite m)
        {
            throw new NotImplementedException();
        }
    }
}
