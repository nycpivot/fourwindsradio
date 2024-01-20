namespace FourWindsRadio.Tools.Media
{
    internal class PlaylistItem
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string Filename { get; set; }
        public bool IsCopy { get; set; }
        public int Next { get; set; }
    }
}
