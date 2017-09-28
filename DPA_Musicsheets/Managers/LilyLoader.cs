using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    class LilyLoader
    {
        public string CleanUpLilySource(string content)
        {
            return content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
        }
    }
}
