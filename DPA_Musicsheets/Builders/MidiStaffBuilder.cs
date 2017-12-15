using DPA_Musicsheets.LilyPond;
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
            n.AddExtension(new LilyNoteInformation());
            _lastMeasure.AddChild(n);
            _lastNote = n;
        }

        public void AddNoteDuration(int duration)
        {
            _lastNote.duration = duration;
        }

        public void AddDots(int amountOfDots)
        {
            LilyNoteInformation lily = (LilyNoteInformation)_lastNote.GetExtensionByName("LilyNoteInformation");
            lily.amountOfDots = amountOfDots;
        }

        public void AddApostrophes(int numberOfApostrophes)
        {
            LilyNoteInformation lily = (LilyNoteInformation)_lastNote.GetExtensionByName("LilyNoteInformation");
            lily.amountOfApostrophes = numberOfApostrophes;
        }

        public void AddCommas(int numberOfCommas)
        {
            LilyNoteInformation lily = (LilyNoteInformation)_lastNote.GetExtensionByName("LilyNoteInformation");
            lily.amountOfCommas = numberOfCommas;
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
