using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    public class MidiLilyBuilder : IMidiLilyBuilder
    {
        private StringBuilder _lilyProduct;

        public MidiLilyBuilder()
        {
            _lilyProduct = new StringBuilder();
        }

        public void AddDefaultConfiguration()
        {
            _lilyProduct.AppendLine("\\relative c' {");
            _lilyProduct.AppendLine("\\clef treble");
        }

        public void AddTimeSignature(int beatNote, int beatsPerBar)
        {
            _lilyProduct.AppendLine($"\\time {beatNote}/{beatsPerBar}");
        }

        public void AddTempo(int bpm)
        {
            _lilyProduct.AppendLine($"\\tempo 4={bpm}");
        }

        public void AddNoteLength(string noteLength)
        {
            _lilyProduct.Append(noteLength);
        }

        public void AddBar()
        {
            _lilyProduct.AppendLine("|");
        }

        public void AddNote(string note)
        {
            _lilyProduct.Append(note);
        }

        public void AddNoteDuration(int duration, int amountOfDots)
        {
            _lilyProduct.Append(duration + new string('.', amountOfDots));
        }

        public void AddNoteSeparator()
        {
            _lilyProduct.Append(" ");
        }

        public void OpenScope()
        {
            _lilyProduct.Append("{");
        }

        public void CloseScope()
        {
            _lilyProduct.Append("}");
        }

        /// <summary>
        /// Appends anything that does not fit in with the
        /// rest of the methods, does not add new line
        /// </summary>
        /// <param name="wildcard">Piece of custom content that will be added to the lily source</param>
        public void AddCustom(string wildcard)
        {
            _lilyProduct.Append("r");
        }

        public string Build()
        {
            return _lilyProduct.ToString();
        }
    }
}
