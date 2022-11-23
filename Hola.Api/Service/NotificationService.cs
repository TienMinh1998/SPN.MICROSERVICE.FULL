using FirebaseAdmin.Messaging;
using Hola.Api.Models;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static Hola.Core.Common.Constants;

namespace Hola.Api.Service
{
    public class NotificationService : BaseService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.MESSAGE_DB;

        public NotificationService(IOptions<SettingModel> setting) : base(setting)
        {
            _options = setting;
        }

        public async void UpdateCurrencyStatus(List<string> recipientIds)
        {
            PingService.CurrencyChanged(new List<string>
            {
                recipientIds.ToString()
            });
            APICrossHelper aPICrossHelper = new APICrossHelper();
            var dataSendFireBase = new SendFireBaseDataRequest
            {
                RecipientIds = recipientIds,
                Event = Constants.FireBase.EventCurrencyChanged
            };
            await aPICrossHelper.Post(_options.Value.CommonServiceUrl + "/FireBase/sendData",
                dataSendFireBase);
        }

        public async void UpdateUserStatus(List<string> recipientIds, string type)
        {
            PingService.UserStatusChanged(new List<string>
            {
                recipientIds.ToString()
            });
            APICrossHelper aPICrossHelper = new APICrossHelper();
            var dataSendFireBase = new SendFireBaseDataRequest();
            switch (type)
            {
                case "deactive":
                    dataSendFireBase = new SendFireBaseDataRequest
                    {
                        RecipientIds = recipientIds,
                        Event = Constants.FireBase.EventUserStatusDeactive
                    };
                    break;

                case "reactive":
                    dataSendFireBase = new SendFireBaseDataRequest
                    {
                        RecipientIds = recipientIds,
                        Event = Constants.FireBase.EventUserStatusReactive
                    };
                    break;
                default:
                    dataSendFireBase = new SendFireBaseDataRequest
                    {
                        RecipientIds = recipientIds,
                        Event = Constants.FireBase.EventUserStatusUnlock
                    };
                    break;
            }
            FireBaseService fireBaseService = new FireBaseService(_options);
            await fireBaseService.SendUserStatus(dataSendFireBase);
        }

