using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.LilyPond
{
    public class LilyNoteInformation : MusicalComponentExtension
    {
        public int amountOfDots = 0; // .
        public int amountOfApostrophes = 0; // '
        public int amountOfCommas = 0; // ,

        public override string ExtensionName => "LilyNoteInformation";
    }
}
