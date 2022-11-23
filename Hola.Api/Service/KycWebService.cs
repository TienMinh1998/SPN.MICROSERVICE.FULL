// -----------Author.Criss--------------
// -Edit : Non----------------
// -------Content : KycService----------

using Dapper;
using Hola.Api.DTO.KYC;
using Hola.Api.DTO.KYC.KycItems;
using Hola.Api.DTO.Request;
using Hola.Api.Model;
using Hola.Api.Models;
using Hola.Api.Service.FilterExtension;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Npgsql;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class KycWebService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.ACCCOUNT_DB;
        private readonly string databaseGalery = Constant.COMMOND_DB;
        private readonly FireBaseService _fire_BaseService;
        private readonly ISearchFilterService _searchFilterService;

        public KycWebService(IOptions<SettingModel> options, ISearchFilterService searchFilterService)
        {
            _options = options;
            _fire_BaseService = new FireBaseService(_options);
            _searchFilterService = searchFilterService;
        }

        public async Task<JsonResponseModel> GetKycByUserId(string UserId)
        {
            try
            {
                string connectionString = _options.Value.Connection + "Database=" + database;
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    StringBuilder user_kyc_query = new StringBuilder();
                    user_kyc_query.Append("select k.\"KycId\", u.\"Username\", u.\"PhoneNumber\",  u.\"UserId\",u.\"CountryId\", k.\"TransactionTimeOfUser\", k.\"CreatedDate\", k.\"Status\",k.\"ModifiedDate\", k.\"KycStage\", ur.\"RoleId\", k.\"KycStage\" ");
                    user_kyc_query.Append("From usr.\"Users\" as \"u\" ");
                    user_kyc_query.Append("INNER JOIN kyc.\"KYCs\" as \"k\" on \"k\".\"UserId\" = u.\"UserId\" ");
                    user_kyc_query.Append("LEFT JOIN usr.\"UserRoles\" as \"ur\" on \"ur\".\"UserId\" = u.\"UserId\" ");
                    user_kyc_query.Append($"WHERE u.\"UserId\" = '{UserId}' LIMIT 1");

                    var listStatus = await connection.QueryFirstOrDefaultAsync<KycWebResponse>(user_kyc_query.ToString());
                    connection.Close();
                    return JsonResponseModel.Success(listStatus);
                }
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        public JsonResponseModel GetListKyc(KycWebRequest request)
        {
            try
            {
                #region CONNECTION
                SettingModel setting = new SettingModel()
                {
                    Connection = _options.Value.Connection + "Database=" + database,
                    Provider = _options.Value.Provider
                };
                # endregion

                #region EXTENSION:  ORDER BY and PADDING

                string orderBy = string.Empty;
                string paging = string.Empty;

                string conditions = GetKYcSearchConditions(request);
                if (!string.IsNullOrEmpty(request.OrderBy))
                {
                    orderBy += $" order by \"{request.OrderBy}\" " + (string.IsNullOrEmpty(request.OrderDirection) ? string.Empty : request.OrderDirection);
                }
                else
                {
                    orderBy += $" order by \"CreatedDate\" DESC";
                }

                var take = !request.PageSize.HasValue || request.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + request.PageSize.Value + " ROWS ONLY";
                paging += " OFFSET " + ((request.PageNumber ?? 1) - 1) * (request.PageSize ?? 0) + " ROWS " + take;

                #endregion EXTENSION:  ORDER BY and PADDING

                #region SELECT AND COUNT
                // Get Data
                StringBuilder sql_select = new StringBuilder();
                sql_select.Append(" select (select * From kyc.criss_check_step_kyc(u.\"UserId\")  as \"step\"), k.\"KycId\", u.\"Username\"," +
                                  " u.\"PhoneNumber\",  u.\"UserId\",u.\"CountryId\", k.\"TransactionTimeOfUser\", k.\"CreatedDate\", k.\"Status\"," +
                                  " k.\"ModifiedDate\", k.\"KycStage\", ur.\"RoleId\" ");
                sql_select.Append("From usr.\"Users\" as \"u\" ");
                sql_select.Append("INNER JOIN kyc.\"KYCs\" as \"k\" on \"k\".\"UserId\" = u.\"UserId\" ");
                sql_select.Append("LEFT JOIN usr.\"UserRoles\" as \"ur\" on \"ur\".\"UserId\" = u.\"UserId\" ");
                sql_select.Append($"WHERE (u.\"PhoneNumber\" LIKE '%{request.SearchText}%' " +
                                  $"or u.\"Username\" LIKE '%{request.SearchText}%' " +
                                  $"or u.\"UserId\" LIKE '%{request.SearchText}%' " +
                                  $"or CAST(k.\"CreatedDate\" AS VARCHAR(10)) LIKE '%" + request.SearchText + "%' " +
                                  $"or CAST(k.\"ModifiedDate\" AS VARCHAR(10)) LIKE '%" + request.SearchText + "%' ) " +
                                  $"{conditions}  And \"IsActive\" = Cast(1 as bit) {orderBy}");

                List<KycWebResponse> response = DatabaseHelper.ExcuteQueryToList<KycWebResponse>(sql_select.ToString(), setting);
                var rs = response.Where(o => o.step == 5).ToList();
                # endregion

                var filterResponse = rs.Where(x => request.Status.HasValue ? x.Status == request.Status : true)
                    .FindBy(request).OrderbEx(request).
                    ToPagedList(request.PageNumber.Value, request.PageSize.Value);

                PaddingResponseModel _padingresponseModel = new PaddingResponseModel
                {
                    Items = filterResponse.ToList(),
                    TotalCount = filterResponse.TotalItemCount,
                    PageLimit = request.PageSize.Value,
                    PageNumber = request.PageNumber.Value
                };
                return JsonResponseModel.Success(_padingresponseModel);
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        public JsonResponseModel GetHistory(KycHistoryRequest request)
        {
            try
            {
                SettingModel setting = new SettingModel()
                {
                    Connection = _options.Value.Connection + "Database=" + database,
                    Provider = _options.Value.Provider
                };
                string sql_query = $" SELECT \"Id\", \"KycId\", \"Type\", \"StatusBefore\", \"StatusAfter\", \"ChangeDate\", \"ChangeBy\", \"Reason\" FROM kyc.\"KycByStepHistory\" WHERE \"KycId\" = '{request.KycId}' and \"KycId\" <> '00000000-0000-0000-0000-000000000000';";
                List<KycHistoryItemResponse> rs = DatabaseHelper.ExcuteQueryToList<KycHistoryItemResponse>(sql_query, setting);

                foreach (var item in rs)
                {
                    item.StepName = GetStepName(item.Type);
                }
                var filterResponse = rs.OrderByDescending(x => x.ChangeDate).ToPagedList(request.PageNumber.Value, request.PageSize.Value);
                PaddingResponseModel _padingresponseModel = new PaddingResponseModel
                {
                    Items = filterResponse.ToList(),
                    TotalCount = filterResponse.TotalItemCount,
                    PageLimit = request.PageSize.Value,
                    PageNumber = request.PageNumber.Value
                };
                return JsonResponseModel.Success(_padingresponseModel);
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        private string GetStepName(string type)
        {
            switch (type)
            {
                case "0":
                    return SystemParam.BankInfotable;

                case "1":
                    return SystemParam.Declaration;

                case "2":
                    return SystemParam.IdentityDocument;

                case "3":
                    return SystemParam.Address;

                case "4":
                    return SystemParam.UserInfomation;

                case "5":
                    return SystemParam.KYC_STEP;

                default:
                    return "";
            }
        }

        private string GetKYcSearchConditions(KycWebRequest request)
        {
            string conditions = string.Empty;
            string countryIds = string.Empty;
            string _userType = string.Empty;
            string _stage = string.Empty;

            // Stage
            if (request.Stage != null)
            {
                foreach (var item in request.Stage) _stage += item + ",";
                _stage += "-1";
                conditions += $"And k.\"KycStage\" IN ({_stage})";
            }
            // Fillter by CountryId
            if (request.CountryId != null)
            {
                foreach (var item in request.CountryId) countryIds += item + ",";
                countryIds += "-1";
                conditions += $"And u.\"CountryId\" IN ({countryIds})";
            }
            if (request.UserType != null)
            {
                foreach (var item in request.UserType) _userType += item + ",";
                _userType += "-1";
                conditions += $" And ur.\"RoleId\" IN ({_userType})";
            }
            // Filter by Date
            if (request.matchedDateFilter == null) return conditions;
            // Date
            if (request.matchedDateFilter.Length > SystemParam.VALUE_ZEZO)
            {
                if (!string.IsNullOrEmpty(request.matchedDateFilter[0]) && string.IsNullOrEmpty(request.matchedDateFilter[1]))
                    conditions += " AND k.\"CreatedDate\" >= '" + request.matchedDateFilter[0] + "'";
                if (string.IsNullOrEmpty(request.matchedDateFilter[0]) && !string.IsNullOrEmpty(request.matchedDateFilter[1]))
                    conditions += " AND k.\"CreatedDate\" <= '" + request.matchedDateFilter[1] + "'";
                if (!string.IsNullOrEmpty(request.matchedDateFilter[0]) && !string.IsNullOrEmpty(request.matchedDateFilter[1]))
                    conditions += " AND (k.\"CreatedDate\" >= '" + request.matchedDateFilter[0] + "' AND k.\"CreatedDate\" <= '" + request.matchedDateFilter[1] + "')";
            }

            return conditions;
        }

        public async Task<JsonResponseModel> GetKycDeatail(string KycId)
        {
            try
            {
                #region CONNECTION
                SettingModel setting = new SettingModel()
                {
                    Connection = _options.Value.Connection + "Database=" + database,
                    Provider = _options.Value.Provider
                };
                # endregion
                KYCBankInfoResponse response = new KYCBankInfoResponse();
                KycDetailWebResponse model = new KycDetailWebResponse();
                using (var connection = new NpgsqlConnection(setting.Connection))
                {
                    using (var multil = connection.QueryMultiple(SQLCommandText.SQL_MULTIPLE_KYC_DETAIL.Replace("@Id", $"{KycId}")))
                    {
                        var kycsModel = await multil.ReadFirstOrDefaultAsync<KYCs>();
                        var bank_info = await multil.ReadFirstOrDefaultAsync<KYCBankInfoResponse>();
                        var document_info = await multil.ReadFirstOrDefaultAsync<KYCDocsResponse>(); ;
                        var declarations_info = await multil.ReadFirstOrDefaultAsync<KycDeclarationResponse>();
                        var kycAddressInfo = await multil.ReadFirstOrDefaultAsync<KYCUserAddressResponse>();
                        var kycUserInfo = await multil.ReadFirstOrDefaultAsync<KYCUserInfoResponse>();

                        // If one of the Steps then Don't  Show
                        int[] docIds = new int[7];
                        long[] doc = new long[3];
                        long[] doc1 = new long[2];
                        long[] doc2 = new long[1];
                        long[] docaddress = new long[2];

                        if (document_info != null)
                        {
                            docIds[0] = document_info.IDCardBack;
                            docIds[1] = document_info.IDCardFont;
                            docIds[2] = document_info.ImageId;

                            doc[0] = document_info.IDCardBack;
                            doc[1] = document_info.IDCardFont;
                            doc[2] = document_info.ImageId;
                        }
                        if (bank_info != null)
                        {
                            docIds[3] = bank_info.BankStatement;
                            docIds[4] = bank_info.NetWorthUsDollars;

                            doc1[0] = bank_info.BankStatement;
                            doc1[1] = bank_info.NetWorthUsDollars;
                        }
                        if (declarations_info != null)
                        {
                            docIds[5] = declarations_info.AttachAfterSigning;
                            doc2[0] = declarations_info.AttachAfterSigning;
                        }
                        if (kycAddressInfo != null)
                        {
                            docIds[6] = kycAddressInfo.FileId;
                            docaddress[0] = kycAddressInfo.FileId;
                        }

                        // Gọi chéo lấy Data :
                        var ListDoc = GetFiles(docIds);
                        model.DocsInfomation = document_info;
                        model.BankInfomation = bank_info;
                        model.Adress = kycAddressInfo;
                        model.DeclarationInfomation = declarations_info;
                        model.UserInfomation = kycUserInfo;

                        if (ListDoc.Count != 0)
                        {
                            model.DocsInfomation.documents = FindByIdDoc(doc, ListDoc);
                            model.BankInfomation.documents = FindByIdDoc(doc1, ListDoc);
                            model.DeclarationInfomation.documents = FindByIdDoc(doc2, ListDoc);
                            model.Adress.documents = FindByIdDoc(docaddress, ListDoc);
                        }
                        model.KycStage = kycsModel.KycStage;
                        model.Status = kycsModel.Status;
                        model.CreatedDate = kycsModel.CreatedDate;
                        model.Note = kycUserInfo.Note;
                    };
                };
                return await Task.FromResult(JsonResponseModel.Success(model));
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        /// <summary>
        /// Create new Note To KYC
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<JsonResponseModel> CreateNoteKyc(KycNoteRequest requestModel, ClaimsPrincipal user)
        {
            #region CONNECTION

            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection + "Database=" + database,
                Provider = _options.Value.Provider
            };

            #endregion CONNECTION

            try
            {
                if (string.IsNullOrEmpty(requestModel.rejectReason)) return JsonResponseModel.Error(SystemParam.DATA_NOTNULL, SystemParam.ERRORCODE);

                string userId = user.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
                string createdId = requestModel.UserId;
                if (userId == requestModel.UserId)
                    createdId = userId;

                using (var connection = new NpgsqlConnection(setting.Connection))
                {
                    StringBuilder sqlCommand = new StringBuilder();
                    sqlCommand.Append($"insert into kyc.\"KYCNoteHistory\" (\"KycId\", \"UserId\", \"Note\", \"CreatedBy\", \"CreatedDate\") " +
                        $"VALUES ('{requestModel.KycId}', '{requestModel.UserId}', '{requestModel.rejectReason.Replace("'", "''")}', '{createdId}', now()); ");
                    await connection.ExecuteAsync(sqlCommand.ToString());
                    connection.Close();
                    return JsonResponseModel.Success(requestModel.rejectReason, SystemParam.UPDATE_SUCCESS);
                }
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.BAD_REQUEST_CODE);
            }
        }

        public async Task<JsonResponseModel> GetNote(KycNoteDataRequest request)
        {
            #region CONNECTION

            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection + "Database=" + database,
                Provider = _options.Value.Provider
            };

            #endregion CONNECTION

            try
            {
                int totalCount = 0;
                string countSql = string.Format("select count(*) " +
                                                    "from kyc.\"KYCNoteHistory\" k left join usr.\"Users\" u on u.\"UserId\" = k.\"CreatedBy\" " +
                                                    "where k.\"KycId\" = '{0}' and k.\"UserId\" = '{1}';",
                                                    request.KycId, request.UserId);

                string sql = string.Format("select k.\"Id\", k.\"KycId\", k.\"UserId\", k.\"Note\", k.\"CreatedBy\" as \"CreatedById\", k.\"CreatedDate\", " +
                                                    "u.\"Username\" as \"CreatedUser\" " +
                                                    "from kyc.\"KYCNoteHistory\" k left join usr.\"Users\" u on u.\"UserId\" = k.\"CreatedBy\" " +
                                                    "where k.\"KycId\" = '{0}' and k.\"UserId\" = '{1}' " +
                                                    "order by k.\"CreatedDate\" desc " +
                                                    "OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY;",
                                                    request.KycId, request.UserId, ((request.PageNumber ?? 1) - 1) * (request.PageSize ?? 0), request.PageSize);

                var data = DatabaseHelper.ExcuteQueryToList<NoteListModel>(sql, setting);
                var obj = DatabaseHelper.ExcuteSql(countSql, setting).Rows;
                if (obj != null && obj.Count > 0)
                {
                    totalCount = int.Parse(obj[0]["count"].ToString());
                }

                var result = new DataModel()
                {
                    TotalCount = totalCount,
                    Items = (IList)data
                };

                return JsonResponseModel.Success(result);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.BAD_REQUEST_CODE);
            }
        }

        private List<FileModelResponse> FindByIdDoc(long[] docid, IList<FileModelResponse> FileModelResponse)
        {
            try
            {
                var list = new List<FileModelResponse>();
                foreach (var item in FileModelResponse)
                    if (docid.Contains(item.Id)) list.Add(item);
                return list;
            }
            catch (Exception)
            {
                return new List<FileModelResponse>();
            }
        }

        public async Task<JsonResponseModel> UpdateStatus(UpdateStatusRequest model, ClaimsPrincipal User)
        {
            try
            {
                #region CONNECTION

                SettingModel setting = new SettingModel()
                {
                    Connection = _options.Value.Connection + "Database=" + database,
                    Provider = _options.Value.Provider
                };

                #endregion CONNECTION

                // Update Status of KYC Item
                if (string.IsNullOrEmpty(model.Note)) JsonResponseModel.Error("NOTE can't not Null", SystemParam.BAD_REQUEST_CODE);
                string tableName = GetTableNameFromDocumentType(model.DocumentCheckType);
                // Update KYC Status
                CheckStatusResponse listStatus = new CheckStatusResponse();
                using (var connection = new NpgsqlConnection(setting.Connection))
                {
                    // get before Status
                    string befote_Status_query = $"SELECT \"Status\" FROM kyc.\"{tableName}\" WHERE \"UserId\" = '{model.UserId}' and \"IsActive\" = Cast(1 as bit);";
                    var before_status = await connection.QueryFirstOrDefaultAsync<short>(befote_Status_query);
                    if (before_status != (short)model.Status)
                    {
                        string strNote = string.IsNullOrEmpty(model.Note) ? string.Empty : model.Note.Replace("'", "''");
                        string strReason = string.IsNullOrEmpty(model.RejectReason) ? string.Empty : model.RejectReason.Replace("'", "''");
                        StringBuilder sqlCommand = new StringBuilder($"UPDATE kyc.\"{tableName}\" SET  \"Status\"={model.Status} ,\"Note\"='{strNote}',\"RejectReason\"='{strReason}'  WHERE \"UserId\" = '{model.UserId}'");

                        long _count = (long)DatabaseHelper.ExcuteNonQuery(sqlCommand.ToString(), setting); string sql_text = $" select * From kyc.checkstatus('{model.UserId}')";
                        listStatus = await connection.QueryFirstOrDefaultAsync<CheckStatusResponse>(sql_text);
                        string KycId_query = $" SELECT \"KycId\" FROM kyc.\"KYCs\" WHERE \"UserId\" = '{model.UserId}'";
                        var kyc_id = await connection.QueryFirstOrDefaultAsync<Guid>(KycId_query);
                        // cập nhật lịch sử, thông báo cho note APP
                        string insert_query = "INSERT INTO kyc.\"KycByStepHistory\"( \"KycId\", \"Type\", \"StatusBefore\", \"StatusAfter\", \"ChangeDate\", \"ChangeBy\", \"Reason\") " +
                                            $" VALUES ( '{kyc_id}', '{model.DocumentCheckType}', {before_status}, {model.Status}, '{DateTime.UtcNow}', {model.UserId}, '{strNote}');";

                        await connection.ExecuteAsync(insert_query);
                        APICrossHelper aPICrossHelper = new APICrossHelper();
                        var dataSendFireBase = new SendFireBaseDataRequest
                        {
                            RecipientIds = new List<string> { model.UserId },
                            Event = Constants.FireBase.EventKYCStatusChange,
                            Data = JsonSerializer.Serialize(model)
                        };
                        FireBaseService fireBaseService = new FireBaseService(_options);
                        await fireBaseService.SendKycStatus(dataSendFireBase,User);
                    }
                    else
                    {
                        return JsonResponseModel.Error("The Current Status Equal new Status", SystemParam.ERRORCODE);
                    }
                    connection.Close();
                }
                return JsonResponseModel.Success(listStatus);
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        public async Task<JsonResponseModel> OnKycOfUser(OnOffRequest request,ClaimsPrincipal User)
        {
            try
            {
                #region CONNECTION

                SettingModel setting = new SettingModel()
                {
                    Connection = _options.Value.Connection + "Database=" + database,
                    Provider = _options.Value.Provider
                };

                #endregion CONNECTION

                int new_status = (request.NewStatus <= SystemParam.ZERO_VALUES || request.NewStatus > SystemParam.ACTIVE)
                                                    ? SystemParam.PENDING
                                                    : SystemParam.ACTIVE;

                StringBuilder sqlCommand = new StringBuilder();
                sqlCommand.Append($"UPDATE kyc.\"KYCs\" SET \"Status\"={new_status} ");
                sqlCommand.Append($"WHERE \"UserId\"= '{request.UserId}'");
                long _type = (long)DatabaseHelper.ExcuteNonQuery(sqlCommand.ToString(), setting);
                APICrossHelper aPICrossHelper = new APICrossHelper();
                var dataSendFireBase = new SendFireBaseDataRequest
                {
                    RecipientIds = new List<string> { request.UserId },
                    Event = Constants.FireBase.EventKYCStatusChange,
                    Data = JsonSerializer.Serialize(request)
                };
                FireBaseService fireBaseService = new FireBaseService(_options);
                await fireBaseService.SendKycStatus(dataSendFireBase,User);
                return JsonResponseModel.Success(_type);
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        public async Task<JsonResponseModel> ChangeKycStage(kycStageRequest request,ClaimsPrincipal User)
        {
            try
            {
                SettingModel setting = new SettingModel()
                {
                    Connection = _options.Value.Connection + "Database=" + database,
                    Provider = _options.Value.Provider
                };

                using (var connection = new NpgsqlConnection(setting.Connection))
                {
                    string query = $"SELECT * FROM kyc.ayncsubstatus('{request.KycId}',{request.KycStage})";
                    var response = connection.QueryFirstOrDefault<Guid>(query);
                    if (response.ToString() == "00000000-0000-0000-0000-000000000000")
                      return  JsonResponseModel.Error("KycId invalid", SystemParam.BAD_REQUEST_CODE);

                    string sql = string.Format("SELECT \"UserId\" From kyc.\"KYCs\" WHERE \"KycId\" = '{0}'", request.KycId);
                    var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
                    string userId = string.Empty;

                    if (obj != null && obj.Count > 0)
                    {
                        userId = obj[0]["UserId"].ToString();
                    }

                    APICrossHelper aPiCrossHelper = new APICrossHelper();
                    var dataSendFireBase = new SendFireBaseDataRequest
                    {
                        RecipientIds = new List<string> { userId },
                        Event = Constants.FireBase.EventKYCStatusChange,
                        Data = JsonSerializer.Serialize(response)
                    };
                    FireBaseService fireBaseService = new FireBaseService(_options);
                    await fireBaseService.SendKycStatus(dataSendFireBase,User);

                    return JsonResponseModel.Success(response);
                }
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        public JsonResponseModel CountTransactionTimeOfUser(string UserId)
        {
            try
            {
                #region CONNECTION

                SettingModel setting = new SettingModel()
                {
                    Connection = _options.Value.Connection + "Database=" + database,
                    Provider = _options.Value.Provider
                };

                #endregion CONNECTION

                // Get TransactionTimeOfUser BY UserId
                string sqlCommand = $" SELECT \"TransactionTimeOfUser\" FROM kyc.\"KYCs\" WHERE \"UserId\" = '{UserId}' ";
                short _count = (short)DatabaseHelper.ExecuteScalar(sqlCommand.ToString(), setting);
                // Count TransactionTimeOfUser

                if (_count > 0)
                {
                    short new_count = ++_count;
                    StringBuilder sqlCommandCount = new StringBuilder();
                    sqlCommandCount.Append($"UPDATE kyc.\"KYCs\" SET \"TransactionTimeOfUser\"={new_count} ");
                    sqlCommandCount.Append($"WHERE \"UserId\"= '{UserId}'");
                    long response = (long)DatabaseHelper.ExcuteNonQuery(sqlCommandCount.ToString(), setting);

                    return JsonResponseModel.Success(response);
                }
                return JsonResponseModel.Error(SystemParam.UPDATE_ERROR, SystemParam.ERRORCODE);
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }
        }

        public async Task<List<string>> UploadImages(string FileName, HttpContext context, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                List<string> listImage = new List<string>();
                var httpRequest = context.Request;
                var postedFile = httpRequest.Form.Files.GetFiles(FileName);
                if (postedFile != null && postedFile.Count > 0)
                {
                    var folderName = Path.Combine("UploadFile", "Images");
                    var pathToSave = Path.Combine(webHostEnvironment.WebRootPath, folderName);

                    var host = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
                    foreach (var file in postedFile)
                    {
                        string name = DateTime.Now.ToString("ssddMMyyyy") + file.FileName;
                        var fullPath = Path.Combine(pathToSave, name);
                        var url = host + "/UploadFile/Images/" + name;
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        listImage.Add(url);
                    }
                    return await Task.FromResult(listImage);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private string GetTableNameFromDocumentType(int DocumentType)
        {
            string table_name = string.Empty;
            switch (DocumentType)
            {
                case (int)DocumentCheckType.KYCBankInfo:
                    table_name = "KYCBankInfo"; break;
                case (int)DocumentCheckType.KYCDeclarations:
                    table_name = "KYCDeclarations"; break;
                case (int)DocumentCheckType.KYCDocs:
                    table_name = "KYCDocs"; break;
                case (int)DocumentCheckType.KYCUserAddress:
                    table_name = "KYCUserAddress"; break;
                case (int)DocumentCheckType.KYCUserInfo:
                    table_name = "KYCUserInfo"; break;
            }
            return table_name;
        }

        private List<FileModelResponse> GetFiles(int[] filesid)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            try
            {
                string _listID = string.Empty;
                foreach (var item in filesid) _listID += item + ",";
                _listID += "-1";
                string sql = string.Format("SELECT \"Id\",\"FileName\", \"FileUrl\", \"Size\", \"Extension\" FROM cmn.\"Gallery\" WHERE \"Id\" IN ({0})", _listID);
                setting.Connection += "Database=" + databaseGalery;
                var response = DatabaseHelper.ExcuteQueryToList<FileModelResponse>(sql, setting);
                if (response == null) JsonResponseModel.Success(new List<FileModelResponse>());
                return response;
            }
            catch (Exception ex)
            {
                return new List<FileModelResponse>();
            }
        }
    }
}