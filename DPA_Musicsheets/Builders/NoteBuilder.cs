using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    class NoteBuilder
    {
        private Note note;

        public NoteBuilder() {
            note = new Note();
        }

        public void setDuration(int dur)
        {
            String notes = "C C#D D#E F F#G G#A A#B ";
            int octv;
            String nt;
            for (int noteNum = 0; noteNum < 128; noteNum++)
            {
                octv = noteNum / 12 - 1;
                nt = notes.Substring((dur % 12) * 2, (dur % 12) * 2 + 2);
                Console.WriteLine("Note # " + noteNum + " = octave " + octv + ", note " + nt);
            }
            note.duration = dur;
        }
        public void setPitch(int pitch)
        {
            note.pitch = pitch;
        }
        public void setIsRest(bool isRest)
        {
            note.isRest = isRest;
        }
        public void setSignature(Note.SIGNATURE sig)
        {
            note.signature = sig;
        }
        public Note getResult() {
            return note;
        }




    }
}
