namespace DPA_Musicsheets.Builders
{
    public interface IMidiLilyBuilder
    {
        void AddBar();
        void AddCustom(string wildcard);
        void AddDefaultConfiguration();
        void AddNote(string note);
        void AddNoteDuration(int duration, int amountOfDots);
        void AddNoteLength(string noteLength);
        void AddNoteSeparator();
        void AddTempo(int bpm);
        void AddTimeSignature(int beatNote, int beatsPerBar);
        string Build();
        void CloseScope();
        void OpenScope();
    }
}