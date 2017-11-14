using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Midi
{
    // builds a staff, this class was designed as a refactor from midi, hence the "Midi" bias
    // it has 0 knowledge of Midi though, should be reusable.
    public class MidiStaffBuilder
    {
        private Staff _staffProduct;
        private Measure _currentMeasure;


        private int beatNote = 0;
        private int beatsPerBar = 0;

        public MidiStaffBuilder()
        {
            _staffProduct = new Staff();
            _currentMeasure = new Measure();
            _staffProduct.measures.AddFirst(_currentMeasure);
        }

        public void SetBeatNote(int beatNote)
        {
            this.beatNote = beatNote;
        }

        public void SetBeatsPerBar(int beatsPerBar)
        {
            this.beatsPerBar = beatsPerBar;
        }

        public void SetBpm(int bpm)
        {
            _staffProduct.bpm = bpm;
        }

        public Staff Build()
        {
            if (beatNote == 0 && beatsPerBar == 0)
            {
                throw new Exception("Both methods `SetBeatNote` and `SetBeatsPerBar` are required");
            }

            _staffProduct.timeSignature = new Tuple<int, int>(beatNote, beatsPerBar);

            return _staffProduct;
        }

        public void AddMeasure()
        {
            Measure newMeasure = new Measure();
            _staffProduct.measures.AddAfter(_staffProduct.measures.Find(_currentMeasure), newMeasure);

            _currentMeasure = newMeasure;
        }
    }
}
