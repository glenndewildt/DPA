using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    class MidiStaffBuilder
    {
        private Staff _staff = new Staff();

        // reference to the last accessed measure
        private Measure _lastMeasure;
        // reference to the last accessed note
        private Note _lastNote;

        public void AddBar()
        {
            Measure newMeasure = new Measure();
            _staff.AddChild(newMeasure);
            _lastMeasure = newMeasure;
        }

        public void AddDefaultConfiguration()
        {
            Measure m = new Measure();
            _staff.AddChild(m);
            _lastMeasure = m;
        }

        public void AddNote(string note)
        {
            Note n = new Note();
            n.name = note;
            _lastMeasure.AddChild(n);
            _lastNote = n;
        }

        public void AddNoteDuration(int duration, int amountOfDots)
        {
            _lastNote.duration = duration;
        }

        public void AddTempo(int bpm)
        {
            _staff.bpm = bpm;
        }

        public void AddTimeSignature(int beatNote, int beatsPerBar)
        {
            _staff.timeSignature = new Tuple<int, int>(beatNote, beatsPerBar);
        }

        public Staff Build()
        {
            return _staff;
        }
    }
}
