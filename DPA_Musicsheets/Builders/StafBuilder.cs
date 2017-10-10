using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    class StafBuilder
    {
        private List<Measure> bars;
        private int tempo;
        private Tuple<int, int> timeSignature;

        private Staff staff;

        public StafBuilder()
        {
            staff = new Staff();
        }
        public void addMeasure(Measure measure)
        {
            if (staff.measures == null)
            {
                staff.measures = new List<Measure>();
            }
            staff.measures.Add(measure);
        }

        public void setTimesignature(int first, int second)
        {
            timeSignature = new Tuple<int, int>(first,second);

              
        }
        public void setTempo(int tempo)
        {
            this.tempo = tempo;

        }

        public Staff getResult()
        {
            return staff;
        }
    }
}
