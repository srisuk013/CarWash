using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Service
{
    public class ServiceToken
    {
        private IConfiguration _configuration;
        private readonly CarWashContext _context;
        public ServiceToken(CarWashContext context)
        {
            _context = context;
        }
        public const string Token = "ToKen";

        public String Issue(Dictionary<string, object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            String token = encoder.Encode(payload, AppSettings.Secret);
            return token;
        }
        public static Dictionary<String, object> Decode(String token, String SecretKey, Boolean verify)
        {
            String json = "";
            Dictionary<String, object> values = new Dictionary<string, object>();
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                token = token.Replace("Bearer", "").Trim();
                json = decoder.Decode(token, SecretKey, verify: verify);
                values = JsonConvert.DeserializeObject<Dictionary<String, object>>(json);

            }
            catch (TokenExpiredException ex)
            {
                throw ex;

            }
            catch (SignatureVerificationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return values;
        }

    }
}