        public DataResultsModel Search(NotificationFilterModel model, ClaimsPrincipal User)
        {
            var result = new DataResultsModel();
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            string conditions = string.Empty;
            string orderBy = " ORDER BY n.\"CreatedDate\" DESC ";
            string paging = string.Empty;

            if (model.FromDate != null)
            {
                conditions += String.Format(" AND n.\"CreatedDate\" >= '{0}' ", model.FromDate.ToString());
            }
            if (model.ToDate != null)
            {
                conditions += String.Format(" AND n.\"CreatedDate\" <= '{0}' ", model.ToDate.ToString());
            }
            if (model.TitleKey != null)
            {
                conditions += String.Format(" AND n.\"TitleKey\" IN ( '{0}' ) ", string.Join("', '", model.TitleKey));
            }

            string sqlCount = string.Format("SELECT COUNT(1) AS \"totalCount\", SUM(CASE WHEN n.\"IsRead\" = CAST(0 AS BIT) THEN 1 ELSE 0 END) AS \"unreadCount\" " +
                                             "FROM msg.\"Notification\" n " +
                                             "WHERE n.\"IsDeleted\" = CAST(0 AS BIT) AND n.\"RecipientId\" = '{0}' {1} ", userId, conditions);

            string sqlUnReadCount = string.Format("SELECT COUNT(1) AS \"totalCount\", SUM(CASE WHEN n.\"IsRead\" = CAST(0 AS BIT) THEN 1 ELSE 0 END) AS \"unreadCount\" " +
                                            "FROM msg.\"Notification\" n " +
                                            "WHERE n.\"IsDeleted\" = CAST(0 AS BIT) AND n.\"RecipientId\" = '{0}' ", userId);

            var take = !model.PageSize.HasValue || model.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + model.PageSize.Value + " ROWS ONLY";
            paging += " OFFSET " + ((model.PageNumber ?? 1) - 1) * (model.PageSize ?? 10) + " ROWS " + take;

            string sqlSearch = string.Format(" SELECT n.\"NotificationId\", " +
                                            "n.\"TransactionId\", " +
                                            "n.\"CreatedDate\", " +
                                            "n.\"TransactionType\", " +
                                            "n.\"TransactionStatus\", " +
                                            "n.\"LanguageId\", " +
                                            "n.\"TitleKey\", " +
                                            "n.\"Title\", " +
                                            "n.\"Text\" AS \"Html\", " +
                                            "n.\"IsRead\" AS \"Read\", " +
                                            "n.\"TransactionType\", " +
                                            "n.\"NotificationKey\", " +
                                            "n.\"OfferType\"," +
                                            "n.\"OfferExchangeId\" " +
                                            "FROM msg.\"Notification\" n " +
                                            "WHERE n.\"IsDeleted\" = CAST(0 AS BIT) AND n.\"RecipientId\" = '{0}' {1} {2} {3}", userId, conditions, orderBy, paging);

            var obj = DatabaseHelper.ExcuteSql(sqlCount, setting).Rows;
            var obj1 = DatabaseHelper.ExcuteSql(sqlUnReadCount, setting).Rows;
            int totalCount = 0;
            int totalUnread = 0;
            if (obj != null && obj.Count > 0)
            {
                totalCount = int.Parse(obj[0]["totalCount"].ToString());
            }
            if (obj1 != null && obj1.Count > 0)
            {
                if (!string.IsNullOrEmpty(obj1[0]["unreadCount"].ToString()))
                {
                    totalUnread = int.Parse(obj1[0]["unreadCount"].ToString());
                }
            }
            var allNotification = QueryToList<NotificationResponseModel>(rawConnection + Constant.MESSAGE_DB, sqlSearch);
            //var allNotification = DatabaseHelper.ExcuteQueryToList<NotificationResponseModel>(sqlSearch, setting);

            // need refactor get actions from db(InternalActions)
            foreach (var item in allNotification)
            {
                item.Actions = new List<string>();
                if (item.NotificationKey == MessagingConstants.Texts.TransactionCancelled
                    || item.NotificationKey == MessagingConstants.Texts.TransactionExpired
                    || item.NotificationKey == MessagingConstants.Texts.TransactionCompleted
                    || item.NotificationKey == MessagingConstants.Texts.AdminDeducted
                    || item.NotificationKey == MessagingConstants.Texts.AdminAdded
                    || item.NotificationKey == MessagingConstants.Texts.BuyWarning
                    || item.TitleKey == MessagingConstants.Types.NoteTransfer)
                    item.Actions.Add(MessagingConstants.Actions.ContactSupport);
                else if (item.NotificationKey == MessagingConstants.Texts.BecomeAgent)
                    item.Actions.Add(MessagingConstants.Actions.ProceedLogout);
            }
            result.Items = allNotification;
            result.TotalCount = totalCount;
            result.UnreadCount = totalUnread;

            return result;
        }

        /// <summary>
        /// LVTan
        /// Check Exists Notification
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool CheckExists(long id, string title)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = "SELECT COUNT(*) FROM msg.\"Notification\" n";

            if (id > 0)
            {
                sql += String.Format("WHERE n.\"IsDeleted\" = '0' AND (CASE WHEN (n.\"NotificationId\" != {0} AND Trim(n.\"Title\") = Trim({1})) THEN 1 ELSE 0 END = 1)", id, title);
            }
            else
            {
                sql += String.Format("WHERE n.\"IsDeleted\" = '0' AND (CASE WHEN (Trim(n.\"Title\") = Trim({0})) THEN 1 ELSE 0 END = 1)", title);
            }

            setting.Connection += "Database=" + database;

            int result = DatabaseHelper.ExcuteQueryToList<int>(sql, setting).FirstOrDefault();

            return result > 0;
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> Create(SendNotificationRequest request)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            //get unread count from ping
            List<string> listUserId = new List<string>();
            listUserId.Add(request.RecipientId);
            var noteCounts = PingService.NoteCounts(listUserId);
            var respNoteCount = noteCounts[0];

            //set unread count to req
            request.UnreadNoteCount = respNoteCount + 1;

            //get Sender and recipenter info
            var usr = await GetUserInfo(request.SenderId);
            var recusr = await GetUserInfo(request.RecipientId);
            if (usr == null)
            {
                usr = new NotificationUserModel();
            }
            if (recusr == null)
            {
                return -1;
            }

