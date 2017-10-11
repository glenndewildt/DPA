using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Staff
    {
        public List<Measure> measures;
        public int tempo;
        public Tuple<int, int> timeSignature;

    }
}
