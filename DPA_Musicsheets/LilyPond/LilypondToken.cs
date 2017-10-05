using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.LilyPond;
using static DPA_Musicsheets.LilyPond.Tokenizer;

namespace DPA_Musicsheets.LilyPond
{
    class LilypondToken
    {
        string Token { get; }
        TokenMatcher Type { get; }

        public LilypondToken(string token, TokenMatcher type)
        {
            this.Token = token;
            this.Type = type;
        }
    }
}
