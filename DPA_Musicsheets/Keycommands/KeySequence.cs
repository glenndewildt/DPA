using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Keycommands
{
    public class KeySequence
    {
        private string _name;
        private List<Key> _keys;
        
        public KeySequence(string name, List<Key> keys)
        {
            _name = name;
            _keys = keys;
        }

        /// <summary>
        /// content based equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(KeySequence other)
        {
            if (other._keys.Count() == this._keys.Count())
            {
                for (int i = 0; i < this._keys.Count(); i++)
                {
                    if (this._keys[i] != other._keys[i])
                    {
                        return false;
                    }
                }
            } else
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            Key key;

            StringBuilder s = new StringBuilder();
            s.Append("Key sequence: ");
            for (int i = 0; i < _keys.Count() - 1; i++)
            {
                key = _keys[i];
                s.Append(key.ToString());
                s.Append(" + ");
            }
            key = _keys.Last();
            s.Append(key.ToString());
            return s.ToString();
        }

        /// <summary>
        /// log the sequence for debugging purposes
        /// </summary>
        public void LogKeysequence()
        {
            Console.Out.WriteLine(this.ToString());
        }
    }
}
