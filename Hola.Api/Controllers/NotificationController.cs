using System;
using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Common;
using Hola.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using System.Collections.Generic;
using System.Linq;
using Hola.Core.Service;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Hola.Core.Model.CommonModel;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class NotificationController : ControllerBase
    {
        // GET
        private readonly ILogger<NotificationController> _logger;
        private readonly IOptions<SettingModel> _config;
        private readonly IHostingEnvironment hostingEnvironment;

        public NotificationController(ILogger<NotificationController> logger, IOptions<SettingModel> config, IHostingEnvironment environment)
        {
            _logger = logger;
            _config = config;
            hostingEnvironment = environment;

        }

        [HttpPost("message/OfferP2PExpire")]
        [AllowAnonymous]
        public async Task<JsonResponseModel> OfferP2PExpire(NotificationCreateOfferExchangeRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Message Offer P2P Expire");
                var req = new SendNotificationP2PRequest
                {
                    NotificationKey = request.NotifiKey,
                    SenderId = request.BuyerId,
                    Event = Constants.FireBase.p2pExchangeChanged,
                    RecipientId = request.SellerId,
                    TitleKey = MessagingConstants.Types.NoteP2P,
                    TransactionId = request.TransactionId,
                    OfferExchangeId = request.OfferExchangeId,
                    TransactionType = MessagingConstants.TransactionTypes.P2P,
                    Status = request.Status
                };
                NotificationService _service = new NotificationService(_config);
                var data = await _service.CreateNotiForP2P(req);

                //var req1 = new SendNotificationP2PRequest
                //{
                //    NotificationKey = request.NotifiKey,
                //    SenderId = request.SellerId,
                //    Event = Constants.FireBase.p2pExchangeChanged,
                //    RecipientId = request.BuyerId,
                //    TitleKey = MessagingConstants.Types.NoteP2P,
                //    TransactionId = request.TransactionId,
                //    TransactionType = MessagingConstants.TransactionTypes.P2P,
                //    Status = request.Status
                //};
                //var data1 = await _service.CreateNotiForP2P(req1);

                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message Offer P2P Expire:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }

        #region John

        [HttpPost("message/CreateOfferExchange")]
        public async Task<JsonResponseModel> CreateOfferExchange(NotificationCreateOfferExchangeRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Message Create Offer Exchange");
                var req = new SendNotificationP2PRequest
                {
                    NotificationKey = request.NotifiKey,
                    SenderId = request.BuyerId,
                    Event = Constants.FireBase.p2pExchangeChanged,
                    RecipientId = request.SellerId,
                    TitleKey = MessagingConstants.Types.NoteP2P,
                    TransactionId = request.TransactionId,
                    OfferExchangeId = request.OfferExchangeId,
                    TransactionType = MessagingConstants.TransactionTypes.P2P,
                    Status = request.Status
                };
                NotificationService _service = new NotificationService(_config);
                var data = await _service.CreateNotiForP2P(req);

                //var req1 = new SendNotificationP2PRequest
                //{
                //    NotificationKey = request.NotifiKey,
                //    SenderId = request.SellerId,
                //    Event = Constants.FireBase.p2pExchangeChanged,
                //    RecipientId = request.BuyerId,
                //    TitleKey = MessagingConstants.Types.NoteP2P,
                //    TransactionId = request.TransactionId,
                //    TransactionType = MessagingConstants.TransactionTypes.P2P,
                //    Status = request.Status
                //};
                //var data1 = await _service.CreateNotiForP2P(req1);

                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message Create Offer Exchange:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }

        [HttpPost("message/AdminTransfer")]
        public async Task<JsonResponseModel> AdminTransfer(NotificationAdminTransferRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Message Admin Transfer");
                var req = new SendNotificationRequest
                {
                    NotificationKey = request.Amount < 0 ? MessagingConstants.Texts.AdminDeducted : MessagingConstants.Texts.AdminAdded,
                    SenderId = request.SenderId,
                    RecipientId = request.RecipientId,
                    TitleKey = MessagingConstants.Types.NoteTransfer,
                    Amount = request.Amount,
                    CurrencyId = request.CurrencyId,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Admin,
                    MaskPhoneNumber = false,
                    Event = Constants.FireBase.EventRecentActivityChanged
                };
                NotificationService _service = new NotificationService(_config);
                var data = await _service.Create(req);
                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message Admin Transfer:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }


        [HttpPost("message/AdminKYC")]
        public async Task<JsonResponseModel> AdminKYC(NotificationAdminKYCRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Message Admin KYC");
                NotificationService _service = new NotificationService(_config);
                foreach (var item in request.RecipientId)
                {
                    var req = new SendNotificationRequest
                    {
                        NotificationKey = MessagingConstants.Texts.KycApplied,
                        SenderId = request.SenderId,
                        RecipientId = item,
                        TitleKey = MessagingConstants.Types.NoteKYC,
                    };
                    await _service.Create(req);

                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                }

                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message Admin KYC:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }


        [HttpPost("message/BuyWarning")]
        public async Task<JsonResponseModel> BuyWarning(NotificationBuyWarningRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Message Buy Warning");
                NotificationService _service = new NotificationService(_config);
                var req = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.BuyWarning,
                    SenderId = request.SenderId,
                    RecipientId = request.RecipientId,
                    TitleKey = MessagingConstants.Types.NoteWarning,
                };
                var data = await _service.Create(req);
                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message Buy Warning:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("message/TransactionCompleted")]
        public async Task<JsonResponseModel> TransactionCompleted(NotificationTransactionStatusChangedRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Transaction Completed");
                NotificationService _service = new NotificationService(_config);
                var respNote = PingService.NoteCounts(new List<string> { request.BuyerId, request.SellerId });
                var req = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.TransactionCompleted,
                    SenderId = request.SellerId,
                    RecipientId = request.BuyerId,
                    TitleKey = MessagingConstants.Types.NoteBuy,
                    UnreadNoteCount = respNote[0],
                    TransactionId = request.TransactionId,
                    Event = Constants.FireBase.EventRecentActivityChanged,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                var data = await _service.Create(req);

                var req1 = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.TransactionCompleted,
                    SenderId = request.BuyerId,
                    RecipientId = request.SellerId,
                    UnreadNoteCount = respNote[1],
                    TitleKey = MessagingConstants.Types.NoteSell,
                    TransactionId = request.TransactionId,
                    Event = Constants.FireBase.EventRecentActivityChanged,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                await _service.Create(req1);
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Transaction Completed:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }

            return response;

        }


        [HttpPost("message/Expired")]
        public async Task<JsonResponseModel> Expire(NotificationExpireRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Expired");
                NotificationService _service = new NotificationService(_config);


                if (request.Recipients.Any())
                {
                    for (int i = 0; i < request.Recipients.Count; i++)
                    {
                        var respNote = PingService.NoteCounts(new List<string> { request.Recipients[i].RecipientId });

                        var req = new SendNotificationRequest
                        {
                            NotificationKey = request.Recipients[i].TitleKey,
                            SenderId = "admin",
                            RecipientId = request.Recipients[i].RecipientId,
                            TitleKey = MessagingConstants.Types.NoteTransfer,
                            UnreadNoteCount = respNote[0],
                            TransactionId = request.Recipients[i].TransactionId,
                            TransactionType = MessagingConstants.TransactionTypes.Admin,
                            MaskPhoneNumber = false
                        };

                        await _service.Create(req);
                    }

                }


                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Expired:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }

            return response;

        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("message/TransactionCancelled")]
        public async Task<JsonResponseModel> TransactionCancelled(NotificationTransactionStatusChangedRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Message Transaction Cancelled");
                var respNote = PingService.NoteCounts(new List<string> { request.BuyerId, request.SellerId });
                var req = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.TransactionCancelled,
                    SenderId = request.SellerId,
                    Event = Constants.FireBase.EventRecentActivityChanged,
                    RecipientId = request.BuyerId,
                    UnreadNoteCount = respNote[0],
                    TitleKey = MessagingConstants.Types.NoteBuy,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                NotificationService _service = new NotificationService(_config);
                var data = await _service.Create(req);

                var req1 = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.TransactionCancelled,
                    SenderId = request.BuyerId,
                    Event = Constants.FireBase.EventRecentActivityChanged,
                    RecipientId = request.SellerId,
                    UnreadNoteCount = respNote[1],
                    TitleKey = MessagingConstants.Types.NoteSell,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                await _service.Create(req1);
                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message Transaction Cancelled:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }

        [HttpPost("message/BecomeAgent")]
        public async Task<JsonResponseModel> BecomeAgent(NotificationBecomeAgentRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Message Become Agent");
                var req = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.BecomeAgent,
                    SenderId = request.SenderId,
                    RecipientId = request.RecipientId,
                    TitleKey = MessagingConstants.Types.NoteAgent,
                };
                NotificationService _service = new NotificationService(_config);
                var data = await _service.Create(req);
                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message Become Agent:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }

        [HttpPost("message/SellerOfferMatched")]
        public async Task<JsonResponseModel> SellerOfferMatched(NotificationOfferMatchedRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Seller Offer Matched:");
                NotificationService _service = new NotificationService(_config);
                var respNote = PingService.NoteCounts(new List<string> { request.BuyerId, request.SellerId });
                var req = new SendNotificationRequest
                {
                    NotificationKey = request.OfferStatus == "WAITING_BANK_DETAILS" ? MessagingConstants.Texts.SellerOfferMatchedUser : MessagingConstants.Texts.BuyerOfferMatchedAgent,
                    SenderId = request.SellerId,
                    RecipientId = request.BuyerId,
                    UnreadNoteCount = respNote[0],
                    TitleKey = MessagingConstants.Types.NoteBuy,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                var data = await _service.Create(req);

                var req1 = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.SellerOfferMatchedAgent,
                    SenderId = request.BuyerId,
                    RecipientId = request.SellerId,
                    UnreadNoteCount = respNote[1],
                    TitleKey = MessagingConstants.Types.NoteSell,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                var data1 = await _service.Create(req1);
                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Seller Offer Matched:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;

        }


        [HttpPost("message/BuyerOfferMatched")]
        public async Task<JsonResponseModel> BuyerOfferMatched(NotificationOfferMatchedRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Buyer Offer Matched:");
                NotificationService _service = new NotificationService(_config);
                var respNote = PingService.NoteCounts(new List<string> { request.BuyerId, request.SellerId });
                var req = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.BuyerOfferMatchedAgent,
                    SenderId = request.SellerId,
                    RecipientId = request.BuyerId,
                    UnreadNoteCount = respNote[0],
                    TitleKey = MessagingConstants.Types.NoteBuy,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                var data = await _service.Create(req);

                var req1 = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.BuyerOfferMatchedUser,
                    SenderId = request.BuyerId,
                    RecipientId = request.SellerId,
                    UnreadNoteCount = respNote[1],
                    TitleKey = MessagingConstants.Types.NoteSell,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };
                var data1 = await _service.Create(req1);
                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Buyer Offer Matched:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;

        }

        [HttpPost("message/OfferAutoMatched")]
        public async Task<JsonResponseModel> OfferAutoMatched(NotificationAutoOfferMatched request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Offer Auto Matched:");
                NotificationService _service = new NotificationService(_config);
                var respNote = PingService.NoteCounts(new List<string> { request.BuyerId, request.SellerId });
                var req = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.AutoMatchedBuyer,
                    SenderId = request.SellerId,
                    RecipientId = request.BuyerId,
                    UnreadNoteCount = respNote[0],
                    TitleKey = MessagingConstants.Types.NoteBuy,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };

                var resp = await _service.Create(req);

                var req1 = new SendNotificationRequest
                {
                    NotificationKey = MessagingConstants.Texts.AutoMatchedSeller,
                    SenderId = request.BuyerId,
                    RecipientId = request.SellerId,
                    UnreadNoteCount = respNote[1],
                    TitleKey = MessagingConstants.Types.NoteSell,
                    TransactionId = request.TransactionId,
                    TransactionType = MessagingConstants.TransactionTypes.Offer,
                    OfferType = request.OfferType
                };

                var resp1 = await _service.Create(req1);
                if (resp > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = resp;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Offer Auto Matched:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }


        #endregion John

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        [Authorize]
        public JsonResponseModel search(NotificationFilterModel req)
        {
            try
            {
                _logger.LogInformation("Create notification");
                JsonResponseModel response = new JsonResponseModel();
                NotificationService notificationService = new NotificationService(_config);

                var rs = notificationService.Search(req, User);

                response.Status = 200;
                response.Message = Constant.MSG_SUCCESS;
                response.Data = rs;

                              
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Search Notification: " + ex.Message);
                return null;
            }
        }

        [HttpPost("UpdateCurrencyStatus")]
        [AllowAnonymous]
        public JsonResponseModel UpdateCurrencyStatus(List<string> recipientIds)
        {
            try
            {
                _logger.LogInformation("Create Currency notification");
                JsonResponseModel response = new JsonResponseModel();
                NotificationService notificationService = new NotificationService(_config);

                notificationService.UpdateCurrencyStatus(recipientIds);

                response.Status = 200;
                response.Message = Constant.MSG_SUCCESS;
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Create Currency notification: " + ex.Message);
                return null;
            }
        }

        [HttpPost("UpdateUserStatus")]
        [AllowAnonymous]
        public JsonResponseModel UpdateUserStatus(UpdateUserStatus model)
        {
            try
            {
                _logger.LogInformation("Create User Status update notification");
                JsonResponseModel response = new JsonResponseModel();
                NotificationService notificationService = new NotificationService(_config);

                notificationService.UpdateUserStatus(model.RecipientIds, model.Type);

                response.Status = 200;
                response.Message = Constant.MSG_SUCCESS;
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Create User Status update notification: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("SetAsRead")]
        [Authorize]
        public JsonResponseModel setAsRead(SetNotificationAsReadRequest req)
        {
            try
            {
                _logger.LogInformation("set as read notification");
                JsonResponseModel response = new JsonResponseModel();
                NotificationService notificationService = new NotificationService(_config);


                bool rs = notificationService.SetAsRead(req, User);
                if (rs)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;

                    sendPingMessage(req);

                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("set as read Notification: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <returns></returns>
        [HttpPost("SetAllAsRead")]
        [Authorize]
        public JsonResponseModel setAllAsRead()
        {
            try
            {
                _logger.LogInformation("set as read notification");
                JsonResponseModel response = new JsonResponseModel();
                NotificationService notificationService = new NotificationService(_config);

                bool rs = notificationService.SetAllAsRead(User);
                if (rs)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    sendPingMessage(null);
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("set as all read Notification: " + ex.Message);
                return null;
            }
        }



        private void sendPingMessage(SetNotificationAsReadRequest req)
        {
            FireBaseService fireBaseService = new FireBaseService(_config);

            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            int respNoteCount = 0;
            if (req != null)
            {
                respNoteCount = PingService.RemoveNotificationForUser(userId, req.NotificationIds);
            }
            else
            {
                respNoteCount = PingService.RemoveAllNotification(userId);
            }

            string token = fireBaseService.getToken(userId);

            var fireBaseRequest = new SendFireBaseDataRequest
            {
                Event = Constants.FireBase.EventNoteCount,
                Data = $"{{\"notificationCount\": {respNoteCount}}}"
            };

            var msg = new Message
            {

                Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount } },

                Token = token,
                Data = new Dictionary<string, string>()
                        {
                            { "event", Constants.FireBase.EventNoteCount},

                            { "dto", fireBaseRequest.Data },
                        }
            };
            fireBaseService.SendFirebase(msg);
        }

        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<JsonResponseModel> create(SendNotificationRequest req)
        {
            try
            {
                _logger.LogInformation("Create notification");
                JsonResponseModel response = new JsonResponseModel();
                NotificationService notificationService = new NotificationService(_config);

                //save notification to db
                var data = await notificationService.Create(req);

                if (data > -1)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    // add noteCount to ping
                    PingService.AddNotificationForUser(req.RecipientId, data);

                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }

                _logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(response));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Create Notification: " + ex.Message);
                return null;
            }
        }

        [HttpGet("GetCountUnread")]
        [Authorize]
        public JsonResponseModel GetCountUnread()
        {
            try
            {
                _logger.LogInformation("Get count unread notification");
                NotificationService notificationService = new NotificationService(_config);
                int rs = notificationService.GetCountUnread(User);
                return JsonResponseModel.Success(rs);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get count unread Notification: " + ex.Message);
                return JsonResponseModel.Error(ex.Message, 204);
            }
        }
    }
}