            //Get message and title of notification
            var noteText = GetNotificationText(request.NotificationKey, recusr.LanguageId ?? 2);
            var noteTitle = await GetNotificationTitle(request.TitleKey, recusr.LanguageId ?? 2);
            if (noteText == null)
            {
                return -1;
            }

            //replace text of notification
            ReplacePlaceHolders(noteText, usr, request.Amount, request.CurrencySymbol, request.MaskPhoneNumber);

            //inser into table notification of db msg
            var sql = String.Format("INSERT INTO msg.\"Notification\" " +
                "(\"SenderId\", " +
                "\"RecipientId\", " +
                "\"TitleKey\", " +
                "\"TransactionId\", " +
                "\"TransactionType\", " +
                "\"Text\", " +
                "\"IsRead\", " +
                "\"CreatedDate\", " +
                "\"NotificationKey\", " +
                "\"OfferType\", " +
                "\"Title\" ) " +
                "VALUES " +
                "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', CAST(0 AS BIT), '{6}', '{7}', '{8}', '{9}' ) RETURNING \"NotificationId\"; ",
                request.SenderId, request.RecipientId, request.TitleKey, request.TransactionId,
                request.TransactionType, noteText.Text.Replace("'", "''"), DateTime.UtcNow, request.NotificationKey, request.OfferType, noteTitle);

            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            int newId = -1;
            if (obj != null && obj.Count > 0)
            {
                newId = int.Parse(obj[0]["NotificationId"].ToString());
            }

