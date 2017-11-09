using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Staff
    { 
        public LinkedList<Measure> measures;

        // beats per minute
        public int bpm;

        // Beats
        // -----
        // BeatsPerMeasure
        public Tuple<int, int> timeSignature;
    }
}
