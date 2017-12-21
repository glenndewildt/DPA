using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPA_Musicsheets.Lilypond;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.LilyPond;

namespace DPA_Musicsheets.Lilypond.Tests
{
    [TestClass()]
    public class LilypondEditorBookmarksTests
    {
        [TestMethod()]
        public void AddBookmarkTest()
        {
            // initialize the bookmarks
            LilypondEditorBookmarks bookmarks = new LilypondEditorBookmarks();
            LilypondEditorMemento mementoA = new LilypondEditorMemento("A");
            LilypondEditorMemento mementoB = new LilypondEditorMemento("B");
            LilypondEditorMemento mementoC = new LilypondEditorMemento("C");
            LilypondEditorMemento mementoD = new LilypondEditorMemento("D");
            LilypondEditorMemento mementoE = new LilypondEditorMemento("E");
            LilypondEditorMemento mementoF = new LilypondEditorMemento("F");

            // add {A, B, C, D, E, F}
            bookmarks.AddBookmark(mementoA);
            bookmarks.AddBookmark(mementoB);
            bookmarks.AddBookmark(mementoC);
            bookmarks.AddBookmark(mementoD);
            bookmarks.AddBookmark(mementoE);
            bookmarks.AddBookmark(mementoF);

            // requesting current should be the last inserted when no undo/redo's have been performed
            LilypondEditorMemento current = bookmarks.GetCurrent();
            Assert.AreEqual(current, mementoF);

            // move the index back one by one until we reach A
            // {A, B, C, D, E, F}
            //              ^
            LilypondEditorMemento b1 = bookmarks.GoBackOne();
            Assert.AreEqual(b1, mementoE);

            // {A, B, C, D, E, F}
            //           ^
            LilypondEditorMemento b2 = bookmarks.GoBackOne();
            Assert.AreEqual(b2, mementoD);

            // {A, B, C, D, E, F}
            //        ^
            LilypondEditorMemento b3 = bookmarks.GoBackOne();
            Assert.AreEqual(b3, mementoC);

            // {A, B, C, D, E, F}
            //     ^
            LilypondEditorMemento b4 = bookmarks.GoBackOne();
            Assert.AreEqual(b4, mementoB);

            // {A, B, C, D, E, F}
            //  ^
            LilypondEditorMemento b5 = bookmarks.GoBackOne();
            Assert.AreEqual(b5, mementoA);

            // now, when me move back once more, it shouldn't go further
            // it should return A again
            // {A, B, C, D, E, F}
            //  ^
            LilypondEditorMemento b6 = bookmarks.GoBackOne();
            Assert.AreEqual(b6, mementoA);

            // now, move forward to C
            // now, move forward to C again
            // {A, B, C, D, E, F}
            //     ^
            LilypondEditorMemento f1 = bookmarks.GoForwardOne();
            Assert.AreEqual(f1, mementoB);

            // {A, B, C, D, E, F}
            //        ^
            LilypondEditorMemento f2 = bookmarks.GoForwardOne();
            Assert.AreEqual(f2, mementoC);

            // now that we're in the middle, insert a new one.
            // the end result should be this
            // {A, B, C, G}
            //           ^
            LilypondEditorMemento mementoG = new LilypondEditorMemento("G");
            bookmarks.AddBookmark(mementoG);
            LilypondEditorMemento current2 = bookmarks.GetCurrent();
            Assert.AreEqual(mementoG, current2);
        }
    }
}