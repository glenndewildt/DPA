using DPA_Musicsheets.Builders;
using DPA_Musicsheets.LilyPond;
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
    public class MidiLoader
    {
        private FileHandler fileHandler;
        private MidiMessageInterpreter midiMessageInterpreter = new MidiMessageInterpreter();

        private MidiStaffBuilder midiStaffBuilder;

        public MidiLoader(FileHandler fileHandler)
        {
            this.fileHandler = fileHandler;
        }

        /// <summary>
        /// Should only be called after LoadMidi
        /// </summary>
        /// <returns></returns>
        public Staff GetStaff()
        {
            return midiStaffBuilder.Build();
        }

        public void LoadMidi(Sequence sequence)
        {
            midiStaffBuilder = new MidiStaffBuilder();

            midiStaffBuilder.AddDefaultConfiguration();

            // This function is best seen as a midi/musical state machine
            // these 7 lines of code define the state of the machine
            // read the rest of the comments in this function for more info
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
                            // these three messages change the state of the machine on a high-level
                            // hence the "Meta" name
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
                            // this message is responsible for using the data in the state machine
                            // to create notes, pauses and their timings, based on the midi state machine's state.
                            // note that this thing has its own state as well..
                            ParseChannel(division, ref previousMidiKey, ref previousNoteAbsoluteTicks, ref percentageOfBarReached, ref startedNoteIsClosed, timeSignature, midiEvent);
                            break;
                    }
                }
            }

            Staff staff = midiStaffBuilder.Build();

            StaffToLilyConverter toLily = new StaffToLilyConverter();

            fileHandler.staff = staff;
            fileHandler.LoadLilypond(toLily.Convert(staff), true);
        }

        private void ParseChannel(int division, ref int previousMidiKey, ref int previousNoteAbsoluteTicks, ref double percentageOfBarReached, ref bool startedNoteIsClosed, Tuple<int, int> timeSignature, MidiEvent midiEvent)
        {
            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
            if (channelMessage.Command == ChannelCommand.NoteOn)
            {
                int loudness = midiMessageInterpreter.Loudness(channelMessage);
                if (loudness > 0)
                {
                    int currentMidiKey = midiMessageInterpreter.Key(channelMessage);

                    string noteName = GetNoteName(previousMidiKey, currentMidiKey);
                    int numberOfApostrophes = CalculateAmountOfApostrophes(previousMidiKey, currentMidiKey);
                    int numberOfCommas = CalculateAmountOfCommas(previousMidiKey, currentMidiKey);

                    midiStaffBuilder.AddNote(noteName);
                    midiStaffBuilder.AddApostrophes(numberOfApostrophes);
                    midiStaffBuilder.AddCommas(numberOfCommas);

                    previousMidiKey = currentMidiKey;
                    startedNoteIsClosed = false;
                }
                else if (!startedNoteIsClosed)
                {
                    int currentAbsoluteTicks = midiMessageInterpreter.AbsoluteTicks(midiEvent);

                    double percentageOfBar = CalcPercentageOfBar(division, timeSignature.Item2, previousNoteAbsoluteTicks, currentAbsoluteTicks);
                    Tuple<int, int> durationAndDots = GetNoteLength(division, timeSignature.Item1, timeSignature.Item2, percentageOfBar);

                    midiStaffBuilder.AddNoteDuration(durationAndDots.Item1);
                    midiStaffBuilder.AddDots(durationAndDots.Item2);

                    previousNoteAbsoluteTicks = currentAbsoluteTicks;

                    percentageOfBarReached += percentageOfBar;
                    if (percentageOfBarReached >= 1)
                    {
                        midiStaffBuilder.AddBar();
                        percentageOfBarReached -= 1;
                    }
                    startedNoteIsClosed = true;
                }
                else
                {
                    midiStaffBuilder.AddNoteDuration(0);
                }
            }
        }

        private double ParseEndOfTrack(MidiStaffBuilder midiStaffBuilder, int division, int previousNoteAbsoluteTicks, double percentageOfBarReached, Tuple<int, int> timeSignature, MidiEvent midiEvent)
        {
            if (previousNoteAbsoluteTicks > 0)
            {
                int currentAbsoluteTicks = midiMessageInterpreter.AbsoluteTicks(midiEvent);

                double percentageOfBar = CalcPercentageOfBar(division, timeSignature.Item2, previousNoteAbsoluteTicks, currentAbsoluteTicks);

                percentageOfBarReached += percentageOfBar;
                if (percentageOfBarReached >= 1)
                {
                    midiStaffBuilder.AddBar();
                    percentageOfBar = percentageOfBar - 1;
                }
            }

            return percentageOfBarReached;
        }

        private int ParseTempo(MidiStaffBuilder midiStaffBuilder, MetaMessage metaMessage)
        {
            int bpm;
            int tempo = midiMessageInterpreter.Tempo(metaMessage);

            bpm = DPA_GLOBAL_CONSTANTS.MINUTE_IN_MICROSECONDS / tempo;
            midiStaffBuilder.AddTempo(bpm);
            return bpm;
        }

        private Tuple<int, int> ParseTimeSignature(MidiStaffBuilder midiStaffBuilder, MetaMessage metaMessage)
        {
            Tuple<int, int> timeSignature = midiMessageInterpreter.TimeSignature(metaMessage);
            int beatNote = timeSignature.Item1;
            int beatsPerBar = timeSignature.Item2;

            midiStaffBuilder.AddTimeSignature(beatNote, beatsPerBar);
            return timeSignature;
        }

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

        private int CalculateAmountOfCommas(int previousMidiKey, int midiKey)
        {
            int distance = midiKey - previousMidiKey;
            int amount = 0;

            while (distance < -6)
            {
                amount += 1;
                distance += 8;
            }

            return amount;
        }

        private int CalculateAmountOfApostrophes(int previousMidiKey, int midiKey)
        {
            int distance = midiKey - previousMidiKey;
            int amount = 0;

            while (distance > 6)
            {
                amount += 1;
                distance -= 8;
            }

            return amount;
        }

        private string GetNoteName(int previousMidiKey, int midiKey)
        {
            List<string> notes = new List<string> { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };

            return notes[midiKey % 12];
        }
    }
}
