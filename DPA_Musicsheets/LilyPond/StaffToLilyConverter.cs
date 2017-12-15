using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.LilyPond
{
    public class StaffToLilyConverter
    {
        StringBuilder LilyString;

        public StaffToLilyConverter()
        {
            LilyString = new StringBuilder();
        }

        public string Convert(Staff staff)
        {
            _Convert(staff);

            return LilyString.ToString();
        }

        private void _Convert(MusicalComponent mc)
        {
            switch (mc.ComponentName)
            {
                case "Staff":
                    _ConvertStaff((Staff) mc);
                    break;
                case "Measure":
                    _ConvertMeasure((Measure) mc);
                    break;
                case "Note":
                    _ConvertNote((Note) mc);
                    break;
                default:
                    throw new Exception("Unexpected musical component during staff to lily conversion");
            }
        }

        private void _ConvertNote(Note note)
        {
            LilyNoteInformation lily = (LilyNoteInformation) note.GetExtensionByName("LilyNoteInformation");
            StringBuilder noteString = new StringBuilder();

            noteString.Append(note.name);
            noteString.Append(new string(',', lily.amountOfCommas));
            noteString.Append(new string('\'', lily.amountOfApostrophes));
            noteString.Append(note.duration);
            noteString.Append(new string('.', lily.amountOfDots));
            noteString.Append(" ");

            LilyString.Append(noteString);
        }

        private void _ConvertMeasure(Measure measure)
        {
            foreach (MusicalComponent mcChild in measure.children)
            {
                _Convert(mcChild);
            }

            if (measure.children.Count() > 0)
            {
                LilyString.AppendLine("|");
            }
        }

        private void _ConvertStaff(Staff staff)
        {
            LilyString.AppendLine("\\relative c' {");
            LilyString.AppendLine("\\clef treble");

            LilyString.AppendLine($"\\time {staff.timeSignature.Item1}/{staff.timeSignature.Item2}");

            LilyString.AppendLine($"\\tempo 4={staff.bpm}");

            foreach (MusicalComponent mcChild in staff.children)
            {
                _Convert(mcChild);
            }

            LilyString.AppendLine("}");
        }
    }
}
