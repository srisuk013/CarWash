using CarWash.Models.DBModels;
using CarWash.Service;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CarWash.Areas.Api.Models
{
    public class CarWashAuthorization : IAuthorizationFilter
    {
        private readonly CarWashContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarWashAuthorization(CarWashContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
          
            if (filterContext != null)
            {
                Microsoft.Extensions.Primitives.StringValues authTokens;
                filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);
                ServiceToken serviceToken = new ServiceToken(_context);


                var _token = authTokens.FirstOrDefault();
                _token = _token.Replace("Bearer ", "");

                if (_token != null)
                {
                    Exception ex;
                    string authToken = _token;
                    Dictionary<String, object> payload = ServiceToken.Decode(authToken, AppSettings.Secret, true);

                    if (authToken != null)
                    {
                        if (Verify(authToken, out ex))
                        {
                            filterContext.HttpContext.Response.Headers.Add("Authorization", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                            filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");
                            
                            int idName = int.Parse(payload["user_id"].ToString());
                            var idNames = payload["user_id"].ToString();
                            User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                            if (user != null)
                            {
                                var claims = new List<Claim>
                                 {
                                     new Claim(ClaimTypes.NameIdentifier,idNames ),
                                     
                                 };
                                var appIdentity = new ClaimsIdentity(claims);
                                filterContext.HttpContext.User.AddIdentity(appIdentity);
                            }

                        }
                        else
                        {
                            if (ex is TokenExpiredException)
                            {
                                filterContext.Result = new JsonResult("NotAuthorized")
                                {
                                    Value = new
                                    {
                                        Status = "Error",
                                        Message = "Invalid Token"
                                    },
                                };
                            }
                            else if (ex is SignatureVerificationException)
                            {
                                filterContext.Result = new JsonResult("NotAuthorized")
                                {
                                    Value = new
                                    {
                                        Status = "Error",
                                        Message = "Invalid Token"
                                    },
                                };
                            }
                            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                            filterContext.Result = new JsonResult("NotAuthorized")
                            {
                                Value = new
                                {
                                    Status = "Error",
                                    Message = "Invalid Token"
                                },
                            };
                        }

                    }

                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                    filterContext.Result = new JsonResult("Please Provide authToken")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = "Please Provide authToken"
                        },
                    };
                }
            }
        }
        public static Boolean Verify(String token, out Exception outEx)
        {
            Boolean isValid = false;
            outEx = null;
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                token = token.Replace("Bearer", "").Trim();
                String SecretKey = AppSettings.Secret;
                string json = decoder.Decode(token, SecretKey, verify: true);
                isValid = true;
            }
            catch (TokenExpiredException ex)
            {
                isValid = false;
                outEx = ex;
            }
            catch (SignatureVerificationException ex)
            {
                isValid = false;
                outEx = ex;
            }
            catch (Exception ex)
            {
                isValid = false;
                outEx = ex;
            }
            return isValid;
        }
    }
}

