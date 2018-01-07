using DPA_Musicsheets.LilyPond;
using DPA_Musicsheets.LilyPond.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    class InsertBarsWhereMissingCommand : ICommand_mb
    {
        private LilypondEditor _lilyEditor;

        public InsertBarsWhereMissingCommand(LilypondEditor lilyEditor)
        {
            _lilyEditor = lilyEditor;
        }

        public void Execute()
        {
            string selectedText = _lilyEditor.GetSelectedText();
            
            LilyLexer lexer = new LilyLexer();
            List<t_LilypondToken> tokens = lexer.TokenizeLilySource(selectedText);

            List<int> missingBarIndices = new List<int>();

            double lengthOfBar = 1;
            double rollingBarLength = 0;

            for (int i = 0; i < tokens.Count; i++)
            {
                t_LilypondToken token = tokens[i];
                double currentTokenLength = 0;

                if (token.Type.type == "TOKEN_REST")
                {
                    // TODO: parse tokens in the parser, not in client code
                    currentTokenLength = 1 / Double.Parse(token.match.Groups["duration"].Value);
                }
                if (token.Type.type == "TOKEN_NOTE")
                {
                    // TODO: parse tokens in the parser, not in client code
                    currentTokenLength = 1 / Double.Parse(token.match.Groups["duration"].Value);
                    if (token.match.Groups["dot"].Length > 0)
                    {
                        currentTokenLength *= 1.5;
                    }
                } else
                {
                    continue;
                }

                rollingBarLength += currentTokenLength;

                if (rollingBarLength >= lengthOfBar) {
                    t_LilypondToken nextToken = GetNextNotUnknownToken(tokens, i);
                    if (nextToken == null || nextToken.Type.type != "TOKEN_BAR")
                    {
                        missingBarIndices.Add(i + 1);
                    }
                    rollingBarLength = 0;
                }
            }

            if (missingBarIndices.Count > 0)
            {
                int inserted = 0;
                foreach (int barIndex in missingBarIndices)
                {
                    // TODO: put in LilyTokenFactory
                    t_LilypondToken bar = new t_LilypondToken("|", new Lilypond.parser.TokenMatcher("TOKEN_BAR", ""), null);
                    tokens.Insert(barIndex + inserted, bar);
                    inserted += 1;
                }

                string newText = LilyLexer.TokensToString(tokens);
                _lilyEditor.SetSelectedText(newText);
                _lilyEditor.AddBookmark(_lilyEditor.SaveStateToMemento());
            }
        }

        /// <summary>
        /// // this algorithm is incredibly ugly, sorry
        // in a list of tokens, say {F, O, Y, TOKEN_UNKNOWN, G}
        // and an index, for example:      ^
        // it will return the next token (if any) in the token list that is not a TOKEN_UNKNOWN
        // so that'd be G in this case.
        // in {} it will be null
        // in {F, O, Y}
        //           ^
        // it will be null
        // in {F, O, Y, G}
        //           ^
        // it will be G
        /// </summary>
        /// <param name="tokens">a list of tokens</param>
        /// <param name="currentIndex">index to start the search from</param>
        /// <returns></returns>
        private t_LilypondToken GetNextNotUnknownToken(List<t_LilypondToken> tokens, int currentIndex) {
            while (true)
            {
                currentIndex += 1;
                if (currentIndex > tokens.Count - 1) return null;

                t_LilypondToken t = tokens[currentIndex];
                if (t.Type.type != "TOKEN_UNKNOWN")
                {
                    return t;
                }
            }
        }
    }
}
