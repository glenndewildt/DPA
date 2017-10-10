using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
   public class Note
    {
        public enum SIGNATURE { none, sharp, flat };

        public int duration;
        public int loudniss;

        public int pitch;
        public bool isRest;
        public bool isDotted;
        public SIGNATURE signature;
    }
}
