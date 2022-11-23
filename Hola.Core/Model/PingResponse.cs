namespace Hola.Core.Model
{
    public class PingResponse
    {
        public bool BalanceChanged { get; set; }
        public int NoteCount { get; set; }
        public int AnnouncementCount { get; set; }
        public int FeatureCount { get; set; }
        public bool RecentActivityChanged { get; set; }
        public bool ApplyStoreStatusChanged { get; set; }
        public bool ApplyAgentStatusChanged { get; set; }
        public bool CurrencyChanged { get; set; }
    }
}
