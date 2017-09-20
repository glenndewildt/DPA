using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    class Tokenizer
    {

        public Sequence MidiSequence { get; set; }

        public String[] splitByString(string lilyPontString) {

            return lilyPontString.Split(null);

        }

        public NotenBalk arrayToNotenBalk(String[] lilypontArray) {
            foreach (var token in lilypontArray) {
                if (token == "c"|| token == "a" || token == "g" || token == "f" || token == "e" || token == "d" || token == "b")
                {

                }
            }
            return new NotenBalk();

            
        }
        

    }
}
