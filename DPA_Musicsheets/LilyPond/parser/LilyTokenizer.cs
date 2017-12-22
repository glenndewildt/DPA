using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.LilyPond.parser
{
    public class LilyTokenizer
    {
        // performing lexical analysis of the lilypond source code is extremely simple
        // just split the input source code using `null`
        public static string[] Tokenize(string source)
        {
            return source.Split(null);
        }
    }
}
