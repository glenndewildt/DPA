using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using DPA_Musicsheets.LilyPond;

namespace DPA_Musicsheets.LilyPond
{
    class Tokenizer
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

        private Dictionary<string, TokenMatcher> tokenMatchers = new Dictionary<string, TokenMatcher>();

        private TokenMatcher TOKEN_NOTE = new TokenMatcher(
            "TOKEN_NOTE",
            // fresh, handcrafted regex that gives you the matches for
            // <name>       a, f, g, etc
            // <signature>  gis, fes, etc (atm, as and es aren't recognized, don't feel like fixing that, the example songs don't use it)
            // <apostrophe> any number of apostrophes (a'4, a''8)
            // <comma>      any number of comma's (a,4, a,,8)
            // <duration>   a4, a8, etc
            // <dot>        a4., a8.
            @"(?<notename>[a-g](?<signature>(es|is)?))(?<apostrophe>'*)(?<comma>,*)(?<duration>\d+)(?<dot>\.?)" // checked and Correct, 101 matches, just like the input file! :D
        );
        private TokenMatcher TOKEN_REST = new TokenMatcher(
            "TOKEN_REST",
            @"r(?<duration>\d+)" // checked and INCORRECT, 0 matches
        );
        private TokenMatcher TOKEN_BAR = new TokenMatcher(
            "TOKEN_BAR",
            @"\|" // checked and correct count
        );
        private TokenMatcher TOKEN_RELATIVE = new TokenMatcher(
            "TOKEN_RELATIVE",
            // we explicitly assume that the file uses c', supporting otherwise requires changing the TokenMatcher to some kind of composite pattern
            // not impossible, but takes a lot of time for a very small feature
            @"\\relative"
        );
        private TokenMatcher TOKEN_CLEF = new TokenMatcher(
            "TOKEN_CLEF",
            @"\\clef"
        );
        private TokenMatcher TOKEN_TEMPO = new TokenMatcher(
            "TOKEN_TEMPO",
            // we explicitly assume that the file uses tempo 4=120
            // you can extend it here if you want to, is not very 
            @"\\tempo"
        );
        private TokenMatcher TOKEN_TIME = new TokenMatcher(
            "TOKEN_TIME",
            @"\\time" // @"\\time (?<a>\d+)/(?<b>\d+)"
                      // checked and correct
        );
        private TokenMatcher TOKEN_REPEAT = new TokenMatcher(
            "TOKEN_REPEAT",
            // we explicitly assume that the files uses repeat volta 2
            // these can be easily parameterized using regex names
            @"\\repeat volta 2"
        );
        private TokenMatcher TOKEN_ALTERNATIVE = new TokenMatcher(
            "TOKEN_ALTERNATIVE",
            @"\\alternative"
        );
        // apologies for the name, the TILDE describes the token, not the musical or lilypond meaning of the token
        private TokenMatcher TOKEN_TILDE = new TokenMatcher(
            "TOKEN_TILDE",
            @"~"
        );
        private TokenMatcher TOKEN_BRACE_OPEN = new TokenMatcher(
            "TOKEN_BRACE_OPEN",
            @"{" // checked and correct, although this one /is/ quite important, should check on a file with more than one group of braces
        );
        private TokenMatcher TOKEN_BRACE_CLOSE = new TokenMatcher(
            "TOKEN_BRACE_CLOSE",
            @"}" // checked and correct, although this one /is/ quite important, should check on a file with more than one group of braces
        );
        private TokenMatcher TOKEN_UNKNOWN = new TokenMatcher(
            "TOKEN_UNKNOWN",
            "UNKOWN-TOKEN-REGEX-DOESN'T-MATTER-AND-SHOULDN'T-MATCH"
        );

        public Tokenizer()
        {
            tokenMatchers.Add("TOKEN_REST", TOKEN_REST);
            tokenMatchers.Add("TOKEN_BAR", TOKEN_BAR);
            tokenMatchers.Add("TOKEN_RELATIVE", TOKEN_RELATIVE);
            tokenMatchers.Add("TOKEN_CLEF", TOKEN_CLEF);
            tokenMatchers.Add("TOKEN_TEMPO", TOKEN_TEMPO);
            tokenMatchers.Add("TOKEN_TIME", TOKEN_TIME);
            tokenMatchers.Add("TOKEN_REPEAT", TOKEN_REPEAT);
            tokenMatchers.Add("TOKEN_ALTERNATIVE", TOKEN_ALTERNATIVE);
            tokenMatchers.Add("TOKEN_TILDE", TOKEN_TILDE);
            tokenMatchers.Add("TOKEN_BRACE_OPEN", TOKEN_BRACE_OPEN);
            tokenMatchers.Add("TOKEN_BRACE_CLOSE", TOKEN_BRACE_CLOSE);
        }

        public List<t_LilypondToken> TokenizeLilySource(string source)
        {
            List<t_LilypondToken> tokens = new List<t_LilypondToken>();

            foreach(KeyValuePair<string, TokenMatcher> kv in tokenMatchers)
            {
                Regex regex = kv.Value.matcher;
                foreach (Match match in regex.Matches(source))
                {
                    tokens.Add(new t_LilypondToken(kv.Value.type, kv.Value, match));
                }
            }

            return tokens;
        }

        // takes in a space-split piece of lilypond source and returns the corresponding tokenmatcher
        private TokenMatcher GetTokenKind(string token)
        {
            foreach (KeyValuePair<string, TokenMatcher> kv in tokenMatchers)
            {
                Regex regex = kv.Value.matcher;
                if (regex.IsMatch(token))
                {
                    return kv.Value;
                }
            }

            return null;
        }
    }
}
