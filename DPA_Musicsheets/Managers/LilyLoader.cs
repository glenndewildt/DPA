using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Managers
{
    class LilyLoader
    {
        public string CleanUpLilySource(string content)
        {
            return content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
        }

        internal LinkedList<LilypondToken> fromString(string content)
        {
            content = CleanUpLilySource(content);
            return FileHandler.GetTokensFromLilypond(content);
        }
    }
}
