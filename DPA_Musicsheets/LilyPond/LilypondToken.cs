using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.LilyPond;
using static DPA_Musicsheets.LilyPond.Tokenizer;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.LilyPond
{
    class t_LilypondToken
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
    }
}
