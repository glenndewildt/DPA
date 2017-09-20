using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class NoteBuilder
    {
        Noot buildNoot;
        public NoteBuilder() {
            buildNoot =new Noot();
        }

        public void setDuration(int duration) {
            buildNoot.duration = duration;
        }
        public void setTone(String tone) {
            buildNoot.tone = tone;
        }
        public void setOctave(char octave) {
            buildNoot.octave = octave;
        }
        public void setHasPoint(bool hasPoint) {
            buildNoot.hasPoint = hasPoint;
        }
        public void setIsHarp(bool hasHarp) {
            buildNoot.isHarp = hasHarp;
        }
        public void setIsMoll(bool hasMoll) {
            buildNoot.isMoll = hasMoll;
        }

        public Noot getResult() {
            return buildNoot;
        }

    }
}
