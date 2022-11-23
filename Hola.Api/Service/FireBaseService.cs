using System;
using System.Collections.Generic;
using System.IO;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using Hola.Api.Models;
using Hola.Core.Model.CommonModel;
using Hola.Core.Service;
using System.Data;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class FireBaseService
    {
        private static FirebaseApp _firebaseApp;
        private readonly IOptions<SettingModel> _options;
        private readonly ILogger<FireBaseService> _logger;
        private readonly string database = Constant.MESSAGE_DB;
        public FireBaseService(IOptions<SettingModel> setting)
        {
            _options = setting;

            if (_firebaseApp == null)
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
                });
            }
        }


        public void SendFirebase(Message msg)
        {
            var messaging = FirebaseMessaging.GetMessaging(_firebaseApp);
            msg.Android = new AndroidConfig { Priority = Priority.High };
            messaging.SendAsync(msg);
        }
        public async Task SendForceLogoutNoti(string recipientId)
        {
            var respNoteCount = PingService.NoteCounts(new List<string>() { recipientId }).FirstOrDefault();
            var listToken = GetAllTokenFireBaseByUserId(recipientId);
            NotificationService notificationService = new NotificationService(_options);
            var usr = await notificationService.GetUserInfo(recipientId);
            var notificationMessage = new Notification();
            notificationMessage = new Notification
            {
                Body = await notificationService.GetNotificationTitle("x_force_logout_body", usr.LanguageId.Value),
                Title = await notificationService.GetNotificationTitle("x_force_logout_title", usr.LanguageId.Value)
            };
            foreach (var item in listToken)
            {
                
                //Push notification to firebase
                var msg = new Message
                {

                    Notification = notificationMessage,
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount } },

                    Token = item,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", Constants.ForceLogoutHeaderKey},
                        { "dto", ""},
                    },
                };

                SendFirebase(msg);
            }

        }

        public async Task SendForceLogoutPinVerifyNoti(string recipientId)
        {
            var respNoteCount = PingService.NoteCounts(new List<string>() { recipientId }).FirstOrDefault();
            var listToken = GetAllTokenFireBaseByUserId(recipientId);
            NotificationService notificationService = new NotificationService(_options);
            var usr = await notificationService.GetUserInfoForceLogout(recipientId);
            var notificationMessage = new Notification();
            notificationMessage = new Notification
            {
                Body = await notificationService.GetNotificationTitle("x_force_logout_body", usr.LanguageId.Value),
                Title = await notificationService.GetNotificationTitle("x_force_logout_pin_verify_title", usr.LanguageId.Value)
            };
            foreach (var item in listToken)
            {

                //Push notification to firebase
                var msg = new Message
                {
                    Notification = notificationMessage,
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount } },

                    Token = item,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", Constants.ForceLogoutPinVerifyKey},
                        { "dto", ""},
                    },
                };

                SendFirebase(msg);
            }

        }
        public void SendAnnoutcementData(SendFireBaseDataRequest request, AnnouncementModel model)
        {
            var respNoteCount = PingService.NoteCounts(request.RecipientIds);
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            setting.Connection += "Database=" + database;
            string sql = string.Format("SELECT  n.\"Title\" " +
                                       "FROM msg.\"AnnouncementCategory\" n " +
                                       "WHERE \"AnnouncementCategoryId\" = {0}", model.CategoryId);
            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            string title = string.Empty;

            if (obj != null && obj.Count > 0)
            {
                title = obj[0]["Title"].ToString();
            }
            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = getToken(recipentId);
                var msg = new Message
                {
                    Notification = new Notification
                    {
                        Body = model.Html,
                        Title = title
                    },
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                SendFirebase(msg);
            }
        }

        public async Task SendUserStatus(SendFireBaseDataRequest request)
        {
            var respNoteCount = PingService.NoteCounts(request.RecipientIds);
            NotificationService notificationService = new NotificationService(_options);
            var usr = await notificationService.GetUserInfo(request.RecipientIds[0]);

            var notificationMessage = new Notification();
            if (request.Event == Constants.FireBase.EventUserStatusDeactive)
            {
                notificationMessage = new Notification
                {
                    Body = await notificationService.GetNotificationTitle("deactive_user_body", usr.LanguageId.Value),
                    Title = await notificationService.GetNotificationTitle("deactive_user_title", usr.LanguageId.Value)
                };
            }

            if (request.Event == Constants.FireBase.EventUserStatusReactive)
            {
                notificationMessage = new Notification
                {
                    Body = await notificationService.GetNotificationTitle("reactive_user_title", usr.LanguageId.Value),
                    Title = await notificationService.GetNotificationTitle("reactive_user_body", usr.LanguageId.Value)
                };
            }

            if (request.Event == Constants.FireBase.EventUserStatusUnlock)
            {
                notificationMessage = new Notification
                {
                    Body = await notificationService.GetNotificationTitle("unlock_account_title", usr.LanguageId.Value),
                    Title = await notificationService.GetNotificationTitle("unlock_account_body", usr.LanguageId.Value)
                };
            }

            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = getToken(recipentId);
                var msg = new Message
                {
                    Notification = notificationMessage,
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                SendFirebase(msg);
            }
        }

        public async Task SendApplyAgentStatus(SendFireBaseDataRequest request)
        {
            var respNoteCount = PingService.NoteCounts(request.RecipientIds);
            NotificationService notificationService = new NotificationService(_options);
            var usr = await notificationService.GetUserInfo(request.RecipientIds[0]);

            var notificationMessage = new Notification();
            if (request.Event == Constants.FireBase.EventApplyAgentStatusApproved)
            {
                notificationMessage = new Notification
                {
                    Body = await notificationService.GetNotificationTitle("become_agent_approved_body", usr.LanguageId.Value),
                    Title = await notificationService.GetNotificationTitle("become_agent_approved_title", usr.LanguageId.Value)
                };
            }

            if (request.Event == Constants.FireBase.EventApplyAgentStatusDeclined)
            {
                notificationMessage = new Notification
                {
                    Body = await notificationService.GetNotificationTitle("become_agent_declined_body", usr.LanguageId.Value),
                    Title = await notificationService.GetNotificationTitle("become_agent_declined_title", usr.LanguageId.Value)
                };
            }

            if (request.Event == Constants.FireBase.EventApplyAgentStatusUnderReview)
            {
                notificationMessage = new Notification
                {
                    Body = await notificationService.GetNotificationTitle("become_agent_review_body", usr.LanguageId.Value),
                    Title = await notificationService.GetNotificationTitle("become_agent_review_title", usr.LanguageId.Value)
                };
            }

            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = getToken(recipentId);
                var msg = new Message
                {
                    Notification = notificationMessage,
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                SendFirebase(msg);
            }
        }

        public async Task SendKycStatus(SendFireBaseDataRequest request, ClaimsPrincipal User)
        {
            var respNoteCount = PingService.NoteCounts(request.RecipientIds);
            NotificationService notificationService = new NotificationService(_options);
            var usr = await notificationService.GetUserInfo(request.RecipientIds[0]);
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            string noteBody = await notificationService.GetNotificationTitle("note_kyc_status_change_body", usr.LanguageId.Value);
            string noteTitle = await notificationService.GetNotificationTitle("note_kyc_status_change_title", usr.LanguageId.Value);
            var sendKycNotificationRequest = new SendKycNotificationRequest()
            {
                SenderId = userIdClaim,
                RecipientId = request.RecipientIds[0],
                TitleKey = "note_kyc_status_change_title",
                NoteText = noteBody,
                NoteTitle = noteTitle,
                NotificationKey = Constants.FireBase.EventKYCStatusChange,
            };
            await notificationService.CreateNotiForKyc(sendKycNotificationRequest);
            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = getToken(recipentId);
               

                var msg = new Message
                {
                    Notification = new Notification
                    {
                        Body = noteBody,
                        Title = noteTitle
                    },
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                SendFirebase(msg);
            }
        }

        public async Task SendEventSendCrypto(SendFireBaseDataRequest request)
        {
            var respNoteCount = PingService.NoteCounts(request.RecipientIds);
            NotificationService notificationService = new NotificationService(_options);

            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = getToken(recipentId);
                var usr = await notificationService.GetUserInfo(recipentId);

                var msg = new Message
                {
                    Notification = new Notification
                    {
                        Body = await notificationService.GetNotificationTitle("send_crypto_body", usr.LanguageId.Value),
                        Title = await notificationService.GetNotificationTitle("send_crypto_title", usr.LanguageId.Value)
                    },
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                SendFirebase(msg);
            }
        }

        public async Task ReciverCrypto(SendFireBaseDataRequest request)
        {
            var respNoteCount = PingService.NoteCounts(request.RecipientIds);
            NotificationService notificationService = new NotificationService(_options);

            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = getToken(recipentId);
                var usr = await notificationService.GetUserInfo(recipentId);

                var msg = new Message
                {
                    Notification = new Notification
                    {
                        Body = await notificationService.GetNotificationTitle("receive_crypto_body", usr.LanguageId.Value),
                        Title = await notificationService.GetNotificationTitle("receive_crypto_title", usr.LanguageId.Value)
                    },
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                SendFirebase(msg);
            }
        }

        public void SendFeatureData(SendFireBaseDataRequest request, FeatureNotificationModel model)
        {
            var respNoteCount = PingService.NoteCounts(request.RecipientIds);


            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = getToken(recipentId);
                var msg = new Message
                {
                    Notification = new Notification
                    {
                        Body = model.Description,
                        Title = model.Title
                    },
                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                SendFirebase(msg);
            }
        }


        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="tokenFireBase"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public bool updateTokenFireBase(string tokenFireBase, ClaimsPrincipal User)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            setting.Connection += "Database=" + database;

            if (tokenFireBase.Length == 0)
            {
                return false;
            }
            //check Json
            string iff = string.Format("SELECT COUNT(*) FROM msg.\"TokenFireBase\" WHERE \"TokenFireBase\".\"UserId\" = '{0}'", userId);


            var obj = DatabaseHelper.ExcuteSql(iff, setting);
            int check = 0;

            if (obj != null && obj.Rows.Count > 0)
            {
                check = int.Parse(obj.Rows[0]["count"].ToString());
            }

            if (check == 0)
            {

                string sqlCommand = string.Format("INSERT INTO msg.\"TokenFireBase\" " +
                                                  "(\"UserId\",\"Token\",\"CreatedDate\",\"CreatedBy\",\"ModifiedDate\",\"ModifiedBy\") " +
                                                  "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}' ) RETURNING \"Id\" ",
                    userId, tokenFireBase, DateTime.UtcNow, userId, DateTime.UtcNow, userId);
                int newId = 0;
                var obj1 = DatabaseHelper.ExcuteSql(sqlCommand, setting).Rows;

                if (obj1 != null && obj1.Count > 0)
                {
                    newId = int.Parse(obj1[0]["Id"].ToString());
                }
                if (newId == -1)
                {
                    return false;
                }

                return true;
            }
            else
            {
                string sql = string.Format("UPDATE msg.\"TokenFireBase\" " +
                                             "SET " +
                    "\"Token\" = '{0}'," +
                    "\"ModifiedBy\" = '{1}', " +
                    "\"ModifiedDate\" = '{2}' " +
                    "WHERE \"UserId\" = '{3}'; ", tokenFireBase, userId, DateTime.UtcNow, userId);

                return DatabaseHelper.ExcuteNonQuery(sql, setting) > 0;

            }

        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="recipientId"></param>
        /// <returns></returns>
        public string getToken(string recipientId)
        {

            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            setting.Connection += "Database=" + database;

            //check Json
            string sql = string.Format("SELECT \"Token\" FROM msg.\"TokenFireBase\" WHERE msg.\"TokenFireBase\".\"UserId\" = '{0}'; ", recipientId);


            var obj = DatabaseHelper.ExcuteSql(sql, setting);
            string token = String.Empty;

            if (obj != null && obj.Rows.Count > 0)
            {
                token = obj.Rows[0]["Token"].ToString();
            }

            return token;
        }
        public List<string> GetAllTokenFireBaseByUserId(string recipientId)
        {

            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            setting.Connection += "Database=" + database;

            //check Json
            string sql = string.Format("SELECT \"Token\" FROM msg.\"TokenFireBase\" WHERE msg.\"TokenFireBase\".\"UserId\" = '{0}'; ", recipientId);

            var data1 = DatabaseHelper.ExcuteSql(sql, setting);
            var tokenFirebaseList = data1.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("Token")).ToList();
            return tokenFirebaseList;
        }
    }

}