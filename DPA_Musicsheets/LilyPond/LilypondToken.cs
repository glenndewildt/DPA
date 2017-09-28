using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.LilyPond
{
    class LilypondToken
    {
        string Token { get; }
        LilypondTokenKind Type { get; }

        public LilypondToken(string token, LilypondTokenKind type)
        {
            this.Token = token;
            this.Type = type;
        }
    }
}
