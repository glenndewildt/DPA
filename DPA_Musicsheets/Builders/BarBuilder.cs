using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    class BarBuilder
    {

        private Bar bar;

        public BarBuilder()
        {
            bar = new Bar();
        }

        public void addMeasure(Measure mas)
        {
            if (bar.measures == null) {
                bar.measures = new List<Measure>();
            }
            bar.measures.Add(mas);
        }
      
        public Bar getResult()
        {
            return bar;
        }

    }
}
