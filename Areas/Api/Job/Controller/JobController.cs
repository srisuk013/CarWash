using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsConst;
using CarWash.Areas.Api.Models.ModelsReponse;
using CarWash.Areas.Api.Models.ModelsReq;
using CarWash.Hubs;
using CarWash.Models.DBModels;
using CarWash.Service;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using MySqlX.XDevAPI.Relational;
using System.Text;
using System.Net.Http.Headers;
using Stripe;
using Org.BouncyCastle.Crypto;

namespace CarWash.Areas.Api.Account.Controllers
{

    [Area("Api")]
    [Route("Api/Job/[Action]")]
    [Obsolete]
    [ServiceFilter(typeof(CarWashAuthorization))]

    public class JobController : CarWashController
    {
        private CarWashContext _context;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        private readonly IHostingEnvironment _env;
        private readonly IHubContext<ChatHub> HubContext;
        private static string ApiKey = "AIzaSyA0xBPLP9vDxXdbsQ1PYkBROfs4-vYvB1M";
        private static string Bucket = "carwash-1e810.appspot.com";
        private static string AuthEmail = "Srisuk013@gmail.com";
        private static string AuthPassword = "ssss1111";



        // Create IPGeolocationAPI object, passing your valid API key


        public JobController(CarWashContext context, UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager, IHostingEnvironment env, IHubContext<ChatHub> hubcontext)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            HubContext = hubcontext;

        }
        [HttpPost]
        public IActionResult DeleteServiceImage([FromBody] ReqDeleteImage req)
        {
            BaseResponse response = new BaseResponse();
            response.Success = false;
            if(req.ImageId == null)
            {
                response.Message = "ไม่ได้ส่งตัวเลขมา";
                return Json(response);
            }
            else if(req.ImageId < 1 || req.ImageId > 8)
            {
                response.Message = "ตัวเลขต้อง 1-8 เท่านั้น";
                return Json(response);
            }
            try
            {
                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                Job jobDb = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                ImageSevice imageSevice = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == jobDb.JobId).FirstOrDefault();
                if(req.ImageId == UpImage.FrontBefore)
                {
                    imageSevice.FrontBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.BackBefore)
                {
                    imageSevice.BackBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.LaftBefore)
                {
                    imageSevice.LaftBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.RightBefore)
                {
                    imageSevice.RightBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.FrontAfter)
                {
                    imageSevice.FrontAfter = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.BackAfter)
                {
                    imageSevice.BackAfter = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.LaftAfter)
                {
                    imageSevice.LaftAfter = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.RightAfter)
                {
                    imageSevice.RightAfter = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                ImageServiceReponse responses = new ImageServiceReponse();
                ServiceImage service = new ServiceImage();
                var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId);
                var serviceDb = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == imageSevice.JobId);
                service.FrontBefore = serviceDb.Select(o => o.FrontBefore).FirstOrDefault();
                service.BackBefore = serviceDb.Select(o => o.BackBefore).FirstOrDefault();
                service.LeftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                service.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                service.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                service.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                service.LeftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                service.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == jobDb.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    service.OtherImagesService.Add(otherImage);
                }
                responses.Success = true;
                responses.Message = "สำเร็จ";
                responses.ServiceImage = service;
                return Json(responses);
            }
            catch(Exception)
            {
                response.Success = false;
                response.Message = "ไม่สำเร็จ";
                return Json(response);
            }

        }
        [HttpPost]
        public IActionResult DeleteServiceOhterImage([FromBody] ReqDeleteImage req)
        {
            BaseResponse response = new BaseResponse();
            response.Success = false;
            if(req.ImageId == null)
            {
                response.Message = "ไม่ได้ใส่ตัวเลข";
                return Json(response);
            }
            try
            {
                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).Include(o => o.OthrerImage).Include(o => o.ImageSevice).OrderByDescending(o => o.JobId);
                Job userEmp = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                _context.Remove(_context.OthrerImage.Single(a => a.ImageId == req.ImageId));
                _context.SaveChanges();
                ImageServiceReponse responses = new ImageServiceReponse();
                ServiceImage service = new ServiceImage();
                ImageSevice imageSevice = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == userEmp.JobId).FirstOrDefault();
                var serviceDb = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == imageSevice.JobId);
                service.FrontBefore = serviceDb.Select(o => o.FrontBefore).FirstOrDefault();
                service.BackBefore = serviceDb.Select(o => o.BackBefore).FirstOrDefault();
                service.LeftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                service.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                service.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                service.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                service.LeftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                service.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == userEmp.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    service.OtherImagesService.Add(otherImage);
                }
                responses.Success = true;
                responses.Message = "สำเร็จ";
                responses.ServiceImage = service;
                return Json(responses);
            }
            catch(Exception)
            {
                response.Success = false;
                response.Message = "ไม่พบImageId";
                return Json(response);
            }

        }

        [HttpGet]
        public IActionResult Imageservice()
        {
            try
            {
                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                Job job = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                ImageServiceReponse response = new ImageServiceReponse();
                ImageSevice service = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == job.JobId).FirstOrDefault();
                var serviceDb = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == job.JobId);
                var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).Include(o => o.ImageSevice).OrderByDescending(o => o.JobId);
                ServiceImage imageservice = new ServiceImage();
                imageservice.FrontBefore = serviceDb.Select(o => o.FrontBefore).FirstOrDefault();
                imageservice.BackBefore = serviceDb.Select(o => o.BackBefore).FirstOrDefault();
                imageservice.LeftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                imageservice.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                imageservice.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                imageservice.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                imageservice.LeftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                imageservice.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == job.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    imageservice.OtherImagesService.Add(otherImage);
                }
                response.Success = true;
                response.Message = "สำเร็จ";
                response.ServiceImage = imageservice;
                return Json(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Uploadimageservice([FromForm] StatusServiceImage image)
        {
            try
            {
                if(image.File.Length > 0)
                {
                    byte[] fileBytes;
                    using(var ms = image.File.OpenReadStream())
                    {
                        using(var memoryStream = new MemoryStream())
                        {
                            ms.CopyTo(memoryStream);
                            fileBytes = memoryStream.ToArray();
                            var img = Image.FromStream(ms);
                            var stream = new System.IO.MemoryStream();
                            img.Save(stream, ImageFormat.Jpeg);
                            stream.Position = 0;
                            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                            var authEmailAndPassword = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                            var cancellation = new CancellationTokenSource();
                            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                            int idName = int.Parse(Id);
                            var nameImage = ServiceCheck.CheckImage(image.StatusService);
                            Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
                            OthrerImage other = new OthrerImage();
                            ImageSevice service = _context.ImageSevice.Where(o => o.JobId == job.JobId).FirstOrDefault();
                            var serviceDb = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == job.JobId);
                            string name = null;
                            DateTime date = DateTime.UtcNow.AddDays(7);
                            long unixTime = ((DateTimeOffset)date).ToUnixTimeSeconds();
                            if(image.StatusService >= 1 && image.StatusService <= 8)
                            {
                                if(image.StatusService == UpImage.FrontBefore)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.FrontBefore;
                                }
                                else if(image.StatusService == UpImage.BackBefore)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.BackBefore;
                                }
                                else if(image.StatusService == UpImage.LaftBefore)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.LaftBefore;
                                }
                                else if(image.StatusService == UpImage.RightBefore)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.RightBefore;
                                }
                                else if(image.StatusService == UpImage.FrontAfter)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.FrontAfter;
                                }
                                else if(image.StatusService == UpImage.BackAfter)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.BackAfter;
                                }
                                else if(image.StatusService == UpImage.LaftAfter)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.LaftAfter;
                                }
                                else if(image.StatusService == UpImage.BackAfter)
                                {
                                    name = job.JobId.ToString() + UpImage.Desc.RightAfter;
                                }
                            }
                            else
                            {
                                name = job.JobId + unixTime.ToString();
                            }
                            var upload = new FirebaseStorage(
                                Bucket,
                                new FirebaseStorageOptions
                                {
                                    AuthTokenAsyncFactory = () => Task.FromResult(authEmailAndPassword.FirebaseToken),
                                    ThrowOnCancel = true
                                })
                                .Child(nameImage)
                                .Child($"{(name)}.jpg")
                                .PutAsync(stream, cancellation.Token);
                            var ImageUrl = await upload;
                            if(image.StatusService >= 1 && image.StatusService <= 8)
                            {
                                if(image.StatusService == 1)
                                {
                                    service.FrontBefore = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                                else if(image.StatusService == 2)
                                {
                                    service.BackBefore = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                                else if(image.StatusService == 3)
                                {
                                    service.LaftBefore = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                                else if(image.StatusService == 4)
                                {
                                    service.RightBefore = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                                else if(image.StatusService == 5)
                                {
                                    service.FrontAfter = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                                else if(image.StatusService == 6)
                                {
                                    service.BackAfter = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                                else if(image.StatusService == 7)
                                {
                                    service.LaftAfter = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                                else if(image.StatusService == 8)
                                {
                                    service.RightAfter = ImageUrl;
                                    _context.ImageSevice.Update(service);
                                    _context.SaveChanges();
                                }
                            }
                            else if(image.StatusService == UpImage.OtherImage)
                            {
                                other.JobId = job.JobId;
                                other.Image = ImageUrl;
                                _context.OthrerImage.Add(other);
                                _context.SaveChanges();
                            }
                            var JobDbmonth = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage).Include(o => o.ImageSevice).Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).ToList();
                            ServiceImage serviceImage = new ServiceImage();
                            serviceImage.FrontBefore = serviceDb.Select(o => o.FrontBefore).FirstOrDefault();
                            serviceImage.BackBefore = serviceDb.Select(o => o.BackBefore).FirstOrDefault();
                            serviceImage.LeftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                            serviceImage.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                            serviceImage.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                            serviceImage.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                            serviceImage.LeftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                            serviceImage.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                            List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == job.JobId).ToList();
                            foreach(OthrerImage images in Jobimage)
                            {
                                OtherImage otherImage = new OtherImage();
                                otherImage.ImageId = images.ImageId;
                                otherImage.Image = images.Image;
                                serviceImage.OtherImagesService.Add(otherImage);
                            }
                            UpImageResponse response = new UpImageResponse();
                            response.Success = true;
                            response.Message = "อัพรูปสำเร็จ";
                            response.ServiceImage = serviceImage;
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
        [HttpPost]
        public IActionResult Navigation([FromBody] ReqNavigation req)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                response.Success = false;
                if(req.Latitude == null)
                {
                    response.Message = "ไม่ได้ส่งตำแหน่ง";
                    return Json(response);
                }
                else if(req.Longitude == null)
                {
                    response.Message = "ไม่ได้ส่งตำแหน่ง";
                    return Json(response);
                }
                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                CarWash.Models.DBModels.User userEmp = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
                var jobDb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId);
                userEmp.Latitude = req.Latitude;
                userEmp.Longitude = req.Longitude;
                _context.User.Update(userEmp);
                _context.SaveChanges();
                Navigation navigation = new Navigation();
                navigation.CustomerLatitude = jobDb.Select(o => o.Customer.Latitude).FirstOrDefault();
                navigation.CustomerLongitude = jobDb.Select(o => o.Customer.Longitude).FirstOrDefault();
                NavigationReponse reponse = new NavigationReponse();
                reponse.Success = true;
                reponse.Message = "สำเร็จ";
                reponse.Navigation = navigation;
                return Json(reponse);
            }
            catch(Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        public IActionResult History(long? DateBegin, long? DateEnd)
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            HistoryResponse historyResponse = new HistoryResponse();
            CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
            if(DateBegin == 0 && DateEnd == 0)
            {
                string date = DateTime.Now.ToString("ddMMyyyyHHmm");
                int month = Convert.ToInt32(date.Substring(2, 2));
                var JobDbmonth = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage)
               .Where(o => o.EmployeeId == idName).Where(o => o.JobDateTime.Month == month).ToList();
                List<JobHistory> jobDb = new List<JobHistory>();
                foreach(Job HistoryJob in JobDbmonth)
                {
                    JobHistory job = new JobHistory(HistoryJob);
                    List<ImageSevice> imageSevices = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(ImageSevice sevice in imageSevices)
                    {
                        ImageSevices imageFrontBefore = new ImageSevices();
                        imageFrontBefore.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                        job.ImagesBeforeService.Add(imageFrontBefore);
                        ImageSevices imageBackBefore = new ImageSevices();
                        imageBackBefore.Image = imageSevices.Select(o => o.BackBefore).FirstOrDefault();
                        job.ImagesBeforeService.Add(imageBackBefore);
                        ImageSevices imageLaftBefore = new ImageSevices();
                        imageLaftBefore.Image = imageSevices.Select(o => o.LaftBefore).FirstOrDefault();
                        job.ImagesBeforeService.Add(imageLaftBefore);
                        ImageSevices imageRightBefore = new ImageSevices();
                        imageRightBefore.Image = imageSevices.Select(o => o.RightBefore).FirstOrDefault();
                        job.ImagesBeforeService.Add(imageRightBefore);
                    }
                    foreach(ImageSevice sevice in imageSevices)
                    {
                        AfterImage imageFrontAfter = new AfterImage();
                        imageFrontAfter.Image = imageSevices.Select(o => o.FrontAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(imageFrontAfter);
                        AfterImage imageBackAfter = new AfterImage();
                        imageBackAfter.Image = imageSevices.Select(o => o.BackAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(imageBackAfter);
                        AfterImage imageLaftAfter = new AfterImage();
                        imageLaftAfter.Image = imageSevices.Select(o => o.LaftAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(imageLaftAfter);
                        AfterImage imageRightAfter = new AfterImage();
                        imageRightAfter.Image = imageSevices.Select(o => o.RightAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(imageRightAfter);
                    }
                    List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(OthrerImage image in Jobimage)
                    {
                        OtherImage otherImage = new OtherImage();
                        otherImage.ImageId = image.ImageId;
                        otherImage.Image = image.Image;
                        job.OtherImagesService.Add(otherImage);
                    }

                    jobDb.Add(job);
                }
                historyResponse.Success = true;
                historyResponse.Message = "สำเร็จ";
                historyResponse.Histories = jobDb;
                return Json(historyResponse);
            }
            List<JobHistory> jobdb = new List<JobHistory>();
            DateTime datebegin = ServiceCheck.DateTime(DateBegin.Value);
            DateTime dateEnd = ServiceCheck.DateTime(DateEnd.Value);
            var JobDb = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage)
           .Where(o => o.EmployeeId == idName).Where(o => o.JobDateTime.Date >= datebegin && o.JobDateTime.Date <= dateEnd).ToList();
            foreach(Job HistoryJob in JobDb)
            {
                JobHistory job = new JobHistory(HistoryJob);
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                List<ImageSevice> imageSevices = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                foreach(ImageSevice sevice in imageSevices)
                {
                    ImageSevices imageFrontBefore = new ImageSevices();
                    imageFrontBefore.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageFrontBefore);
                    ImageSevices imageBackBefore = new ImageSevices();
                    imageBackBefore.Image = imageSevices.Select(o => o.BackBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageBackBefore);
                    ImageSevices imageLaftBefore = new ImageSevices();
                    imageLaftBefore.Image = imageSevices.Select(o => o.LaftBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageLaftBefore);
                    ImageSevices imageRightBefore = new ImageSevices();
                    imageRightBefore.Image = imageSevices.Select(o => o.RightBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageRightBefore);
                }
                foreach(ImageSevice sevice in imageSevices)
                {
                    AfterImage imageFrontAfter = new AfterImage();
                    imageFrontAfter.Image = imageSevices.Select(o => o.FrontAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageFrontAfter);
                    AfterImage imageBackAfter = new AfterImage();
                    imageBackAfter.Image = imageSevices.Select(o => o.BackAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageBackAfter);
                    AfterImage imageLaftAfter = new AfterImage();
                    imageLaftAfter.Image = imageSevices.Select(o => o.LaftAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageLaftAfter);
                    AfterImage imageRightAfter = new AfterImage();
                    imageRightAfter.Image = imageSevices.Select(o => o.RightAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageRightAfter);
                }
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    job.OtherImagesService.Add(otherImage);
                }
                jobdb.Add(job);
            }
            historyResponse.Success = true;
            historyResponse.Message = "สำเร็จ";
            historyResponse.Histories = jobdb;
            return Json(historyResponse);
        }

        [HttpPost]
        public async Task<IActionResult> JobQuestionAsync()
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            JobRequset jobreq = new JobRequset();
            jobreq.JobId = job.JobId;
            jobreq.FullName = "กัญญา เนื่องบุรี";
            jobreq.ImageProfile = "https://scontent.fbkk5-4.fna.fbcdn.net/v/t1.0-9/p720x720/53656017_2208855455803733_6158198433413857280_o.jpg?_nc_cat=110&_nc_sid=110474&_nc_eui2=AeEZxZny9F1z_Es8eNXVpkL34Kgb6H0dL93gqBvofR0v3R-7JPOUsSuXE3rgjgsq98W7QpuSyxfOff3RNVrYCe8r&_nc_oc=AQkBIfTYrQlRZFmWKHQDAqTC4Ot9b2WnQOoVnDCrW3gNYFW3_PH8k06Ai_5c-up68RjbM6Wy63U9X7XwVxdvT1YZ&_nc_ht=scontent.fbkk5-4.fna&_nc_tp=6&oh=1f3c303c5fd39d4257d2dc6789123410&oe=5F0DAD45";

            var Latitude = job.Latitude;
            var Longitude = job.Longitude;
            JobRequestResponse jobRequest = new JobRequestResponse();
            jobreq.Longitude = job.Longitude;
            jobreq.PackageName = "ล้างสี";
            jobreq.VehicleRegistration = "อด285";

            string key = "&mode=t&type=25&locale=th&key=c1d2a99899af37a0e2b5b1a3a1b1088e";
            string empLongitude = "flon=" + job.Employee.Longitude.ToString(); ;
            string empLatitude = "&flat=" + job.Employee.Latitude.ToString();
            string cusLongitude = "&tlon=" + Longitude.ToString();
            string cusLatitude = "&tlat=" + Latitude.ToString();
            string Baseurl = "https://mmmap15.longdo.com/mmroute/json/route/guide?" + empLongitude + empLatitude + cusLatitude + cusLongitude + key;
            LocationReponse EmpInfo = new LocationReponse();
            using(var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync(Baseurl);
                //Checking the response is successful or not which is sent using HttpClient  
                if(Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<LocationReponse>(EmpResponse);

                }
            }
            var Distance = EmpInfo.data.Select(o => o.distance).FirstOrDefault();
            Double DistanceSum = (Distance / 1000);
            string showDistance = String.Format("{0:0.0} km", DistanceSum);
            jobreq.Distance = showDistance;
            jobreq.Price = "฿ 120.00";
            jobreq.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            jobRequest.Success = true;
            jobRequest.Message = "สำเร็จ";
            jobRequest.location = EmpInfo;
            jobRequest.Job = jobreq;
            return Json(jobRequest);
        }

        [HttpPost]

        public async Task<IActionResult> JobAnswerAsync([FromBody] ReqJobStatus status)
        {
            JobRequestResponse jobRequest = new JobRequestResponse();
            jobRequest.Success = false;
            if(status.JobStatus == null)
            {
                jobRequest.Message = "ไม่ได้ส่งJobStatus";
                return Json(jobRequest);
            }
            else if(status.JobStatus >= 0 && status.JobStatus >= 1 && status.JobStatus >= 2)
            {
                jobRequest.Message = "JobStatusไม่ถูกกต้อง";
                return Json(jobRequest);
            }
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            JobRequset jobname = new JobRequset();
            string date = DateTime.Now.ToString("ddMMyyyyHHmm");
            int month = Convert.ToInt32(date.Substring(2, 2));

            var homeScoreSum = _context.HomeScore.Include(o => o.Employee).Where(o => o.EmployeeId == idName);
            HomeScore homeScore = _context.HomeScore.Where(o => o.EmployeeId == idName).FirstOrDefault();
            var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Include(o => o.Package).Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId);
            Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            int sum = 1;
            var dateMonth = homeScoreSum.Select(o => o.CreatedTime.Month).FirstOrDefault();
            if(dateMonth != month)
            {
                homeScore.Timeout = 0;
                homeScore.Acceptance = 0;
                homeScore.Cancellation = 0;
                homeScore.MaxJob = 0;
                homeScore.Rating = 0;
                homeScore.CreatedTime = DateTime.Now;
                _context.HomeScore.Update(homeScore);
                _context.SaveChanges();
            }
            if(status.JobStatus == 0)
            {
                if(homeScoreSum.Select(o => o.Cancellation)?.FirstOrDefault() == null)
                {
                    homeScore.Cancellation = 0;
                    _context.HomeScore.Update(homeScore);
                    _context.SaveChanges();
                }
                homeScore.Cancellation = homeScoreSum.Select(o => o.Cancellation).FirstOrDefault() + sum;
                homeScore.CreatedTime = DateTime.Now;
                homeScore.MaxJob = homeScoreSum.Select(o => o.MaxJob).FirstOrDefault() + sum;
                _context.HomeScore.Update(homeScore);
                _context.SaveChanges();
                job.StatusName = JobStatus.Desc.RejectJob;
                jobRequest.Success = false;
                jobRequest.Message = "สำเร็จ";
                jobRequest.Job = null;
                _context.Job.Update(job);
                _context.SaveChanges();
                return Json(jobRequest);
            }
            else if(status.JobStatus == 1)
            {
                if(homeScoreSum.Select(o => o.Acceptance)?.FirstOrDefault() == null)
                {
                    homeScore.Acceptance = 0;
                    _context.HomeScore.Update(homeScore);
                    _context.SaveChanges();
                }
                homeScore.Acceptance = homeScoreSum.Select(o => o.Acceptance).FirstOrDefault() + sum;
                homeScore.MaxJob = homeScoreSum.Select(o => o.MaxJob).FirstOrDefault() + sum;
                _context.HomeScore.Update(homeScore);
                _context.SaveChanges();
                job.StatusName = JobStatus.Desc.ReceiveJob;
                jobname.JobId = jobdb.Select(o => o.JobId).FirstOrDefault();
                jobname.FullName = jobdb.Select(o => o.Customer.FullName).FirstOrDefault();
                jobname.Phone = jobdb.Select(o => o.Customer.Phone).FirstOrDefault();
                jobname.ImageProfile = jobdb.Select(o => o.Customer.Image).FirstOrDefault();
                jobname.Latitude = jobdb.Select(o => o.Latitude).FirstOrDefault();
                jobname.Longitude = jobdb.Select(o => o.Longitude).FirstOrDefault();
                string location = await ServiceCheck.LocationAsync(jobdb.Select(o => o.Longitude).FirstOrDefault(), jobdb.Select(o => o.Latitude).FirstOrDefault());
                jobname.Location = location;
                jobname.PackageName = jobdb.Select(o => o.Package.PackageName).FirstOrDefault();
                jobname.VehicleRegistration = jobdb.Select(o => o.Car.VehicleRegistration).FirstOrDefault();
                jobname.Price = jobdb.Select(o => o.Price.ToString()).FirstOrDefault() + ".00 ฿";
                jobname.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                jobRequest.Success = true;
                jobRequest.Message = "สำเร็จ";
                jobRequest.Job = jobname;
                _context.Job.Update(job);
                _context.SaveChanges();
                return Json(jobRequest);
            }
            else if(status.JobStatus == 2)
            {
                homeScore.Timeout = homeScoreSum.Select(o => o.Timeout).FirstOrDefault() + sum;
                _context.HomeScore.Update(homeScore);
                _context.SaveChanges();
                jobRequest.Success = false;
                jobRequest.Message = "สำเร็จ";
                jobRequest.Job = null;
                return Json(jobRequest);
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult StatusService()
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            BaseResponse response = new BaseResponse();
            Job JobStatusName = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            response.Success = true;
            response.Message = "สำเร็จ";
            JobStatusName.StatusName = JobStatus.Desc.Arrive;
            _context.Job.Update(JobStatusName);
            _context.SaveChanges();
            return Json(response);
        }
        [HttpPost]
        public IActionResult PaymentJob()
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            BaseResponse response = new BaseResponse();
            Job JobStatusName = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
            user.State = State.On;
            JobStatusName.StatusName = JobStatus.Desc.Payment;
            response.Success = true;
            response.Message = "สำเร็จ";
            JobStatusName.StatusName = JobStatus.Desc.Arrive;
            _context.User.Update(user);
            _context.Job.Update(JobStatusName);
            _context.SaveChanges();
            return Json(response);
        }

        [HttpPost]
        public IActionResult ReportJob([FromBody] ReqReport req)
        {
            BaseResponse response = new BaseResponse();
            if(String.IsNullOrEmpty(req.Report))
            {
                response.Success = false;
                response.Message = "ไม่ได้ใส่ข้อความ";
                return Json(response);
            }
            try
            {
                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
                Job job = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                job.Comment = req.Report;
                user.State = State.On;
                _context.User.Update(user);
                _context.Job.Update(job);
                _context.SaveChanges();
                response.Success = true;
                response.Message = "บันทึกข้อมูลสำเร็จ";
                return Json(response);
            }
            catch(Exception)
            {

            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CusJobAsync([FromBody] ChatMessage chat)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userId = int.Parse(Id);
                Chat chatHub = new Chat();
                chatHub.Message = chat.Message;
                chatHub.Name = chat.Name;
                _context.Chat.Update(chatHub);
                _context.SaveChanges();
                ChatMessage json = new ChatMessage();
                Chat chatDb = _context.Chat.OrderByDescending(o => o.ChatId).FirstOrDefault();
                json.ChatId = chatDb.ChatId;
                json.Name = chatDb.Name;
                json.Message = chatDb.Message;
                string result = JsonConvert.SerializeObject(json);
                await HubContext.Clients.All.SendAsync("ReceiveChat", result);

                response.Success = true;
                response.Message = "สำเร็จ";
                return Json(response);
            }
            catch(Exception)
            {
                response.Success = false;
                response.Message = "ไม่สำเร็จ";
                return Json(response);

            }

        }

        [HttpPost]

        public async Task<IActionResult> BookingJobAsync([FromBody] ReqBookingJob req)
        {

            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            Job job = new Job();
            job.CustomerId = idName;
            job.JobDateTime = DateTime.Now;
            job.PackageId = req.PackageId;
            job.CarId = req.CarId;
            job.Latitude = req.Latitude;
            job.Longitude = req.Longitude;
            job.StatusName = JobStatus.Desc.BookingJob;
            var Loc = ServiceCheck.LocationAsync(req.Longitude, req.Latitude);
            job.Location = Loc.ToString();
            BaseResponse bookingJob = new BaseResponse();
            /* _context.Job.Add(job);
             _context.SaveChanges();*/
            var user = _context.User.Include(o => o.HomeScore).Where(o => o.Role == Role.Employee && o.State == State.On).ToList();
            List<UserEmplocation> jobDb = new List<UserEmplocation>();
            foreach(CarWash.Models.DBModels.User jobrole in user)
            {
                UserEmplocation userEmplocation = new UserEmplocation(jobrole);
                double lat = userEmplocation.Latitude ??= 0;
                double lon = userEmplocation.Longitude ??= 0;
                double Location;
                Location = ServiceCheck.CalculateDistance(req.Latitude, req.Longitude, lat!, lon);
                if(Location < 10)
                {
                    // string showDistance = String.Format("{0:0.0}", Location);
                    userEmplocation.Location = Location;
                }
                var users = _context.HomeScore.Where(o => o.EmployeeId == userEmplocation.UserId).OrderByDescending(o => o.Rating);
                userEmplocation.Rating = users.Select(o => o.Rating).FirstOrDefault();
                jobDb.Add(userEmplocation);
            }
            var filteredList = jobDb.OrderByDescending(o => o.Rating).ToList();
            var Name = filteredList.OrderByDescending(o => o.Rating).FirstOrDefault();
            JobRequset jobname = new JobRequset();
            JobRequestResponse json = new JobRequestResponse();
            var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Include(o => o.Package).Where(o => o.EmployeeId == Name.UserId).OrderByDescending(o => o.JobId);
            jobname.JobId = jobdb.Select(o => o.JobId).FirstOrDefault();
            jobname.EmployeeId = jobdb.Select(o => o.EmployeeId).FirstOrDefault();
            jobname.FullName = jobdb.Select(o => o.Customer.FullName).FirstOrDefault();
            jobname.Phone = jobdb.Select(o => o.Customer.Phone).FirstOrDefault();
            jobname.ImageProfile = jobdb.Select(o => o.Customer.Image).FirstOrDefault();
            jobname.Latitude = jobdb.Select(o => o.Latitude).FirstOrDefault();
            jobname.Longitude = jobdb.Select(o => o.Longitude).FirstOrDefault();
            string vLoc = await ServiceCheck.LocationAsync(jobdb.Select(o => o.Longitude).FirstOrDefault(), jobdb.Select(o => o.Latitude).FirstOrDefault());
            string showDistance = await ServiceCheck.DistanceAsync(jobdb.Select(o => o.Longitude).FirstOrDefault(), jobdb.Select(o => o.Latitude).FirstOrDefault(), req.Longitude, req.Latitude);
            jobname.Location = await Loc;
            jobname.Distance = showDistance;
            jobname.PackageName = jobdb.Select(o => o.Package.PackageName).FirstOrDefault();
            jobname.VehicleRegistration = jobdb.Select(o => o.Car.VehicleRegistration).FirstOrDefault();
            jobname.Price = jobdb.Select(o => o.Price.ToString()).FirstOrDefault() + ".00 ฿";
            jobname.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            json.Success = true;
            json.Message = "สำเร็จ";
            json.Job = jobname;
            string result = JsonConvert.SerializeObject(json);
            await HubContext.Clients.All.SendAsync("ReceiveChat", result);
            return Json(bookingJob);
        }

    }
}

