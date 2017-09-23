namespace DPA_Musicsheets.Models
{
    class Maat
    {
        public Maat first;
        public Maat last;

        public Maat vorige;
        public Maat volgende;
        int duration;
        public Noot vorigeNoot;
        public Noot volgendeNoot;

        public Maat(int duration) {
            this.duration = duration;
        }
    }
}
