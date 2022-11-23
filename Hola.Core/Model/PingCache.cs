using System;
using System.Collections.Generic;
using System.Linq;

namespace Hola.Core.Model
{
    /// <summary>
    /// nthai3
    /// </summary>
    internal static class PingCache
    {
        #region Private Fields
        private static readonly object LockerBalance = new object();
        private static readonly object LockerRecent = new object();
        private static readonly object LockerNote = new object();
        private static readonly object LockerAnnouncement = new object();
        private static readonly object LockedFeature = new object();
        private static readonly object LockerApplyStoreStatusChanged = new object();
        private static readonly object LockerApplyAgentStatusChanged = new object();
        private static readonly object LockerCurrencyChanged = new object();
        private static readonly object LockerUserStatusChanged = new object();

        private static readonly HashSet<string> _balanceChange = new HashSet<string>();
        private static readonly HashSet<string> _recentChange = new HashSet<string>();
        private static readonly Dictionary<string, HashSet<int>> _noteCount = new Dictionary<string, HashSet<int>>();
        private static readonly Dictionary<string, HashSet<long>> _announcementCount = new Dictionary<string, HashSet<long>>();
        private static readonly Dictionary<string, HashSet<long>> _featureCount = new Dictionary<string, HashSet<long>>();
        private static readonly HashSet<string> _applyStoreStatusChanged = new HashSet<string>();
        private static readonly HashSet<string> _applyAgentStatusChanged = new HashSet<string>();
        private static readonly HashSet<string> _currencyChanged = new HashSet<string>();
        private static readonly HashSet<string> _userStatusChanged = new HashSet<string>();
        #endregion Private Fields

        #region Balance
        internal static bool IsBalanceChange(string userId)
        {
            lock (LockerBalance)
            {
                var res = _balanceChange.Contains(userId);
                _balanceChange.Remove(userId);
                return res;
            }
        }


        internal static void BalanceChangeed(List<string> userIds)
        {
            lock (LockerBalance)
            {
                foreach (var userId in userIds)
                {
                    if (!_balanceChange.Contains(userId))
                        _balanceChange.Add(userId);
                }
            }
        }
        #endregion Balance
        #region Recent
        internal static bool IsRecentChange(string userId)
        {
            lock (LockerRecent)
            {
                var res = _recentChange.Contains(userId);
                _recentChange.Remove(userId);
                return res;
            }
        }



        internal static void RecentChanged(List<string> userIds)
        {
            lock (LockerRecent)
            {
                foreach (var userId in userIds)
                {
                    if (!_recentChange.Contains(userId))
                        _recentChange.Add(userId);
                }
            }
        }
        #endregion

        #region Note
        internal static int NoteCount(string userId)
        {
            lock (LockerNote)
            {
                if (!_noteCount.ContainsKey(userId))
                    return 0;
                return _noteCount[userId].Count;
            }
        }
        internal static List<int> NoteCounts(List<string> userIds)
        {
            lock (LockerNote)
            {
                var counts = new List<int>();
                foreach (var userId in userIds)
                {
                    if (!_noteCount.ContainsKey(userId))
                        counts.Add(0);
                    else counts.Add(_noteCount[userId].Count);
                }
                return counts;
            }
        }
        internal static void NewNotification(string userId, int noteId)
        {
            lock (LockerNote)
            {
                if (_noteCount.ContainsKey(userId))
                {
                    if (!_noteCount[userId].Contains(noteId))
                        _noteCount[userId].Add(noteId);
                }
                else
                    _noteCount.Add(userId, new HashSet<int> { noteId });
            }
        }
        internal static void RemoveNotification(string userId, List<int> noteIds)
        {
            lock (LockerNote)
            {
                if (_noteCount.ContainsKey(userId))
                {
                    foreach (var noteId in noteIds)
                        _noteCount[userId].Remove(noteId);
                    if (_noteCount[userId].Count == 0)
                        _noteCount.Remove(userId);
                }
            }
        }
        #endregion Note

        internal static void RemoveAllNotification(string userId)
        {
            lock (LockerNote)
            {
                if (_noteCount.ContainsKey(userId))
                {
                    _noteCount.Remove(userId);
                }
            }
        }

