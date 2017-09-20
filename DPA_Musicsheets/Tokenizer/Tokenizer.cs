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
            List<Noot> noten = new List<Noot>();
            
            foreach (var token in lilypontArray) {
                if (token.Contains("c")|| token.Contains("a") || token.Contains("g") || token.Contains("f") || token.Contains("e") || token.Contains("d") || token.Contains("b"))
                {
                    var noot = new Noot();
                    noot.tone = token;
                    noten.Add(noot);

                }
            }
            return new NotenBalk();

            
        }
        

    }
}
