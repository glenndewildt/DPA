using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    class MeasureBuilder
    {
        private Measure measure;

        public MeasureBuilder()
        {
            measure = new Measure();
        }

        public void addNote(Note note)
        {
            if (measure.notes == null)
            {
                measure.notes = new List<Note>();
            }
            measure.notes.Add(note);
        }

        public Measure getResult()
        {
            Measure measure = new Measure();
            measure = this.measure;
            this.measure = new Measure();
            return measure;
        }
    }
}
