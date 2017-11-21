
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Midi;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class FileHandler
    {
        private string _lilypondText;
        public string LilypondText
        {
            get { return _lilypondText; }
            set
            {
                _lilypondText = value;
                LilypondTextChanged?.Invoke(this, new LilypondEventArgs() { LilypondText = value });
            }
        }
        public List<MusicalSymbol> WPFStaffs { get; set; } = new List<MusicalSymbol>();

        public Sequence MidiSequence { get; set; }

        public event EventHandler<LilypondEventArgs> LilypondTextChanged;
        public event EventHandler<WPFStaffsEventArgs> WPFStaffsChanged;
        public event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;

        public Staff staff;

        private MidiLoader midiLoader;

        public FileHandler()
        {
            midiLoader = new MidiLoader(this);
        }

        private void OpenMidi(string fileName)
        {
            MidiSequence = new Sequence();
            MidiSequence.Load(fileName);
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
            midiLoader.LoadMidi(MidiSequence);
        }

        public void OpenFile(string fileName)
        {
            if (Path.GetExtension(fileName).EndsWith(".mid"))
            {
                OpenMidi(fileName);
            }
            else if (Path.GetExtension(fileName).EndsWith(".ly"))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var line in File.ReadAllLines(fileName))
                {
                    sb.AppendLine(line);
                }

                LilypondText = sb.ToString();

                LoadLilypond(sb.ToString());
            }
            else
            {
                throw new NotSupportedException($"File extension {Path.GetExtension(fileName)} is not supported.");
            }
        }

        public void LoadLilypond(string content)
        {
            LilypondText = content;
            content = content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
            LinkedList<LilypondToken> tokens = GetTokensFromLilypond(content);
            WPFStaffs.Clear();
            string message;
            WPFStaffs.AddRange(GetStaffsFromTokens(tokens, out message));
            WPFStaffsChanged?.Invoke(this, new WPFStaffsEventArgs() { Symbols = WPFStaffs, Message = message });

            MidiSequence = GetSequenceFromWPFStaffs();
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
        }

        #region Staffs loading
        private static IEnumerable<MusicalSymbol> GetStaffsFromTokens(LinkedList<LilypondToken> tokens, out string message)
        {
            // this was defined in FileHandler's scope, is now in function scope
            // it was used exclusively in GetStaffsFromTokens, FileHandler's complexity
            List<Char> notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };

            List<MusicalSymbol> symbols = new List<MusicalSymbol>();
            message = "";

            try
            {
                Clef currentClef = null;
                int previousOctave = 4;
                char previousNote = 'c';

                LilypondToken currentToken = tokens.First();
                while (currentToken != null)
                {
                    switch (currentToken.TokenKind)
                    {
                        case LilypondTokenKind.Unknown:
                            break;
                        case LilypondTokenKind.Note:
                            // Length
                            int noteLength = Int32.Parse(Regex.Match(currentToken.Value, @"\d+").Value);
                            // Crosses and Moles
                            int alter = 0;
                            alter += Regex.Matches(currentToken.Value, "is").Count;
                            alter -= Regex.Matches(currentToken.Value, "es|as").Count;
                            // Octaves
                            int distanceWithPreviousNote = notesorder.IndexOf(currentToken.Value[0]) - notesorder.IndexOf(previousNote);
                            if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
                            {
                                distanceWithPreviousNote -= 7; // The number of notes in an octave
                            }
                            else if (distanceWithPreviousNote < -3)
                            {
                                distanceWithPreviousNote += 7; // The number of notes in an octave
                            }

                            if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) >= 7)
                            {
                                previousOctave++;
                            }
                            else if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) < 0)
                            {
                                previousOctave--;
                            }

                            // Force up or down.
                            previousOctave += currentToken.Value.Count(c => c == '\'');
                            previousOctave -= currentToken.Value.Count(c => c == ',');

                            previousNote = currentToken.Value[0];

                            var note = new PSAMControlLibrary.Note(currentToken.Value[0].ToString().ToUpper(), alter, previousOctave, (MusicalSymbolDuration)noteLength, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
                            note.NumberOfDots += currentToken.Value.Count(c => c.Equals('.'));

                            symbols.Add(note);
                            break;
                        case LilypondTokenKind.Rest:
                            var restLength = Int32.Parse(currentToken.Value[1].ToString());
                            symbols.Add(new Rest((MusicalSymbolDuration)restLength));
                            break;
                        case LilypondTokenKind.Bar:
                            symbols.Add(new Barline());
                            break;
                        case LilypondTokenKind.Clef:
                            currentToken = currentToken.NextToken;
                            if (currentToken.Value == "treble")
                                currentClef = new Clef(ClefType.GClef, 2);
                            else if (currentToken.Value == "bass")
                                currentClef = new Clef(ClefType.FClef, 4);
                            else
                                throw new NotSupportedException($"Clef {currentToken.Value} is not supported.");

                            symbols.Add(currentClef);
                            break;
                        case LilypondTokenKind.Time:
                            currentToken = currentToken.NextToken;
                            var times = currentToken.Value.Split('/');
                            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0]), UInt32.Parse(times[1])));
                            break;
                        case LilypondTokenKind.Tempo:
                            // Tempo not supported
                            break;
                        default:
                            break;
                    }
                    currentToken = currentToken.NextToken;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return symbols;
        }

        private static LinkedList<LilypondToken> GetTokensFromLilypond(string content)
        {
            var tokens = new LinkedList<LilypondToken>();

            foreach (string s in content.Split(' '))
            {
                LilypondToken token = new LilypondToken()
                {
                    Value = s
                };

                switch (s)
                {
                    case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
                    case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
                    case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
                    case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
                    case "|": token.TokenKind = LilypondTokenKind.Bar; break;
                    default: token.TokenKind = LilypondTokenKind.Unknown; break;
                }

                token.Value = s;

                if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Note;
                }
                else if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Rest;
                }

                if (tokens.Last != null)
                {
                    tokens.Last.Value.NextToken = token;
                    token.PreviousToken = tokens.Last.Value;
                }

                tokens.AddLast(token);
            }

            return tokens;
        }
        #endregion Staffs loading

        #region Saving to files
        internal void SaveToMidi(string fileName)
        {
            Sequence sequence = GetSequenceFromWPFStaffs();

            sequence.Save(fileName);
        }

        // temporary adapter for refactoring purposes, needs to be refactored and fixed together with "GetSequenceFromWPFStaffs"
        private class WPFFilehandlerAdapter
        {
            private FileHandler handler;

            public WPFFilehandlerAdapter(FileHandler handler)
            {
                this.handler = handler;
            }

            public int GetBpm()
            {
                return handler.staff.bpm;
            }

            public int GetBeatNote()
            {
                return handler.staff.timeSignature.Item1;
            }

            public int GetBeatsPerMeasure()
            {
                return handler.staff.timeSignature.Item2;
            }
        }

        private Sequence GetSequenceFromWPFStaffs()
        {
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };

            WPFFilehandlerAdapter fhAdapter = new WPFFilehandlerAdapter(this);

            int absoluteTicks = 0;

            Sequence sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);

            // Calculate tempo
            int speed = DPA_GLOBAL_CONSTANTS.MINUTE_IN_MICROSECONDS / fhAdapter.GetBpm();
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

            Track notesTrack = new Track();
            sequence.Add(notesTrack);

            foreach (MusicalSymbol musicalSymbol in WPFStaffs)
            {
                switch (musicalSymbol.Type)
                {
                    case MusicalSymbolType.Note:
                        PSAMControlLibrary.Note note = musicalSymbol as PSAMControlLibrary.Note;

                        // Calculate duration
                        double absoluteLength = 1.0 / (double)note.Duration;
                        absoluteLength += (absoluteLength / 2.0) * note.NumberOfDots;

                        double relationToQuartNote = fhAdapter.GetBeatNote() / 4.0;
                        double percentageOfBeatNote = (1.0 / fhAdapter.GetBeatNote()) / absoluteLength;
                        double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                        // Calculate height
                        int noteHeight = notesOrderWithCrosses.IndexOf(note.Step.ToLower()) + ((note.Octave + 1) * 12);
                        noteHeight += note.Alter;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                        absoluteTicks += (int)deltaTicks;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume

                        break;
                    case MusicalSymbolType.TimeSignature:
                        byte[] timeSignature = new byte[4];
                        timeSignature[0] = (byte)fhAdapter.GetBeatsPerMeasure();
                        timeSignature[1] = (byte)(Math.Log(fhAdapter.GetBeatNote()) / Math.Log(2));
                        metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
                        break;
                    default:
                        break;
                }
            }

            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            return sequence;
        }

        internal void SaveToPDF(string fileName)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string tmpFileName = $"{fileName}-tmp.ly";
            SaveToLilypond(tmpFileName);

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited)
            { /* Wait for exit */
            }
            if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }

        internal void SaveToLilypond(string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write(LilypondText);
                outputFile.Close();
            }
        }
        #endregion Saving to files
    }
}
