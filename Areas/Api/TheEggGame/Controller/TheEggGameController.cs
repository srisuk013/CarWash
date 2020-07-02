using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarWash.Areas.Api.Account.Controllers;
using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.TheEggGame.Model;
using CarWash.Models.DBModels;
using CarWash.Service;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Areas.Api.TheEggGame.Controllers
{
    [Obsolete]
    [Area("Api")]
  

    public class TheEggGameController : CarWashController
    {
    
        private readonly IHostingEnvironment _env;
        private static string ApiKey = "AIzaSyA0xBPLP9vDxXdbsQ1PYkBROfs4-vYvB1M";
        private static string Bucket = "carwash-1e810.appspot.com";
        private static string AuthEmail = "Srisuk013@gmail.com";
        private static string AuthPassword = "ssss1111";


        public TheEggGameController(ServiceToken service, ServiceCheck check, IHostingEnvironment env)
        {
            Service = service;
            ServiceCheck = check;
            _env = env;
        }

        [HttpPost]
       // [Route("Api/TheEggGame/UpImage")]
      
        public async Task<IActionResult> UpImage([FromForm] IFormFile file)
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
                            DateTime date = DateTime.UtcNow.AddDays(7);
                            long unixTime = ((DateTimeOffset)date).ToUnixTimeSeconds();
                            var upload = new FirebaseStorage(
                                Bucket,
                                new FirebaseStorageOptions
                                {
                                    AuthTokenAsyncFactory = () => Task.FromResult(authEmailAndPassword.FirebaseToken),
                                    ThrowOnCancel = true
                                })
                                .Child("TEGImage")
                                .Child($"{(unixTime)}.jpg")
                                .PutAsync(stream, cancellation.Token);
                            var ImageUrl = await upload;
                            Reqpones response = new Reqpones();
                            response.Success = true;
                            response.Message = "เพิ่มรูปสำเร็จ";
                            response.UrlImage = ImageUrl;
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