            try
            {
                //Push notification to firebase
                FireBaseService fireBaseService = new FireBaseService(_options);
                string token = fireBaseService.getToken(request.RecipientId);
                var events = new List<string>();
                if (request.Event != null)
                {
                    events.Add(request.Event);
                }

                PushNotificationModel dto = new PushNotificationModel
                {
                    TitleKey = request.TitleKey,
                    RecipientId = request.RecipientId,
                    TransactionId = request.TransactionId,
                    TransactionType = request.TransactionType,
                    Title = noteTitle,
                    NotificationId = newId,
                    Read = false,
                    CreatedDate = DateTime.UtcNow,
                    Html = noteText.Text,
                    OfferType = request.OfferType,
                    Amount = request.Amount,
                    CurrencySymbol = request.CurrencySymbol,
                    NotificationCount = request.UnreadNoteCount,
                    CurrencyId = request.CurrencyId,
                    Events = events
                };
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string dtoJson = System.Text.Json.JsonSerializer.Serialize(dto, serializeOptions);

                var msg = new Message
                {
                    Notification = new Notification
                    {
                        Title = noteTitle,
                        Body = noteText.PushText
                    },
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = request.UnreadNoteCount } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", Constants.FireBase.EventNoteCount},
                        { "dto", dtoJson},
                    },
                };

                fireBaseService.SendFirebase(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return newId;
        }

        public async Task<int> CreateNotiForKyc(SendKycNotificationRequest request)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            //inser into table notification of db msg
            var sql = String.Format("INSERT INTO msg.\"Notification\" " +
                "(\"SenderId\", " +
                "\"RecipientId\", " +
                "\"TitleKey\", " +
                "\"Text\", " +
                "\"IsRead\", " +
                "\"CreatedDate\", " +
                "\"NotificationKey\", " +
                "\"Title\" ) " +
                "VALUES " +
                "('{0}', '{1}', '{2}', '{3}', CAST(0 AS BIT), '{4}', '{5}', '{6}' ) RETURNING \"NotificationId\"; ",
                request.SenderId, request.RecipientId, request.TitleKey, request.NoteText.Replace("'", "''"), DateTime.UtcNow, request.NotificationKey, request.NoteTitle);

            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            int newId = -1;
            if (obj != null && obj.Count > 0)
            {
                newId = int.Parse(obj[0]["NotificationId"].ToString());
            }
            return newId;
        }
        public async Task<int> CreateNotiForP2P(SendNotificationP2PRequest request)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            //get unread count from ping
            List<string> listUserId = new List<string>();
            listUserId.Add(request.RecipientId);
            var noteCounts = PingService.NoteCounts(listUserId);
            var respNoteCount = noteCounts[0];

            //set unread count to req
            request.UnreadNoteCount = respNoteCount + 1;

            //get Sender and recipenter info
            var usr = await GetUserInfo(request.SenderId);
            var recusr = await GetUserInfo(request.RecipientId);
            if (usr == null)
            {
                usr = new NotificationUserModel();
            }
            if (recusr == null)
            {
                return -1;
            }

            //Get message and title of notification
            var noteText = GetNotificationText(request.NotificationKey, recusr.LanguageId ?? 2);
            var noteTitle = await GetNotificationTitle(request.TitleKey, recusr.LanguageId ?? 2);
            if (noteText == null)
            {
                return -1;
            }

            //replace text of notification
            ReplacePlaceHolders(noteText, usr, request.Amount, request.CurrencySymbol, request.MaskPhoneNumber);

            //inser into table notification of db msg
            var sql = String.Format("INSERT INTO msg.\"Notification\" " +
                "(\"SenderId\", " +
                "\"RecipientId\", " +
                "\"TitleKey\", " +
                "\"TransactionId\", " +
                "\"TransactionType\", " +
                "\"Text\", " +
                "\"IsRead\", " +
                "\"CreatedDate\", " +
                "\"NotificationKey\", " +
                "\"OfferType\", " +
                "\"Title\"," +
                "\"OfferExchangeId\" ) " +
                "VALUES " +
                "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', CAST(0 AS BIT), '{6}', '{7}', '{8}', '{9}',{10} ) RETURNING \"NotificationId\"; ",
                request.SenderId, request.RecipientId, request.TitleKey, request.TransactionId,
                request.TransactionType, noteText.Text, DateTime.UtcNow, request.NotificationKey, request.Status, noteTitle, request.OfferExchangeId);

            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            int newId = -1;
            if (obj != null && obj.Count > 0)
            {
                newId = int.Parse(obj[0]["NotificationId"].ToString());
            }

            try
            {
                //Push notification to firebase
                FireBaseService fireBaseService = new FireBaseService(_options);
                string token = fireBaseService.getToken(request.RecipientId);
                var events = new List<string>();
                if (request.Event != null)
                {
                    events.Add(request.Event);
                }

                PushNotificationP2PModel dto = new PushNotificationP2PModel
                {
                    TitleKey = request.TitleKey,
                    RecipientId = request.RecipientId,
                    TransactionId = request.TransactionId,
                    TransactionType = request.TransactionType,
                    OfferExchangeId = request.OfferExchangeId,
                    Title = noteTitle,
                    NotificationId = newId,
                    Read = false,
                    CreatedDate = DateTime.UtcNow,
                    Html = noteText.Text,
                    OfferType = request.Status.ToString(),
                    Amount = request.Amount,
                    CurrencySymbol = request.CurrencySymbol,
                    NotificationCount = request.UnreadNoteCount,
                    CurrencyId = request.CurrencyId,
                    Events = events
                };
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string dtoJson = System.Text.Json.JsonSerializer.Serialize(dto, serializeOptions);

                var msg = new Message
                {
                    Notification = new Notification
                    {
                        Title = noteTitle,
                        Body = noteText.PushText
                    },
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = request.UnreadNoteCount } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", Constants.FireBase.EventNoteCount},
                        { "dto", dtoJson},
                    },
                };

                fireBaseService.SendFirebase(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return newId;
        }

        public async Task<List<int>> CreateMultiple(SendMultiNotificationRequest request)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var rs = new List<int>();
            //get unread count from ping
            List<string> listUserId = request.RecipientId;

            var noteCounts = PingService.NoteCounts(listUserId);
            var respNoteCount = noteCounts[0];

            //set unread count to req
            request.UnreadNoteCount = respNoteCount + 1;

            //get Sender and recipenter info
            var usr = await GetUserInfo(request.SenderId);
            foreach (var item in listUserId)
            {
                var recusr = await GetUserInfo(item);
                if (usr == null)
                {
                    usr = new NotificationUserModel();
                }
                if (recusr == null)
                {
                    return null;
                }

                //Get message and title of notification
                var noteText = GetNotificationText(request.NotificationKey, recusr.LanguageId ?? 2);
                var noteTitle = await GetNotificationTitle(request.TitleKey, recusr.LanguageId ?? 2);
                if (noteText == null)
                {
                    return null;
                }
                //replace text of notification
                ReplacePlaceHolders(noteText, usr, request.Amount, request.CurrencySymbol, request.MaskPhoneNumber);
                //inser into table notification of db msg
                var sql = String.Format("INSERT INTO msg.\"Notification\" " +
                    "(\"SenderId\", " +
                    "\"RecipientId\", " +
                    "\"TitleKey\", " +
                    "\"TransactionId\", " +
                    "\"TransactionType\", " +
                    "\"Text\", " +
                    "\"IsRead\", " +
                    "\"CreatedDate\", " +
                    "\"NotificationKey\", " +
                    "\"OfferType\", " +
                    "\"Title\" ) " +
                    "VALUES " +
                    "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', CAST(0 AS BIT), '{6}', '{7}', '{8}', '{9}' ) RETURNING \"NotificationId\"; ",
                    request.SenderId, item, request.TitleKey, request.TransactionId,
                    request.TransactionType, noteText.Text, DateTime.UtcNow, request.NotificationKey, request.OfferType, noteTitle);

                var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
                int newId = -1;
                if (obj != null && obj.Count > 0)
                {
                    newId = int.Parse(obj[0]["NotificationId"].ToString());
                }

                try
                {
                    //Push notification to firebase
                    FireBaseService fireBaseService = new FireBaseService(_options);
                    string token = fireBaseService.getToken(item);
                    var events = new List<string>();
                    events.Add(Constants.FireBase.EventNoteCount);

                    PushNotificationModel dto = new PushNotificationModel
                    {
                        TitleKey = request.TitleKey,
                        RecipientId = item,
                        TransactionId = request.TransactionId,
                        TransactionType = request.TransactionType,
                        Title = noteTitle,
                        NotificationId = newId,
                        Read = false,
                        CreatedDate = DateTime.UtcNow,
                        Html = noteText.Text,
                        OfferType = request.OfferType,
                        Amount = request.Amount,
                        CurrencySymbol = request.CurrencySymbol,
                        NotificationCount = request.UnreadNoteCount,
                        CurrencyId = request.CurrencyId,
                        Events = events
                    };
                    var serializeOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };
                    string dtoJson = System.Text.Json.JsonSerializer.Serialize(dto, serializeOptions);

                    var msg = new Message
                    {
                        Notification = new Notification
                        {
                            Title = noteTitle,
                            Body = noteText.PushText
                        },
                        Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = request.UnreadNoteCount } },

                        Token = token,
                        Data = new Dictionary<string, string>()
                    {
                        { "event", Constants.FireBase.EventNoteCount},
                        { "dto", dtoJson},
                    },
                    };

                    fireBaseService.SendFirebase(msg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                rs.Add(newId);
            }
            return rs;
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="request"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public bool SetAsRead(SetNotificationAsReadRequest request, ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;

            var sqlRead = string.Format("UPDATE msg.\"Notification\" SET \"IsRead\" = CAST(1 AS BIT) WHERE \"RecipientId\" = '{0}' AND \"NotificationId\" IN ( {1} )",
                userId,
                string.Join(", ", request.NotificationIds));

            int result = DatabaseHelper.ExcuteNonQuery(sqlRead, setting);

            return result >= 1;
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <returns></returns>
        public bool SetAllAsRead(ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;

            var sqlRead = string.Format("UPDATE msg.\"Notification\" SET \"IsRead\" = CAST(1 AS BIT) WHERE \"RecipientId\" = '{0}' ", userId);

            int result = DatabaseHelper.ExcuteNonQuery(sqlRead, setting);

            return result >= 1;
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <param name="currSymbol"></param>
        /// <param name="maskPhoneNumber"></param>
        private void ReplacePlaceHolders(NotificationTextModel model, NotificationUserModel user, decimal? amount, string currSymbol, bool maskPhoneNumber)
        {
            if (model == null)
                return;
            if (amount.HasValue)
            {
                var camount = $"{currSymbol}{string.Format("{0:n0}", amount)}";
                model.Text = model.Text.Replace("$%amount%$", camount);
                if (!string.IsNullOrEmpty(model.PushText))
                    model.PushText = model.PushText.Replace("$%amount%$", camount);
            }

            var phone = string.IsNullOrEmpty(user.Phone) ? "***NAN" : (maskPhoneNumber ? formatPhone(user.Phone, 2) : user.Phone);
            var holder = string.IsNullOrEmpty(user.CardHolderName) ? "NO HOLDER NAME" : user.CardHolderName.ToUpper();

            model.Text = model.Text.Replace("$%phone%$", phone);
            model.Text = model.Text.Replace("$%holder%$", holder);
            if (!string.IsNullOrEmpty(model.PushText))
            {
                model.PushText = model.PushText.Replace("$%phone%$", phone);
                model.PushText = model.PushText.Replace("$%holder%$", holder);
            }
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lastCharCount"></param>
        /// <returns></returns>
        private string formatPhone(string source, int lastCharCount)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            if (lastCharCount >= source.Length)
                return "";

            string phoneCode = CountryPhoneCodes.AllPhoneCodes.FirstOrDefault(c => source.StartsWith(c)) ?? "";
            string asterisks = new string('*', source.Length - lastCharCount - phoneCode.Length);
            string lastVisibleDigits = source.Substring(source.Length - lastCharCount);

            return $"{phoneCode}{asterisks}{lastVisibleDigits}";
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<NotificationUserModel> GetUserInfo(string userId)
        {
            using HttpClient client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{_options.Value.UserServiceUrl}/User/GetUserForNoti?userId=" + userId);

            using HttpResponseMessage response = await client.SendAsync(httpRequest);
            using HttpContent content = response.Content;
            string json = await content.ReadAsStringAsync();
            if (json.Length > 0)
            {
                JObject jsonOb = JObject.Parse(json);
                NotificationUserModel userModel = jsonOb["data"].ToObject<NotificationUserModel>();
                return userModel;
            }

            return null;
        }

        public async Task<NotificationUserModel> GetUserInfoForceLogout(string userId)
        {
            APICrossHelper aPICrossHelper = new APICrossHelper();
            var result = await aPICrossHelper.Get<JsonResponseModel>(_options.Value.UserServiceUrl + "/User/GetUserForNoti?userId=" + userId);
            NotificationUserModel notificationUserModel = new NotificationUserModel();
            if (result.Data != null)
            {
                notificationUserModel = Newtonsoft.Json.JsonConvert.DeserializeObject<NotificationUserModel>(result.Data.ToString());
                return notificationUserModel;
            }
            return null;
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lngId"></param>
        /// <returns></returns>
        private NotificationTextModel GetNotificationText(string key, short lngId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            var sqlCommand = string.Format("SELECT \"Id\", " +
                "\"LanguageId\", " +
                "\"NotificationKey\", " +
                "\"Text\", \"PushText\"," +
                " \"PushFireBase\", " +
                "\"Recipient\", " +
                "\"Description\", " +
                "\"HasAction\"" +
                " FROM msg.\"NotificationText\" n WHERE n.\"NotificationKey\" = '{0}' AND n.\"LanguageId\" = {1}", key, lngId);

            return DatabaseHelper.ExcuteQueryToList<NotificationTextModel>(sqlCommand, setting).FirstOrDefault();
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="titleKey"></param>
        /// <param name="lngId"></param>
        /// <returns></returns>
        public async Task<string> GetNotificationTitle(string titleKey, short lngId)
        {
            using HttpClient client = new HttpClient();

            string url = string.Format("{0}/LanguageText/GetTextByKey?key={1}&languageId={2}", _options.Value.ConfigServiceUrl, titleKey, lngId);

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            using HttpResponseMessage response = await client.SendAsync(httpRequest);
            using HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            if (json.Length > 0)
            {
                JObject jsonOb = JObject.Parse(json);
                var string1 = (string)jsonOb["data"];
                return string1;
            }

            return titleKey;
        }

        public int GetCountUnread(ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            string userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;

            string sql = string.Format("SELECT COUNT(*) as \"TotalCount\" FROM msg.\"Notification\" WHERE \"RecipientId\" = '{0}' AND \"IsRead\" = '0';", userId);

            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;

            int totalCount = 0;

            if (obj != null && obj.Count > 0)
            {
                totalCount = int.Parse(obj[0]["TotalCount"].ToString());
            }

            return totalCount;
        }
    }
}