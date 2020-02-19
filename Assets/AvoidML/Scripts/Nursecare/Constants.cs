namespace AvoidML.Nursecare
{
    public static class Constants
    {
        public static readonly float timeFrequency = 100f; // data per seconds
        public static readonly bool hasHeader = true;
        public static readonly float convert2meter = 1f / 1000f;

        public static readonly float timeInterval = 1f / timeFrequency; // second per data
    }
}
