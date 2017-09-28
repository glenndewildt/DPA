using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DPA_Musicsheets.Models;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.LilyPond
{
    class Tokenizer
    {
        private class TokenMatcher
        {
            tokenTypes type;
            Regex matcher;

            public TokenMatcher (tokenTypes type, string regexPattern)
            {
                this.type = type;
                this.matcher = new Regex(regexPattern);
            }
        }

        public enum tokenTypes
        {
            Unknown,
            Keyword,
            Note,
            Rest,
            Bar,
            Relative
        }

        private TokenMatcher TOKEN_UNKNOWN = new TokenMatcher(
            tokenTypes.Unknown,
            "the-regex-doesn't-matter-sorry"
        );
        private TokenMatcher TOKEN_NOTE = new TokenMatcher(
            tokenTypes.Note,
            // fresh, handcrafted regex that gives you the matches for
            // <name>       a, f, g, etc
            // <signature>  gis, fes, etc (atm, as and es aren't recognized, don't feel like fixing that, the example songs don't use it)
            // <apostrophe> any number of apostrophes (a'4, a''8)
            // <comma>      any number of comma's (a,4, a,,8)
            // <duration>   a4, a8, etc
            // <dot>        a4., a8.
            @"(?<notename>[a-g](?<signature>es|is)?)(?<apostrophe>'*)(?<comma>,*)(?<duration>\d+)(?<dot>\.?)"
        );
        private TokenMatcher TOKEN_REST = new TokenMatcher(
            tokenTypes.Rest,
            @"r(?<duration>\d+)"
        );
        private TokenMatcher TOKEN_BAR = new TokenMatcher(
            tokenTypes.Bar,
            @"\|"
        );
        private TokenMatcher TOKEN_RELATIVE = new TokenMatcher(
            tokenTypes.Relative,
            // we explicitly assume that the file uses c', supporting otherwise requires changing the TokenMatcher to some kind of composite pattern
            // not impossible, but takes a lot of time for a very small feature
            @"\\relative c'"
        );
        private TokenMatcher TOKEN_clef = new TokenMatcher(
            tokenTypes.clef,
            @"\\"
        );


        public List<LilypondToken> TokenizeLilySource(string source)
        {
            string[] strTokens = source.Split(null);

            List<LilypondToken> tokens = new List<LilypondToken>();

            foreach (string strToken in strTokens)
            {
                LilypondTokenKind type = GetTokenKind(strToken);
                tokens.Add(new LilypondToken(strToken, type));
            }

            return tokens;
        }

        private LilypondTokenKind GetTokenKind(string token)
        {
            
        }
    }
}
