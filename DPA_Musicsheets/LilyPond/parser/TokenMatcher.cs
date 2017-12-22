using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Lilypond.parser
{
    public class TokenMatcher
    {
        public string type;
        public Regex matcher;

        public TokenMatcher(string type, string regexPattern)
        {
            this.type = type;
            this.matcher = new Regex(regexPattern);
        }
    }
}
