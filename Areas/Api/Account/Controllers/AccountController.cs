using CarWash.Areas.Api.Account.Controllers;
using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using CarWash.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using User = CarWash.Models.DBModels.User;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using Firebase.Storage;
using System.Threading;
using System.Diagnostics;
using Firebase.Auth;
using Microsoft.AspNetCore.Hosting;

namespace CarWash.Areas.Account
{
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
                IdentityUser aspnetUserCheck = await _userManager.FindByNameAsync(req.Username.ToLower());
                if(req.Username.Length < 4)
                {
                    response.Message = "กรุณากรอกชื่อผู้ใช้งานมากกว่า 4 ตัวอักษร";
                    return Json(response);
                }
                else if(aspnetUserCheck != null)
                {
                    response.Message = "มีผู้ใช้งานแล้ว";
                    return Json(response);
                }
                else if(ServiceCheck.CheckPassWord(req.Password) == false)
                {
                    response.Message = "กรุณาตรวจสอบPassword";
                    return Json(response);
                }
                User idCardNumberCheck = _context.User.Where(o => o.IdCardNumber == req.IdCardNumber).FirstOrDefault();
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
                else if(ServiceCheck.PhoneCheck(req.Phone) == false)
                {
                    response.Message = "ตรวจสอบเบอร์";
                    return Json(response);
                }
                User phoneCheck = _context.User.Where(o => o.Phone == req.Phone).FirstOrDefault();
                if(phoneCheck != null)
                {
                    response.Message = "เบอร์นี้มีผู้ใช้งานแล้ว";
                    return Json(response);
                }
                if(ServiceCheck.CheckRole(req.Role) == false)
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
                        User user = new User();
                        user.AspNetRole = roleName;
                        user.AspNetUserId = aspnetUser.Id;
                        user.CreatedTime = DateTime.Now;
                        user.UpdatedTime = DateTime.Now;
                        user.State = State.Offline;
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
                    User user = _context.User.Where(o => o.Username == req.Username).FirstOrDefault();
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
                User user = _context.User.Where(o => o.Username == login.Username && o.Role == login.Role).FirstOrDefault();
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
                User StatusCheck = _context.User.Where(o => o.Username == login.Username && o.Status == Status.InActive).FirstOrDefault();
                User StatusCheck1 = _context.User.Where(o => o.Username == login.Username && o.Status == Status.PendingApproval).FirstOrDefault();
                if(StatusCheck != null)
                {
                    return BadRequest();
                }
                else if(StatusCheck1 != null)
                {
                    return BadRequest();

                }
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(aspnetUserCheck, login.Password, false, false);
                if(signInResult.Succeeded)
                {
                    DateTime date = DateTime.UtcNow.AddDays(7);
                    long unixTime1 = ((DateTimeOffset)date).ToUnixTimeSeconds();
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
                    DateTime date1 = DateTime.UtcNow.AddDays(8);
                    long unixTime = ((DateTimeOffset)date1).ToUnixTimeSeconds();
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
            catch(Exception e)
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
                User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
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
            catch(Exception e)
            {

            }
            return Ok();
        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult ChangePhone([FromBody] ReqChangePhone reqChangePhone)
        {

            if(ServiceCheck.PhoneCheck(reqChangePhone.Phone) == false)
            {
                return BadRequest();
            }
            User phoneCheck = _context.User.Where(o => o.Phone == reqChangePhone.Phone).FirstOrDefault();
            if(phoneCheck != null)
            {
                return BadRequest();
            }
            try
            {

                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                user.Phone = reqChangePhone.Phone;
                _context.User.Update(user);
                _context.SaveChanges();
                BaseResponse response = new BaseResponse();
                response.Success = true;
                response.Message = "เปลี่ยนเบอร์แล้ว";
                return Json(response);
            }
            catch(Exception e)
            {

            }
            return Ok();
        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult SwitchSystem([FromBody] ReqSwitchSystem req)
        {
            if(ServiceCheck.CheckState(req.State) == false)
            {
                return BadRequest();
            }
            try
            {
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                user.State = req.State;
                _context.User.Update(user);
                _context.SaveChanges();
                BaseResponse response = new BaseResponse();
                response.Success = true;
                response.Message = "เปลียนstateสำเร็จ";
                return Json(response);
            }
            catch(Exception e)
            {

            }
            return Ok();
        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Report([FromBody] ReqReport req)
        {
            try
            {
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                Job job = _context.Job.Where(o => o.EmployeeId == idName).FirstOrDefault();
                job.Report = req.Report;
                _context.Job.Update(job);
                _context.SaveChanges();
                BaseResponse response = new BaseResponse();
                response.Success = true;
                response.Message = "แจ้งReportสำเร็จ";
                return Json(response);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult UserLogs([FromBody] ReqUserLogs req)
        {
            if(String.IsNullOrEmpty(req.LogsKeys.ToString()))
            {
                return BadRequest();
            }
            else if(String.IsNullOrEmpty(req.LogsStatus.ToString()))
            {
                return BadRequest();
            }
            else if(req.LogsStatus != 1 && req.LogsStatus != 0)
            {
                return BadRequest();
            }
            try
            {
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                UserLogs user = new UserLogs();
                if(req.LogsStatus == 1)
                {
                    user.UserId = idName;
                    user.LogsKey = req.LogsKeys;
                    user.DatetimeActiveIn = DateTime.Now;
                    _context.UserLogs.Add(user);
                }
                else if(req.LogsStatus == 0)
                {
                    UserLogs logs = _context.UserLogs.Where(o => o.LogsKey == req.LogsKeys).FirstOrDefault();
                    logs.DatetimeActiveOut = DateTime.Now;
                    _context.UserLogs.Update(logs);
                }
                _context.SaveChanges();
                BaseResponse response = new BaseResponse();
                response.Success = true;
                response.Message = "สำเร็จ";
                return Json(response);

            }
            catch(Exception e)
            {

            }
            return Ok();
        }

        public async Task<string> UpProfileAsync(IFormFile file, string code)
        {
            string ImageUrl = null;
            FileStream fs = null;
            if(file.Length > 0)
            {
                string folderName = "FirebaseFilesV1";
                string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");

                if(Directory.Exists(path))
                {
                    using(fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }
                    fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
                }
                else
                {
                    Directory.CreateDirectory(path);
                }
                var img = Image.FromStream(fs);
                Size size = new Size(200, 200);
                var newImage = ServiceCheck.resizeImage(img, size);
                var stream = new System.IO.MemoryStream();
                newImage.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                var cancellation = new CancellationTokenSource();
                var upload = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("Imageprofile")
                    .Child($"{(code)}.jpg")
                    .PutAsync(stream, cancellation.Token);
                ImageUrl = await upload;
            }
            return ImageUrl;
        }

        [HttpPost]
        [Route("/api/ChangeProfile")]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public async Task<IActionResult> ChangeProfileAsync([FromForm] IFormFile file)
        {
            FileStream fs = null;
            try
            {
                if(file.Length > 0)
                {
                    string folderName = "FirebaseFilesV1";
                    string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");

                    if(Directory.Exists(path))
                    {
                        using(fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                        fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }

                    Image img = Image.FromStream(fs);
                    Size size = new Size(200, 200);
                    var newImage = ServiceCheck.resizeImage(img, size);
                    var stream = new System.IO.MemoryStream();
                    newImage.Save(stream, ImageFormat.Jpeg);
                    stream.Position = 0;
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                    var cancellation = new CancellationTokenSource();
                    string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                    String Code = User.FindFirst(ClaimTypes.PostalCode).Value;
                    var upload = new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
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
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();
        }
    }
}







































































































