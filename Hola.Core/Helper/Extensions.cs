using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hola.Core.Common;
using Hola.Core.Model.CommonModel;
using Hola.Core.Model.DBModel.cmn;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Hola.Core.Helper
{
    public class Extensions
    {
        public CountryModel ResolveCountryFromPhoneNumber(string phoneN, CountryModel country)
        {
            phoneN = phoneN.TrimStart('0');


            if (phoneN.StartsWith(country.PhoneCode))
            {
                return country;
            }

            return country; // need more investigation
        }
        public CountryRequest ResolveCountryFromPhoneNumber(string phoneN, CountryRequest country)
        {
            phoneN = phoneN.TrimStart('0');


            if (phoneN.StartsWith(country.PhoneCode))
            {
                return country;
            }

            return country; // need more investigation
        }
        public string MaskPhoneNumber(string source, int lastCharCount)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            if (lastCharCount >= source.Length)
                return source;

            string phoneCode = Constants.CountryPhoneCodes.AllPhoneCodes.FirstOrDefault(c => source.StartsWith(c)) ?? "";
            string asterisks = new string('*', source.Length - lastCharCount - phoneCode.Length);
            string lastVisibleDigits = source.Substring(source.Length - lastCharCount);

            return $"{phoneCode}{asterisks}{lastVisibleDigits}";
        }
        public string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return null;
            return phoneNumber.TrimStart('+').Replace(" ", "");
        }
        public string NormalizePhoneNumber(string countryCode, string phoneNumber)
        {
            var normalizePhoneNumber = NormalizePhoneNumber(phoneNumber);

            if (normalizePhoneNumber.StartsWith(countryCode + '0'))
                normalizePhoneNumber = countryCode + normalizePhoneNumber[(countryCode.Length + 1)..];

            return normalizePhoneNumber;
        }
        public static async Task<string> GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            var rawRequestBody = request.Body.ToString();
            try
            {
                var token = request.Headers["Authorization"].ToString();
                var TokenArray = token.Split(" ");
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(TokenArray.Last<string>());
                var _userid = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
                keyBuilder.Append(_userid);
            }
            catch (System.Exception)
            {
                keyBuilder.Append("_");
            }
            var reader = new StreamReader(request.Body);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            var _bodyContent = await reader.ReadToEndAsync();
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_bodyContent);
            if (request.Query.Count > SystemParam.VALUE_ZEZO)
            {
                foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                {
                    keyBuilder.Append($"|{key}?{value}");
                }
            }
            else
            {
                if (values!=null)
                {
                    foreach (var (key, value) in values)
                    {
                        keyBuilder.Append($"|{key}?{value}");
                    }
                }
                
            }
            return keyBuilder.ToString();
        }

       
    }
}