        #region Announcement
        internal static int AnnouncementCount(string userId)
        {
            lock (LockerAnnouncement)
            {
                if (!_announcementCount.ContainsKey(userId))
                    return 0;
                return _announcementCount[userId].Count;
            }
        }
        internal static void NewAnnouncement(List<KeyValuePair<string, long>> items)
        {
            lock (LockerAnnouncement)
            {
                foreach (var item in items)
                {
                    if (_announcementCount.ContainsKey(item.Key))
                    {
                        if (!_announcementCount[item.Key].Contains(item.Value))
                            _announcementCount[item.Key].Add(item.Value);
                    }
                    else
                        _announcementCount.Add(item.Key, new HashSet<long> { item.Value });
                }

            }
        }
      
        internal static void RemoveAnnouncement(string userId, List<long> announcementIds)
        {
            lock (LockerAnnouncement)
            {
                if (_announcementCount.ContainsKey(userId))
                {
                    foreach (var noteId in announcementIds)
                        _announcementCount[userId].Remove(noteId);
                    if (_announcementCount[userId].Count == 0)
                        _announcementCount.Remove(userId);
                }
            }
        }
        #endregion Announcement

        #region ApplyStoreStatusChanged
        internal static bool IsApplyStoreStatusChanged(string userId)
        {
            lock (LockerApplyStoreStatusChanged)
            {
                var res = _applyStoreStatusChanged.Contains(userId);
                _applyStoreStatusChanged.Remove(userId);
                return res;
            }
        }


        internal static void ApplyStoreStatusChanged(List<string> userIds)
        {
            lock (LockerApplyStoreStatusChanged)
            {
                foreach (var userId in userIds)
                {
                    if (!_applyStoreStatusChanged.Contains(userId))
                        _applyStoreStatusChanged.Add(userId);
                }
            }
        }
        #endregion ApplyStoreStatusChanged

        #region ApplyAgentStatusChanged
        internal static bool IsApplyAgentStatusChanged(string userId)
        {
            lock (LockerApplyAgentStatusChanged)
            {
                var res = _applyAgentStatusChanged.Contains(userId);
                _applyAgentStatusChanged.Remove(userId);
                return res;
            }
        }


        internal static void ApplyAgentStatusChanged(List<string> userIds)
        {
            lock (LockerApplyAgentStatusChanged)
            {
                foreach (var userId in userIds)
                {
                    if (!_applyAgentStatusChanged.Contains(userId))
                        _applyAgentStatusChanged.Add(userId);
                }
            }
        }
        #endregion ApplyAgentStatusChanged

        internal static bool IsCurrencyChanged(string userId)
        {
            lock (LockerCurrencyChanged)
            {
                var res = _currencyChanged.Contains(userId);
                _currencyChanged.Remove(userId);
                return res;
            }
        }
        internal static void CurrencyChanged(List<string> userIds)
        {
            lock (LockerCurrencyChanged)
            {
                foreach (var userId in userIds)
                {
                    if (!_currencyChanged.Contains(userId))
                        _currencyChanged.Add(userId);
                }
            }
        }
        internal static bool IsUserStatusChanged(string userId)
        {
            lock (LockerUserStatusChanged)
            {
                var res = _userStatusChanged.Contains(userId);
                _userStatusChanged.Remove(userId);
                return res;
            }
        }
        internal static void UserStatusChanged(List<string> userIds)
        {
            lock (LockerUserStatusChanged)
            {
                foreach (var userId in userIds)
                {
                    if (!_userStatusChanged.Contains(userId))
                        _userStatusChanged.Add(userId);
                }
            }
        }
        #region FeatureNotification
        internal static int FeatureCount(string userId)
        {
            lock (LockedFeature)
            {
                if (!_featureCount.ContainsKey(userId))
                    return 0;
                return _featureCount[userId].Count;
            }
        }
        internal static void NewFeature(List<KeyValuePair<string, long>> items)
        {
            lock (LockedFeature)
            {
                foreach (var item in items)
                {
                    if (_featureCount.ContainsKey(item.Key))
                    {
                        if (!_featureCount[item.Key].Contains(item.Value))
                            _featureCount[item.Key].Add(item.Value);
                    }
                    else
                        _featureCount.Add(item.Key, new HashSet<long> { item.Value });
                }

            }
        }

        internal static void RemoveFeature(string userId, List<long> announcementIds)
        {
            lock (LockedFeature)
            {
                if (_featureCount.ContainsKey(userId))
                {
                    foreach (var noteId in announcementIds)
                        _featureCount[userId].Remove(noteId);
                    if (_featureCount[userId].Count == 0)
                        _featureCount.Remove(userId);
                }
            }
        }
        #endregion
    }
}
