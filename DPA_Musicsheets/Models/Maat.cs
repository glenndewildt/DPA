using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{

    class Maat
    {
        public Maat vorige;
        public Maat volgende;
        int duration;
        public Noot vorigeNoot;
        public Noot volgendeNoot;

        public Maat(int duration) {
            this.duration = duration;
        }

       

    }
   
}
