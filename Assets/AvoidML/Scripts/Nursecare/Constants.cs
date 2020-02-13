namespace AvoidML.Nursecare
{
    public static class Constants
    {
        public static readonly float timeFrequency = 100f; // data per seconds
        public static readonly float timeInterval = 1f / timeFrequency; // second per data
        public static readonly int positionCount = 29;
        public static readonly bool hasHeader = true;
    }
}
