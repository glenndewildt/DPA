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
            Clef,
            Relative,
            Tempo,
            Time,
            Repeat,
            Alternative,
            Tilde
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
        private TokenMatcher TOKEN_CLEF = new TokenMatcher(
            tokenTypes.Clef,
            @"\\clef treble"
        );
        private TokenMatcher TOKEN_TEMPO = new TokenMatcher(
            tokenTypes.Tempo,
            // we explicitly assume that the file uses tempo 4=120
            // you can extend it here if you want to, is not very 
            @"\\tempo 4=120"
        );
        private TokenMatcher TOKEN_TIME = new TokenMatcher(
            tokenTypes.Time,
            @"\\time (?<a>\d+)/(?<b>\d+)"
        );
        private TokenMatcher TOKEN_REPEAT = new TokenMatcher(
            tokenTypes.Repeat,
            // we explicitly assume that the files uses repeat volta 2
            // these can be easily parameterized using regex names
            @"\\repeat volta 2"
        );
        private TokenMatcher TOKEN_ALTERNATIVE = new TokenMatcher(
            tokenTypes.Alternative,
            @"\\alternative"
        );
        // apologies for the name, the TILDE describes the token, not the musical or lilypond meaning of the token
        private TokenMatcher TOKEN_TILDE = new TokenMatcher(
            tokenTypes.Tilde,
            @"~"
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
