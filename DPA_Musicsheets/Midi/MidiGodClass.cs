﻿using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Midi
{
    public class MidiGodClass
    {
        private FileHandler fileHandler;
        private MidiMessageInterpreter midiMessageInterpreter = new MidiMessageInterpreter();

        private MidiLilyBuilder lilyPondContent;
        private MidiStaffBuilder midiStaffBuilder;

        public MidiGodClass(FileHandler fileHandler)
        {
            this.fileHandler = fileHandler;

            lilyPondContent = new MidiLilyBuilder();
            midiStaffBuilder = new MidiStaffBuilder();
        }

        public void LoadMidi(Sequence sequence)
        {
            lilyPondContent.AddDefaultConfiguration();

            // The midi loading acts as a sort of state machine
            // however, it's not modelled out as a state-machine in the code itself
            int division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;

            Tuple<int, int> timeSignature = null;
            int bpm;

            for (int i = 0; i < sequence.Count(); i++)
            {
                Track track = sequence[i];

                foreach (var midiEvent in track.Iterator())
                {
                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    switch (midiMessage.MessageType)
                    {
                        case MessageType.Meta:
                            var metaMessage = midiMessage as MetaMessage;
                            switch (metaMessage.MetaType)
                            {
                                case MetaType.TimeSignature:
                                    timeSignature = ParseTimeSignature(midiStaffBuilder, metaMessage);
                                    break;
                                case MetaType.Tempo:
                                    bpm = ParseTempo(midiStaffBuilder, metaMessage);
                                    break;
                                case MetaType.EndOfTrack:
                                    percentageOfBarReached = ParseEndOfTrack(midiStaffBuilder, division, previousNoteAbsoluteTicks, percentageOfBarReached, timeSignature, midiEvent);
                                    break;
                                default: break;
                            }
                            break;
                        case MessageType.Channel:
                            ParseChannel(division, ref previousMidiKey, ref previousNoteAbsoluteTicks, ref percentageOfBarReached, ref startedNoteIsClosed, timeSignature, midiEvent);
                            break;
                    }
                }
            }

            lilyPondContent.CloseScope();

            Staff staff = midiStaffBuilder.Build();

            fileHandler.staff = staff;
            fileHandler.LoadLilypond(lilyPondContent.Build());
        }

        private void ParseChannel(int division, ref int previousMidiKey, ref int previousNoteAbsoluteTicks, ref double percentageOfBarReached, ref bool startedNoteIsClosed, Tuple<int, int> timeSignature, MidiEvent midiEvent)
        {
            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
            if (channelMessage.Command == ChannelCommand.NoteOn)
            {
                // parse the message
                int loudness = midiMessageInterpreter.Loudness(channelMessage);
                if (loudness > 0)
                {
                    // Append the new note.
                    int currentMidiKey = midiMessageInterpreter.Key(channelMessage);

                    // build lily
                    lilyPondContent.AddNote(GetNoteName(previousMidiKey, currentMidiKey));

                    // update local state
                    previousMidiKey = currentMidiKey;
                    startedNoteIsClosed = false;
                }
                else if (!startedNoteIsClosed)
                {
                    // parse the message
                    int currentAbsoluteTicks = midiMessageInterpreter.AbsoluteTicks(midiEvent);

                    double percentageOfBar = CalcPercentageOfBar(division, timeSignature.Item2, previousNoteAbsoluteTicks, currentAbsoluteTicks);
                    Tuple<int, int> durationAndDots = GetNoteLength(division, timeSignature.Item1, timeSignature.Item2, percentageOfBar);

                    lilyPondContent.AddNoteDuration(durationAndDots.Item1, durationAndDots.Item2);
                    lilyPondContent.AddNoteSeparator();

                    // update local state
                    previousNoteAbsoluteTicks = currentAbsoluteTicks;

                    // update local state
                    percentageOfBarReached += percentageOfBar;
                    if (percentageOfBarReached >= 1)
                    {
                        // build lily
                        lilyPondContent.AddBar();
                        percentageOfBarReached -= 1;
                    }
                    startedNoteIsClosed = true;
                }
                else
                {
                    // build lily
                    lilyPondContent.AddCustom("r");
                }
            }
        }

        private double ParseEndOfTrack(MidiStaffBuilder midiStaffBuilder, int division, int previousNoteAbsoluteTicks, double percentageOfBarReached, Tuple<int, int> timeSignature, MidiEvent midiEvent)
        {
            if (previousNoteAbsoluteTicks > 0)
            {
                // Finish the last notelength.
                // adapt to lilybuilder interface
                int currentAbsoluteTicks = midiMessageInterpreter.AbsoluteTicks(midiEvent);

                double percentageOfBar = CalcPercentageOfBar(division, timeSignature.Item2, previousNoteAbsoluteTicks, currentAbsoluteTicks);
                Tuple<int, int> durationAndDots = GetNoteLength(division, timeSignature.Item1, timeSignature.Item2, percentageOfBar);

                lilyPondContent.AddNoteDuration(durationAndDots.Item1, durationAndDots.Item2);
                lilyPondContent.AddNoteSeparator();

                // stateful message parse
                percentageOfBarReached += percentageOfBar;
                if (percentageOfBarReached >= 1)
                {
                    // build lily
                    lilyPondContent.AddBar();
                    midiStaffBuilder.AddMeasure();
                    percentageOfBar = percentageOfBar - 1;
                }
            }

            return percentageOfBarReached;
        }

        private int ParseTempo(MidiStaffBuilder midiStaffBuilder, MetaMessage metaMessage)
        {
            int bpm;
            // parse the message
            int tempo = midiMessageInterpreter.Tempo(metaMessage);

            bpm = DPA_GLOBAL_CONSTANTS.MINUTE_IN_MICROSECONDS / tempo;
            midiStaffBuilder.SetBpm(bpm);

            // build lily
            lilyPondContent.AddTempo(bpm);
            return bpm;
        }

        private Tuple<int, int> ParseTimeSignature(MidiStaffBuilder midiStaffBuilder, MetaMessage metaMessage)
        {
            // parse the message
            Tuple<int, int> timeSignature = midiMessageInterpreter.TimeSignature(metaMessage);
            int beatNote = timeSignature.Item1;
            int beatsPerBar = timeSignature.Item2;

            midiStaffBuilder.SetBeatNote(beatNote);
            midiStaffBuilder.SetBeatsPerBar(beatsPerBar);

            // build lily
            lilyPondContent.AddTimeSignature(beatNote, beatsPerBar);
            return timeSignature;
        }

        // technically, this should be part of the MidiLilyBuilder, since it takes in midi stuff and outputs partial lily sourcecode
        private Tuple<int, int> GetNoteLength(int division, int beatNote, int beatsPerBar, double percentageOfBar)
        {
            int duration = 0;
            int dots = 0;

            if (percentageOfBar == 0)
            {
                return new Tuple<int, int>(duration, dots);
            }

            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfBar <= absoluteNoteLength)
                {
                    if (noteLength < 2)
                        noteLength = 2;

                    int subtractDuration;

                    if (noteLength == 32)
                        subtractDuration = 32;
                    else if (noteLength >= 16)
                        subtractDuration = 16;
                    else if (noteLength >= 8)
                        subtractDuration = 8;
                    else if (noteLength >= 4)
                        subtractDuration = 4;
                    else
                        subtractDuration = 2;

                    duration = CalcDuration(noteLength);
                    dots = CalcAmountOfDots(beatNote, dots, noteLength, subtractDuration);

                    break;
                }
            }

            return new Tuple<int, int>(duration, dots);
        }

        private double CalcPercentageOfBar(int division, int beatsPerBar, int absoluteTicks, int nextNoteAbsoluteTicks)
        {
            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                return 0;
            }

            double percentageOfBar;
            double percentageOfBeatNote = deltaTicks / division;
            percentageOfBar = (1.0 / beatsPerBar) * percentageOfBeatNote;
            return percentageOfBar;
        }

        private int CalcDuration(int noteLength)
        {
            int duration;
            if (noteLength >= 17)
                duration = 32;
            else if (noteLength >= 9)
                duration = 16;
            else if (noteLength >= 5)
                duration = 8;
            else if (noteLength >= 3)
                duration = 4;
            else
                duration = 2;
            return duration;
        }

        /// <summary>
        /// Calculates the amount of dots needed for a lily note
        /// </summary>
        /// <param name="beatNote"></param>
        /// <param name="dots"></param>
        /// <param name="noteLength"></param>
        /// <param name="subtractDuration"></param>
        /// <returns></returns>
        private int CalcAmountOfDots(int beatNote, int dots, int noteLength, int subtractDuration)
        {
            double currentTime = 0;

            while (currentTime < (noteLength - subtractDuration))
            {
                var addtime = 1 / ((subtractDuration / beatNote) * Math.Pow(2, dots));
                if (addtime <= 0) break;
                currentTime += addtime;
                if (currentTime <= (noteLength - subtractDuration))
                {
                    dots++;
                }
                if (dots >= 4) break;
            }

            return dots;
        }

        private string GetNoteName(int previousMidiKey, int midiKey)
        {
            List<string> notes = new List<string> { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };

            int octave = (midiKey / 12) - 1;
            string name = notes[midiKey % 12];

            int distance = midiKey - previousMidiKey;
            while (distance < -6)
            {
                name += ",";
                distance += 8;
            }

            while (distance > 6)
            {
                name += "'";
                distance -= 8;
            }

            return name;
        }
    }
}
