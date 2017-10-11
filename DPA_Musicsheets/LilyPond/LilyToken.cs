using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DPA_Musicsheets.Lilypond.LilyLexer;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Lilypond;

namespace DPA_Musicsheets.Lilypond
{
    public class t_LilypondToken
    {
        string Token { get; }
        TokenMatcher Type { get; }
        Match match;

        public t_LilypondToken(string token, TokenMatcher type, Match m)
        {
            this.Token = token;
            this.Type = type;
            this.match = m;
        }

        // just a debugging method
        public override string ToString()
        {
            string s = "";
            s += this.Type.type;
            if (this.match.Success)
            {
                s += " with match `" + this.match.Value + "`";
            } else
            {
                // the this.Token field contains the token value when there was no regex match
                s += ", was unable to find a match for `" + this.Token + "`";
            }
            return s;
        }
    }
}
