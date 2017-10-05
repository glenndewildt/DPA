using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public enum TokenMatcher
    {
        Unknown,
        Note,
        Rest,
        Bar,
        Clef,
        Time,
        Tempo,
        Staff
    }
}
