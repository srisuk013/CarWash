using CarWash.Areas.Api.Account.Controllers;
using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsConst;
using CarWash.Areas.Api.Models.ModelsReponse;
using CarWash.Hubs;
using CarWash.Models.DBModels;
using CarWash.Service;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CarWash.Areas.Account
{
    [Obsolete]
    [Area("Api")]

    public class AccountController : CarWashController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly CarWashContext _context;
        private readonly IHostingEnvironment _env;
        private static string ApiKey = "AIzaSyA0xBPLP9vDxXdbsQ1PYkBROfs4-vYvB1M";
        private static string Bucket = "carwash-1e810.appspot.com";
        private static string AuthEmail = "Srisuk013@gmail.com";
        private static string AuthPassword = "ssss1111";


        public AccountController(CarWashContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, ServiceToken service, ServiceCheck check, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            Service = service;
            ServiceCheck = check;
            _env = env;
        }
        public string RunnigCodeId(int role)
        {
            int countRunning = _context.User.Where(o => o.Role == role
            && o.CreatedTime.Year == DateTime.Now.Year
            && o.CreatedTime.Month == DateTime.Now.Month).Count() + 1;
            string codeSum = DateTime.Now.ToString("yyMM") + countRunning.ToString().PadLeft(4, '0');
            string code = "";
            if(role == Role.Admin)
            {
                code = "Adm" + codeSum;
            }
            else if(role == Role.Customer)
            {
                code = "Cus" + codeSum;
            }
            else if(role == Role.Employee)
            {
                code = "Emp" + codeSum;
            }
            return code;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] ReqRegister req)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                response.Success = false;
                if(ServiceCheck.CheckfluFullName(req.FullName) == false)
                {
                    response.Message = "กรุณาตรวจสอบFullName";
                    return Json(response);
                }
                else if(String.IsNullOrEmpty(req.Username))
                {
                    response.Message = "กรุณากรอกUser";
                    return Json(response);
                }
                IdentityUser aspnetUserCheckDb = await _userManager.FindByNameAsync(req.Username.ToLower());
                if(req.Username.Length < 4)
                {
                    response.Message = "กรุณากรอกชื่อผู้ใช้งานมากกว่า 4 ตัวอักษร";
                    return Json(response);
                }
                else if(aspnetUserCheckDb != null)
                {
                    response.Message = "มีผู้ใช้งานแล้ว";
                    return Json(response);
                }
                else if(ServiceCheck.CheckPassWord(req.Password) == false)
                {
                    response.Message = "กรุณาตรวจสอบPassword";
                    return Json(response);
                }
                if(req.Role == Role.Employee)
                {                           
                    Models.DBModels.User idCardNumberCheck = _context.User.Where(o => o.IdCardNumber == req.IdCardNumber).FirstOrDefault();
                    if(String.IsNullOrEmpty(req.IdCardNumber))
                    {
                        response.Message = "กรุณากรอกIdCardNumber";

                        return Json(response);
                    }
                    else if(req.IdCardNumber.Length != 13)
                    {
                        response.Message = "เลขบัตรประชาชนให้ครบ13";
                        return Json(response);
                    }
                    else if(idCardNumberCheck != null)
                    {
                        response.Message = "เลขบัตรประชาชนซ้ำ";
                        return Json(response);
                    }
                    else if(ServiceCheck.VerifyPeopleID(req.IdCardNumber))
                    {
                        response.Message = "กรุณากรอกเลขบัตรประชาชนให้ถูกต้อง";
                        return Json(response);
                    }
                }
                
                if(String.IsNullOrEmpty(req.Phone))
                {
                    response.Message = "กรุณาใส่เบอร์";
                    return Json(response);
                }
                if(ServiceCheck.PhoneCheck(req.Phone) != false)
                {
                    response.Message = "กรุณาตรวจสอบเบอร์";
                    return Json(response);
                }
                else if(ServiceCheck.ValidatePhone(req.Phone) == false)
                {
                    response.Message = "กรุณาตรวจสอบเบอร์";
                    return Json(response);
                }
                Models.DBModels.User phoneCheck = _context.User.Where(o => o.Phone == req.Phone).FirstOrDefault();
                if(phoneCheck != null)
                {
                    response.Message = "เบอร์นี้มีผู้ใช้งานแล้ว";
                    return Json(response);
                }
                else if(ServiceCheck.CheckRole(req.Role) == false)
                {
                    response.Message = "ตรวจสอบRole";
                    return Json(response);
                }

                IdentityUser aspnetUser = new IdentityUser();
                aspnetUser.UserName = req.Username;
                aspnetUser.PhoneNumber = req.Phone;
                IdentityResult result = await _userManager.CreateAsync(aspnetUser, req.Password);
                if(result == IdentityResult.Success)
                {
                    string roleName = "";
                    if(req.Role == Role.Admin)
                    {
                        roleName = Role.Desc.Admin;
                    }
                    else if(req.Role == Role.Customer)
                    {
                        roleName = Role.Desc.Customer;
                    }
                    else if(req.Role == Role.Employee)
                    {
                        roleName = Role.Desc.Employee;
                    }
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(aspnetUser, roleName);
                    if(roleResult == IdentityResult.Success)
                    {

                        Models.DBModels.User user = new Models.DBModels.User();
                        user.AspNetRole = roleName;
                        user.AspNetUserId = aspnetUser.Id;
                        user.CreatedTime = DateTime.Now;
                        user.UpdatedTime = DateTime.Now;
                        user.State = State.Off;
                        user.Role = req.Role;
                        user.Status = Status.PendingApproval;
                        var CodeId = RunnigCodeId(req.Role);
                        user.Code = CodeId;
                        user.Image = await UpProfileAsync(req.file, CodeId);
                        user.FullName = req.FullName;
                        user.Username = req.Username;
                        user.Phone = req.Phone;
                        user.IdCardNumber = req.IdCardNumber;
                        _context.User.Add(user);
                        _context.SaveChanges();
                        response.Success = true;
                        response.Message = "Sign up success";
                        return Json(response);
                    }
                }
                else
                {
                    return Json(result.Errors);
                }
            }
            catch(Exception e)
            {
                IdentityUser deleteUser = await _userManager.FindByNameAsync(req.Username.ToLower());
                if(deleteUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(deleteUser);
                    Models.DBModels.User user = _context.User.Where(o => o.Username == req.Username).FirstOrDefault();
                    if(user != null)
                    {
                        _context.User.Remove(user);
                        _context.SaveChanges();
                    }
                }

                return BadRequest(e.Message);

            }
            return Ok(); ;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] ReqLogin login)
        {
            SignInResponse signInResponse = new SignInResponse();
            signInResponse.Success = false;
            try
            {
                if(String.IsNullOrEmpty(login.Username))
                {
                    signInResponse.Message = "กรุณากรอกUser";
                    return Json(signInResponse);
                }
                else if(login.Username.Length < 4)
                {
                    signInResponse.Message = "กรุณากรอกชื่อผู้ใช้งานมากกว่า 4 ตัวอักษร";
                    return Json(signInResponse);
                }
                else if(ServiceCheck.CheckPassWord(login.Password) == false)
                {
                    signInResponse.Message = "กรุณาตรวจสอบPassword";
                    return Json(signInResponse);
                }
                else if(ServiceCheck.CheckRole(login.Role) == false)
                {
                    signInResponse.Message = "กรุณาตรวจสอบRoleให้ถูกต้อง";
                    return Json(signInResponse);
                }
                IdentityUser aspnetUserCheck = await _userManager.FindByNameAsync(login.Username);
                Models.DBModels.User user = _context.User.Where(o => o.Username == login.Username && o.Role == login.Role).FirstOrDefault();
                if(user == null)
                {
                    signInResponse.Message = "กรุณาตรวจสอบUserNameและPassword";
                    return Json(signInResponse);
                }
                else if(aspnetUserCheck == null)
                {
                    signInResponse.Message = "UserNameไม่ถูกต้อง";
                    return Json(signInResponse);
                }
                Models.DBModels.User StatusCheckInActive = _context.User.Where(o => o.Username == login.Username && o.Status == Status.InActive).FirstOrDefault();
                Models.DBModels.User StatusCheckPendingApproval = _context.User.Where(o => o.Username == login.Username && o.Status == Status.PendingApproval).FirstOrDefault();
                if(StatusCheckInActive != null)
                {
                    signInResponse.Message = "User ของคุณอยู่ในสถานะ InActive ";
                    return Json(signInResponse);
                }
                else if(StatusCheckPendingApproval != null)
                {
                    signInResponse.Message = "User ของคุณอยู่ในสถานะ PendingApproval ";
                    return Json(signInResponse);
                }
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(aspnetUserCheck, login.Password, false, false);
                if(signInResult.Succeeded)
                {
                    DateTime dateToken = DateTime.UtcNow.AddDays(7);
                    long unixTime1 = ((DateTimeOffset)dateToken).ToUnixTimeSeconds();
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
                    DateTime dateRefreshToken = DateTime.UtcNow.AddDays(8);
                    long unixTime = ((DateTimeOffset)dateRefreshToken).ToUnixTimeSeconds();
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
                    signInResponse.Success = true;
                    signInResponse.Message = "เข้าสู่ระบบสำเร็จ";
                    signInResponse.Token = Service.Issue(payloadBody);
                    signInResponse.RefreshToken = Service.Issue(payloadBodyRe);
                    return Json(signInResponse);
                }
                if(signInResult.IsLockedOut)
                {
                    signInResponse.Message = ("บัญชีผู้ใช้ถูกล็อค");
                    return Json(signInResponse);
                }
                else if(signInResult.IsNotAllowed)
                {
                    return BadRequest();
                }
                else
                {
                    signInResponse.Message = "กรุณาตรวจสอบUserและpassword";
                    return Json(signInResponse);
                }
            }
            catch(Exception)
            {
                signInResponse.Message = "กรุณาตรวจสอบUserและpassword";
                signInResponse.Success = false;
                return Json(signInResponse);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                BaseResponse baseResponse = new BaseResponse();
                await _signInManager.SignOutAsync();
                baseResponse = new BaseResponse();
                baseResponse.Message = "logout สำเร็จ";
                baseResponse.Success = true;
                return Json(baseResponse);
            }
            catch(Exception)
            {

            }
            return Ok();
        }

        [HttpGet]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Userinfo()
        {
            try
            {
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                Models.DBModels.User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                UserInfo userInfo = new UserInfo();
                userInfo.UserId = user.UserId;
                userInfo.FullName = user.FullName;
                userInfo.IdCardNumber = user.IdCardNumber;
                userInfo.Phone = user.Phone;
                userInfo.Code = user.Code;
                userInfo.Image = user.Image;
                UserInfoResponse userInfoResponse = new UserInfoResponse();
                userInfoResponse.Success = true;
                userInfoResponse.Message = "สำเร็จ";
                userInfoResponse.UserInfo = userInfo;
                return Json(userInfoResponse);
            }
            catch(Exception)
            {

            }
            return Ok();
        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult ChangePhone([FromBody] ReqChangePhone reqChangePhone)
        {
            BaseResponse response = new BaseResponse();
            response.Success = false;
            if(String.IsNullOrEmpty(reqChangePhone.Phone))
            {
                response.Message = "กรุณาใส่เบอร์";
                return Json(response);
            }
            if(ServiceCheck.PhoneCheck(reqChangePhone.Phone) != false)
            {
                response.Message = "กรุณาตรวจสอบเบอร์";
                return Json(response);
            }
            else if(ServiceCheck.ValidatePhone(reqChangePhone.Phone) == false)
            {
                response.Message = "กรุณาตรวจสอบเบอร์";
                return Json(response);
            }
            Models.DBModels.User CheckPhoneDb = _context.User.Where(o => o.Phone == reqChangePhone.Phone).FirstOrDefault();
            if(CheckPhoneDb != null)
            {
                response.Message = "มีผู้ใช้งานอยู่แล้ว";
                return Json(response);
            }
            try
            {

                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                Models.DBModels.User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                user.Phone = reqChangePhone.Phone;
                _context.User.Update(user);
                _context.SaveChanges();
                response.Success = true;
                response.Message = "เปลี่ยนเบอร์แล้ว";
                return Json(response);
            }
            catch(Exception)
            {

            }
            return Ok();
        }

        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult UserLogs([FromBody] ReqUserLogs req)
        {
            BaseResponse response = new BaseResponse();
            if(req.LogsStatus != 1 && req.LogsStatus != 0)
            {
                response.Success = true;
                response.Message = "LogsStatus มีค่า=0,1เท่านั้น";
                return Json(response);
            }
            try
            {
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                UserLogs user = new UserLogs();
                if(req.LogsStatus == 1)
                {
                    user.UserId = idName;
                    user.DatetimeActiveIn = DateTime.Now;
                    _context.UserLogs.Add(user);
                }
                else if(req.LogsStatus == 0)
                {
                    UserLogs logs = _context.UserLogs.OrderByDescending(o => o.UserLogsId).FirstOrDefault();
                    logs.DatetimeActiveOut = DateTime.Now;
                    _context.UserLogs.Update(logs);
                }
                _context.SaveChanges();
                response.Success = true;
                response.Message = "สำเร็จ";
                return Json(response);

            }
            catch(Exception)
            {

            }
            return Ok();
        }

        public async Task<string> UpProfileAsync(IFormFile file, string code)
        {
            string ImageUrl = null;

            if(file.Length > 0)
            {
                byte[] fileBytes;
                using(var ms = file.OpenReadStream())
                {
                    using(var memoryStream = new MemoryStream())
                    {
                        ms.CopyTo(memoryStream);
                        fileBytes = memoryStream.ToArray();
                        Image img = Image.FromStream(ms);
                        var newImage = ServiceCheck.ResizeImage(img, 200, 200);
                        var stream = new System.IO.MemoryStream();
                        newImage.Save(stream, ImageFormat.Jpeg);
                        stream.Position = 0;
                        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                        var authEmailAndPassword = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                        var cancellation = new CancellationTokenSource();
                        var upload = new FirebaseStorage(
                            Bucket,
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(authEmailAndPassword.FirebaseToken),
                                ThrowOnCancel = true
                            })
                            .Child("Imageprofile")
                            .Child($"{(code)}.jpg")
                            .PutAsync(stream, cancellation.Token);
                        ImageUrl = await upload;
                    }
                }
            }
            return ImageUrl;
        }

        [HttpPost]
        public IActionResult CheckPhone([FromBody] ReqCheckPhone req)
        {
            BaseResponse response = new BaseResponse();
            response.Success = false;
            if(String.IsNullOrEmpty(req.Phone))
            {
                response.Message = "กรุณาใส่เบอร์";
                return Json(response);
            }
            if(ServiceCheck.PhoneCheck(req.Phone) != false)
            {
                response.Message = "กรุณาตรวจสอบเบอร์";
                return Json(response);
            }
            else if(ServiceCheck.ValidatePhone(req.Phone) == false)
            {
                response.Message = "กรุณาตรวจสอบเบอร์";
                return Json(response);
            }
            Models.DBModels.User phoneCheck = _context.User.Where(o => o.Phone == req.Phone).FirstOrDefault();
            if(phoneCheck != null)
            {
                int? Id = phoneCheck.Status;
                var Status = ServiceCheck.Check(req.Phone, Id);
                response.Message = Status;
                return Json(response);
            }
            else
            {
                response.Success = true;
                response.Message = "สำเร็จ";
                return Json(response);
            }

        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public async Task<IActionResult> changePassword([FromBody] ReqNewPassword req)
        {
            BaseResponse response = new BaseResponse();
            response.Success = false;
            if(ServiceCheck.CheckPassWord(req.OldPassword) == false)
            {
                response.Message = "กรุณาใส่รหัสผ่านเดิมให้ถูกต้อง";
                return Json(response);
            }
            else if(ServiceCheck.CheckPassWord(req.NewPassword) == false)
            {
                response.Message = "กรุณาใส่รหัสผ่าน";
                return Json(response);
            }
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            string username = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.ChangePasswordAsync(user, req.OldPassword, req.NewPassword);
            if(result.Succeeded)
            {
                response.Success = true;
                response.Message = "เปลียนรหัสสำเร็จ";
                return Json(response);
            }
            else
            {
                response.Success = false;
                response.Message = "ตรวจสอบรหัสผ่านอีกครั้ง";
                return Json(response);
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Homescore()
        {
            try
            {
                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                String date = DateTime.Now.ToString("ddMMyyyyHHmm");
                int day = Convert.ToInt32(date.Substring(0, 2));
                int month = Convert.ToInt32(date.Substring(2, 2));
                int year = Convert.ToInt32(date.Substring(4, 4));
                Job jobdb = _context.Job.Where(o => o.EmployeeId == userId).FirstOrDefault();
                var job = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId && o.JobDateTime.Year == year).FirstOrDefault();
                HomeScore homeScore = _context.HomeScore.Where(o => o.EmployeeId == userId).FirstOrDefault();
                var homeScoreSum = _context.HomeScore.Include(o => o.Employee).Where(o => o.EmployeeId == userId);
                var dateDay = homeScoreSum.Select(o => o.CreatedTime.Day).FirstOrDefault();
                var dateMonth = homeScoreSum.Select(o => o.CreatedTime.Month).FirstOrDefault();
                var dateYear = homeScoreSum.Select(o => o.CreatedTime.Year).FirstOrDefault();
                HomeScoreModel model = new HomeScoreModel();
                HomeScoreResponse home = new HomeScoreResponse();
                if(dateMonth == 0)
                {
                    HomeScore newhomescore = new HomeScore();
                    newhomescore.Acceptance = 0;
                    newhomescore.Cancellation = 0;
                    newhomescore.MaxJob = 0;
                    newhomescore.Rating = 5.00;
                    newhomescore.Timeout = 0;
                    newhomescore.CreatedTime = DateTime.Now;
                    _context.HomeScore.Update(newhomescore);
                    _context.SaveChanges();
                    home.Success = true;
                    home.Message = "สำเร็จ";
                    model.Ratings = "5.0";
                    model.Acceptance = "0 %";
                    model.Cancellation = "0 %";
                    home.HomeScore = model;
                    return Json(home);
                }
                else if(dateYear != year)
                {
                    homeScore.Timeout = 0;
                    homeScore.Acceptance = 0;
                    homeScore.Cancellation = 0;
                    homeScore.MaxJob = 0;
                    homeScore.Score = 0;
                    homeScore.Rating = 5.00;
                    homeScore.CreatedTime = DateTime.Now;
                    _context.HomeScore.Update(homeScore);
                    _context.SaveChanges();
                    model.Acceptance = "0 %";
                    model.Cancellation = "0 %";
                    model.Ratings = "5.0";
                    home.Success = true;
                    home.Message = "สำเร็จ";
                    home.HomeScore = model;
                    return Json(home);
                }
                else if(dateMonth == month && dateYear == year)
                {
                    var jobSum = job.JobId.ToString().Count();
                    var CancellationSum = homeScoreSum.Select(o => o.Cancellation).FirstOrDefault();
                    var AcceptanceSum = homeScoreSum.Select(o => o.Acceptance).FirstOrDefault();
                    var jobMaxSum = homeScoreSum.Select(o => o.MaxJob).FirstOrDefault();
                    var ScoreSum = homeScoreSum.Select(o => o.Score).FirstOrDefault();
                    var rating = homeScoreSum.Select(o => o.Rating)?.FirstOrDefault();
                    if(rating == null)
                    {
                        homeScore.Rating = 0;
                        _context.HomeScore.Update(homeScore);
                        _context.SaveChanges();
                    }
                    double? ratings = ((ScoreSum / jobSum));
                    string showratings = String.Format("{0:0}", ratings);
                    model.Ratings = showratings + ".00";
                    double? Acceptances = ((AcceptanceSum / jobMaxSum) * 100);
                    string showAccptances = String.Format("{0:0.00}%", Acceptances);
                    model.Acceptance = showAccptances;
                    var Cancellations = ((CancellationSum / jobMaxSum) * 100);
                    string showCancellations = String.Format("{0:0.00}%", Cancellations);
                    model.Cancellation = showCancellations;
                    home.Success = true;
                    home.Message = "สำเร็จ";
                    home.HomeScore = model;
                    return Json(home);
                }
            }
            catch(Exception)
            {

            }

            return Ok();
        }
        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Location([FromBody] ReqLocation location)
        {
            BaseResponse response = new BaseResponse();
            if(location.latitude == null)
            {
                response.Success = false;
                response.Message = "ไม่ได้ใส่่ตำแหน่ง";
                return Json(response);
            }
            else if(location.longitude == null)
            {
                response.Success = false;
                response.Message = "ไม่ได้ใส่่ตำแหน่ง";
                return Json(response);
            }
            try
            {
                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                Models.DBModels.User user = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
                user.Latitude = location.latitude;
                user.Longitude = location.longitude;
                _context.User.Update(user);
                _context.SaveChanges();
                response.Success = true;
                response.Message = "บันทึกตำแหน่ง";
                return Json(response);
            }
            catch(Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult SwitchSystem([FromBody] ReqSwitchSystem req)
        {
            BaseResponse response = new BaseResponse();
            response.Success = false;
            if(req.State == null)
            {
                response.Message = "ไม่ได้ส่งค่าState";
                return Json(response);
            }
            if(req.State != 1 && req.State != 0)
            {
                response.Message = "State มีค่า=0,1เท่านั้น";
                return Json(response);
            }
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = int.Parse(Id);
            Models.DBModels.User user = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
            if(req.State == State.Off)
            {
                user.State = State.Off;
            }
            else if(req.State == State.On)
            {
                user.State = State.On;
            }
            _context.User.Update(user);
            _context.SaveChanges();
            response.Success = true;
            response.Message = "สำเร็จ";
            return Json(response);

        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public async Task<IActionResult> ChangeProfile([FromForm] IFormFile file)
        {
            try
            {
                if(file.Length > 0)
                {
                    byte[] fileBytes;
                    using(var ms = file.OpenReadStream())
                    {
                        using(var memoryStream = new MemoryStream())
                        {
                            ms.CopyTo(memoryStream);
                            fileBytes = memoryStream.ToArray();
                            Image img = Image.FromStream(ms);
                            var newImage = ServiceCheck.ResizeImage(img, 200, 200);
                            var stream = new System.IO.MemoryStream();
                            newImage.Save(stream, ImageFormat.Jpeg);
                            stream.Position = 0;
                            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                            var authEmailAndPassword = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                            var cancellation = new CancellationTokenSource();
                            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                            String Code = User.FindFirst(ClaimTypes.PostalCode).Value;
                            var upload = new FirebaseStorage(
                                Bucket,
                                new FirebaseStorageOptions
                                {
                                    AuthTokenAsyncFactory = () => Task.FromResult(authEmailAndPassword.FirebaseToken),
                                    ThrowOnCancel = true
                                })
                                .Child("Imageprofile")
                                .Child($"{(Code)}.jpg")
                                .PutAsync(stream, cancellation.Token);
                            var ImageUrl = await upload;
                            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                            int idName = int.Parse(Id);
                            CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                            user.Image = ImageUrl;
                            _context.User.Update(user);
                            _context.SaveChanges();
                            BaseResponse response = new BaseResponse();
                            response.Success = true;
                            response.Message = "เปลี่ยนรูปสำเร็จ";
                            return Json(response);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();
        }

    }
}







































































































