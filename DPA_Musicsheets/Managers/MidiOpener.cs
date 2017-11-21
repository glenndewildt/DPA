using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Managers
{
    public class MidiOpener : AbstractFileOpener
    {
        private FileHandler _handler;

        public MidiOpener(FileHandler handler)
        {
            _handler = handler;
        }

        public override void Open(string fileName)
        {
            _handler.MidiSequence = new Sequence();
            _handler.MidiSequence.Load(fileName);
            _handler.HandleMidiOpen();
        }
    }
}
