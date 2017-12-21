using DPA_Musicsheets.LilyPond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Lilypond
{
    public class LilypondEditorBookmarks
    {
        private List<LilypondEditorMemento> _bookmarks = new List<LilypondEditorMemento>();
        private int _bookmarkIndex = 0;

        public void AddBookmark(LilypondEditorMemento memento)
        {
            // when a new bookmark gets added, but you're into the history of bookmarks
            // ie, when your _bookmarks looks like
            // {A, B, C, D, E, F}
            //        ^
            // (with ^ being the bookmarkIndex)
            // and you insert a new one, "G"
            // then everything after C should get removed to insert G afterwards
            // resulting in this
            // {A, B, C, G}
            int lastIndex = _bookmarks.Count - 1;
            if (_bookmarkIndex < lastIndex)
            {
                int removeStartIndex = _bookmarkIndex + 1;
                int removeCount = _bookmarks.Count - removeStartIndex;
                _bookmarks.RemoveRange(removeStartIndex, removeCount);
                _bookmarks.Add(memento);
                _bookmarkIndex = _bookmarks.Count - 1;
            } else
            {
                _bookmarks.Add(memento);
                _bookmarkIndex = _bookmarks.Count - 1;
            }

            Console.Out.WriteLine("After adding bookmark: " + ToString());
        }

        public LilypondEditorMemento GetCurrent()
        {
            return _bookmarks[_bookmarkIndex];
        }

        public void Clear()
        {
            _bookmarks = new List<LilypondEditorMemento>();
        }
        
        public LilypondEditorMemento GoBackOne()
        {
            Console.Out.WriteLine("before undo " + ToString());
            if (_bookmarkIndex == 0)
            {
                return _bookmarks[0];
            } else
            {
                _bookmarkIndex -= 1;
                return _bookmarks[_bookmarkIndex];
            }
        }

        public LilypondEditorMemento GoForwardOne()
        {
            Console.Out.WriteLine("before redo " + ToString());
            if (_bookmarkIndex == _bookmarks.Count - 1)
            {
                return _bookmarks[_bookmarks.Count + 1];
            } else
            {
                _bookmarkIndex += 1;
                return _bookmarks[_bookmarkIndex];
            }
        }

        public bool CanUndo()
        {
            return _bookmarkIndex > 0;
        }

        public bool CanRedo()
        {
            return _bookmarkIndex < _bookmarks.Count - 1;
        }

        public override string ToString ()
        {
            StringBuilder s = new StringBuilder();
            s.Append("{ ");
            for (int i = 0; i < _bookmarks.Count; i++)
            {
                LilypondEditorMemento bookmark = _bookmarks[i];
                s.Append(bookmark.id + ", ");
            }
            s.Append("}");
            s.Append("\n");
            s.Append("At index ");
            s.Append(_bookmarkIndex);
            if (_bookmarks.Count > 0)
            {
                s.Append(" which is id ");
                s.Append(_bookmarks[_bookmarkIndex].id);
            }
            
            return s.ToString();
        }
    }
}
