using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Measure : MusicalComposite
    {
        public Tuple<int, int> timeSignature;
    }
}
