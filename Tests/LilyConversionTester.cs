﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using DPA_Musicsheets.Midi;
using DPA_Musicsheets.Managers;

namespace Tests
{
    [TestClass]
    public class LilyConversion
    {
        [TestMethod]
        public void ConvertsCorrectly()
        {
            string expectedResult = @"\relative c' {
\clef treble
\time 4 / 4
\tempo 4 = 120
g'4 g4 a4 a4 |
d8.e16 d8 c8 b4 d4 |
g8 a8 b8 g8 a8 d,8 d'4 |
g,2 g,2 |
g4 a4 a4 |
d8 e8 d8 c8 b4 a4 |
g8 a8 g8 fis8 e4 d4 |
g8 a8 g8 fis8 e4 d4 |
g4 g4 a4 a4 |
d8 e8 d8 c8 b4 a4 |
e2 fis2 |
g8 b8 d8 b8 g2 |
g4 g4 a4 a4 |
d8 e8 d8 c8 b4 a4 |
g8 a8 g8 fis8 e4 d4 |
g8 a8 g8 fis8 e4 d4 |
g4 g4 a4 a4 |
d8 e8 d8 c8 b4 a4 |
e2 fis2 |
g8 b8 d8 b8 g2 |
g8 b8 d8 fis8 g2 |
32 }";

            StaffToLilyConverter staffToLilyConverter = new StaffToLilyConverter();

            Sequence MidiSequence = new Sequence();
            MidiSequence.Load("Files\\Alle-eendjes-zwemmen-in-het-water.mid");

            FileHandler handler = new FileHandler();

            MidiLoader midiGod = new MidiLoader(handler);
            midiGod.LoadMidi(MidiSequence);

            string actualResult = staffToLilyConverter.Convert(handler.staff);

            Assert.AreEqual(actualResult, expectedResult);
        }
    }
}
