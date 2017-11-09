using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Midi
{
    public class MidiStaffBuildAdapter
    {
        // builds a staff from midi interface
        private Staff _staffProduct;

        private int beatNote = 0;
        private int beatsPerBar = 0;

        public MidiStaffBuildAdapter()
        {
            _staffProduct = new Staff();
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
    }

}
