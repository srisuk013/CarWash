﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using User = CarWash.Models.DBModels.User;
using CarWash.Service;
using System.Drawing;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]
    public class ProfileController : Controller
    {
        private readonly IHostingEnvironment _env;
        private CarWashContext _context;
        private static string ApiKey = "AIzaSyA0xBPLP9vDxXdbsQ1PYkBROfs4-vYvB1M";
        private static string Bucket = "carwash-1e810.appspot.com";
        private static string AuthEmail = "Srisuk013@gmail.com";
        private static string AuthPassword = "ssss1111";

        public ProfileController(CarWashContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;

        }
        [HttpPost]
        [Route("/api/ChangeProfile")]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public async Task<IActionResult> ChangeProfileAsync([FromForm] IFormFile file)
        {
            FileStream fs = null;
            if(file.Length > 0)
            {
                string folderName = "FirebaseFiles";
                string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
                if(Directory.Exists(path))
                {
                    using(fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                   /*     using(var img = Image.FromStream(fs))
                        {
                            var newImage = ServiceCheck.ResizeImage(img, 200, 200);
                        }*/
                    }
                    fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
                }
                else
                {
                    Directory.CreateDirectory(path);
                }
                /*var img = Image.FromStream(fs);
                var newImage = ServiceCheck.ResizeImage(img, 200, 200);*/
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                var cancellation = new CancellationTokenSource();
                var upload = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
                    .Child("Imageprofile")
                    .Child($"{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}")
                    .PutAsync(fs, cancellation.Token);
                try
                {
                    
                    var ImageUrl = await upload;
                    string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                    String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    int idName = int.Parse(Id);
                    User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                    user.Image = ImageUrl;
                    _context.User.Update(user);
                    _context.SaveChanges();
                    BaseResponse response = new BaseResponse();
                    response.Success = true;
                    response.Message = "เปลี่ยนรูปสำเร็จ";
                    return Json(response);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"******************{ex}*******************");
                    ViewBag.error = $"Exception was thrown: {ex}";
                    throw;
                }
            }
            return BadRequest();
        }


    }

}