using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using CarWash.Service;
using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]

    public class TokenController : CarWashController
    {

        private CarWashContext _context;

        public TokenController(CarWashContext context, ServiceToken service)
        {
            _context = context;
            Service = service;
        }

        public DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }
        [HttpPost]
        public IActionResult RefreshToken([FromBody]ReqRefreshToken reqRefreshToken)
        {
            if (String.IsNullOrEmpty(reqRefreshToken.RefreshToken))
            {
                return BadRequest();
            }
            Exception ex;
            var VerifyToken = CarWashAuthorization.Verify(reqRefreshToken.RefreshToken, out ex);
            if (ex is TokenExpiredException)
            {
                BaseResponse baseResponse = new BaseResponse();
                baseResponse.Success = false;
                baseResponse.Message = "กรุณาlogin";
                return Json(baseResponse);
            }
            Dictionary<String, object> payload = ServiceToken.Decode(reqRefreshToken.RefreshToken, AppSettings.Secret, true);
            long unitDate1 = (long)Convert.ToDouble(payload["exp"]);
            try
            {
                int idName = int.Parse(payload["user_id"].ToString());
                User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                DateTime foo1 = DateTime.UtcNow.AddDays(7);
                long unixTime1 = ((DateTimeOffset)foo1).ToUnixTimeSeconds();
                Dictionary<String, Object> payloadBody = new Dictionary<String, Object>
                    {
                        { "issuer" , "Carwash-wed-App"},
                        { "audience" , "Carwash" },
                        { "sub", "CarWash-Api"},
                        { "username", user.Username},
                        { "user_id", user.UserId },
                        { "Code", user.Code },
                        { "exp", unixTime1 },
                        { "alg", "HS256"}
                    };
                DateTime foo = DateTime.UtcNow.AddDays(8);
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                Dictionary<String, Object> payloadBodyRe = new Dictionary<String, Object>
                    {
                        { "issuer" , "Carwash-wed-App"},
                        { "audience" , "Carwash" },
                        { "sub", "CarWash-Api"},
                        { "username", user.Username},
                        { "user_id", user.UserId },
                        { "Code", user.Code },
                        { "exp", unixTime},
                        { "alg", "HS256"}
                    };
                SignInResponse signInResponse = new SignInResponse();
                signInResponse.Success = true;
                signInResponse.Message = "สำเร็จ";
                signInResponse.Token = Service.Issue(payloadBody);
                signInResponse.RefreshToken = Service.Issue(payloadBodyRe);
                return Json(signInResponse);
            }
            catch (Exception e)
            {

            }
            return Ok();
        }
    }
}