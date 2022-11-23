using Hola.Core.Model;
using System.Collections.Generic;


namespace Hola.Core.Service
{
    /// <summary>
    /// nthai3
    /// </summary>
    public class PingService 
    {

      
        public static PingResponse Get(string userId)
        {
            return new PingResponse
            {
                BalanceChanged = PingCache.IsBalanceChange(userId),
                RecentActivityChanged = PingCache.IsRecentChange(userId),
                NoteCount = PingCache.NoteCount(userId),
                AnnouncementCount = PingCache.AnnouncementCount(userId),
                FeatureCount = PingCache.FeatureCount(userId),
                ApplyStoreStatusChanged = PingCache.IsApplyStoreStatusChanged(userId),
                ApplyAgentStatusChanged = PingCache.IsApplyAgentStatusChanged(userId),
                CurrencyChanged = PingCache.IsCurrencyChanged(userId),
            };
        }

        public static void UserBalanceChanged(List<string> userIds)
        {
            PingCache.BalanceChangeed(userIds);
        }
        public static void RecentActivityChanged(List<string> userIds)
        {
            PingCache.RecentChanged(userIds);
        }
        public static long AddNotificationForUser(string userId, int noteId)
        {
            PingCache.NewNotification(userId, noteId);
            return PingCache.NoteCount(userId);
        }
        public static int RemoveNotificationForUser(string userId, List<int> noteIds)
        {
            PingCache.RemoveNotification(userId, noteIds);
            return PingCache.NoteCount(userId);
        }

        public static int RemoveAllNotification(string userId)
        {
            PingCache.RemoveAllNotification(userId);
            return 0;
        }

        public static List<int> NoteCounts(List<string> userIds)
        {
            return PingCache.NoteCounts(userIds) ;
        }
        public static void AddAnnouncement(List<KeyValuePair<string, long>> items)   
        {
            PingCache.NewAnnouncement(items);
        }
        public static void RemoveAnnouncementForUser(string userId, List<long> announcementIds)
        {
            PingCache.RemoveAnnouncement(userId, announcementIds);
        }
        public static void AddFeature(List<KeyValuePair<string, long>> items)
        {
            PingCache.NewFeature(items);
        }
        public static void RemoveFeature(string userId, List<long> featureIds)
        {
            PingCache.RemoveFeature(userId, featureIds);
        }
        public static void ApplyStoreStatusChanged(List<string> userIds)
        {
            PingCache.ApplyStoreStatusChanged(userIds);
        }
        public static void ApplyAgentStatusChanged(List<string> userIds)
        {
            PingCache.ApplyAgentStatusChanged(userIds);
        }

        public static void CurrencyChanged(List<string> userIds)
        {
            PingCache.CurrencyChanged(userIds);
        }
        public static void UserStatusChanged(List<string> userIds)
        {
            PingCache.UserStatusChanged(userIds);
        }
    }
}
