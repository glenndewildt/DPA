using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Midi
{
    public class MidiParser
    {
        public Tuple<int, int> TimeSignature(MetaMessage message)
        {
            byte[] timeSignatureBytes = message.GetBytes();

            int _beatNote = timeSignatureBytes[0];
            int _beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));

            return Tuple.Create<int, int>(_beatNote, _beatsPerBar);
        }

        public int Tempo(MetaMessage message)
        {
            byte[] tempoBytes = message.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);

            return tempo;
        }

        public int Loudness(ChannelMessage message)
        {
            // in midi, ChannelMessage.Data2 is defined as the 'loudness'
            //          ChannelMessage.Data1 is defined as the 'pitch'
            return message.Data2;
        }

        // decouple actual Midi interface from DPA_Midi interface, even though we use
        // the exact same terminology and data underneath.
        public int AbsoluteTicks(MidiEvent message)
        {
            return message.AbsoluteTicks;
        }

        public int Key(ChannelMessage message)
        {
            return message.Data1;
        }
    }
}
