using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsConst;
using CarWash.Areas.Api.Models.ModelsReponse;
using CarWash.Areas.Api.Models.ModelsReq;
using CarWash.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Claims;
using System.Threading;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]
    [Route("Api/Job/[Action]")]
    [Obsolete]
    [ServiceFilter(typeof(CarWashAuthorization))]
    public class JobController : CarWashController
    {
        private CarWashContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHubContext<ChatHub> HubContext;
        private readonly IHubContext<CarWashEmployeeHup> EmployeeHup;
        private static string ApiKey = "AIzaSyA0xBPLP9vDxXdbsQ1PYkBROfs4-vYvB1M";
        private static string Bucket = "carwash-1e810.appspot.com";
        private static string AuthEmail = "Srisuk013@gmail.com";
        private static string AuthPassword = "ssss1111";
        public JobController(CarWashContext context, UserManager<IdentityUser> userManager,
     SignInManager<IdentityUser> signInManager, IHostingEnvironment env, IHubContext<ChatHub> hubcontext, IHubContext<CarWashEmployeeHup> employeeHup, IHubContext<TimeHub> timeHup)
        {
            _context = context;
            _env = env;
            HubContext = hubcontext;
            EmployeeHup = employeeHup;
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
                int userId = IdUser();
                Job jobDb = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                ImageService imageSevice = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == jobDb.JobId).FirstOrDefault();
                switch(req.ImageId)
                {
                    case 1:
                        imageSevice.FrontBefore = null;
                        _context.SaveChanges();
                        break;
                    case 2:
                        imageSevice.BackBefore = null;
                        _context.SaveChanges();
                        break;
                    case 3:
                        imageSevice.FrontBefore = null;
                        _context.SaveChanges();
                        break;
                    case 4:
                        imageSevice.BackBefore = null;
                        _context.SaveChanges();
                        break;
                    case 5:
                        imageSevice.FrontBefore = null;
                        _context.SaveChanges();
                        break;
                    case 6:
                        imageSevice.BackBefore = null;
                        _context.SaveChanges();
                        break;
                    case 7:
                        imageSevice.FrontBefore = null;
                        _context.SaveChanges();
                        break;
                    case 8:
                        imageSevice.BackBefore = null;
                        _context.SaveChanges();
                        break;
                }
                ImageServiceResponse responses = new ImageServiceResponse();
                var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId);
                ImageService serviceDb = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == imageSevice.JobId).FirstOrDefault();
                ServiceImage service = new ServiceImage(serviceDb);
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == jobDb.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage(image);
                    service.OtherImagesService.Add(otherImage);
                }
                responses.Success = true;
                responses.Message = "สำเร็จ";
                responses.Service = service;
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
                int userId = IdUser();
                var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).Include(o => o.OthrerImage).Include(o => o.ImageService).OrderByDescending(o => o.JobId);
                Job userEmp = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                _context.Remove(_context.OthrerImage.Single(a => a.ImageId == req.ImageId));
                _context.SaveChanges();
                ImageServiceResponse responses = new ImageServiceResponse();
                ImageService imageSevice = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == userEmp.JobId).FirstOrDefault();
                var serviceDb = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == imageSevice.JobId).FirstOrDefault();
                ServiceImage service = new ServiceImage(serviceDb);
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == userEmp.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage(image);
                    service.OtherImagesService.Add(otherImage);
                }
                responses.Success = true;
                responses.Message = "สำเร็จ";
                responses.Service = service;
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
                ImageServiceResponse response = new ImageServiceResponse();
                int userId = IdUser();
                Job job = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                ImageService updateimage = _context.ImageService.Where(o => o.JobId == job.JobId).FirstOrDefault();
                ImageService serviceDb = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == job.JobId).FirstOrDefault();
                int? imageid = null;
                if(updateimage == null)
                {
                    imageid = ImageidDB(job.JobId);
                }
                else
                {
                    imageid = updateimage.ImageId;
                }
                ServiceImage imageservice = serviceDb != null ? imageservice = new ServiceImage(serviceDb) : imageservice = new ServiceImage();

                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == job.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage(image);
                    imageservice.OtherImagesService.Add(otherImage);
                }
                response.Success = true;
                response.Message = "สำเร็จ";
                response.JobId = job.JobId;
                response.ImageId = imageid;
                response.Service = imageservice;
                return Json(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
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
                            var stream = new MemoryStream();
                            img.Save(stream, ImageFormat.Jpeg);
                            stream.Position = 0;
                            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                            var authEmailAndPassword = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                            var cancellation = new CancellationTokenSource();
                            
                            int userId = int.Parse(Id);
                            var nameImage = ServiceCheck.CheckImage(image.StatusService);
                            Job job = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                            OthrerImage other = new OthrerImage();
                            ImageService serviceDb = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == job.JobId).FirstOrDefault();
                            string name = job.JobId.ToString();
                            DateTime date = DateTime.UtcNow.AddDays(7);
                            long unixTime = ((DateTimeOffset)date).ToUnixTimeSeconds();
                            if(image.StatusService >= 1 && image.StatusService <= 8)
                            {
                                switch(image.StatusService)
                                {
                                    case UpImage.FrontBefore:
                                        name += UpImage.Desc.FrontBefore;
                                        break;
                                    case UpImage.BackBefore:
                                        name += UpImage.Desc.BackBefore;
                                        break;
                                    case UpImage.LeftBefore:
                                        name += UpImage.Desc.LeftBefore;
                                        break;
                                    case UpImage.RightBefore:
                                        name += UpImage.Desc.RightBefore;
                                        break;
                                    case UpImage.FrontAfter:
                                        name += UpImage.Desc.FrontAfter;
                                        break;
                                    case UpImage.BackAfter:
                                        name += UpImage.Desc.BackAfter;
                                        break;
                                    case UpImage.LeftAfter:
                                        name += UpImage.Desc.LeftAfter;
                                        break;
                                    case UpImage.RightAfter:
                                        name += UpImage.Desc.RightAfter;
                                        break;
                                }
                            }
                            else
                            {
                                name += unixTime.ToString();
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
                            ImageService servicedb = _context.ImageService.Where(o => o.ImageId == image.ImageId).FirstOrDefault();
                            if(image.StatusService >= 1 && image.StatusService <= 8)
                            {
                                switch(image.StatusService)
                                {
                                    case 1:
                                        servicedb.FrontBefore = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                    case 2:
                                        servicedb.BackBefore = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                    case 3:
                                        servicedb.LeftBefore = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                    case 4:
                                        servicedb.RightBefore = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                    case 5:
                                        servicedb.FrontAfter = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                    case 6:
                                        servicedb.BackAfter = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                    case 7:
                                        servicedb.LeftAfter = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                    case 8:
                                        servicedb.RightAfter = ImageUrl;
                                        _context.ImageService.Update(servicedb);
                                        _context.SaveChanges();
                                        break;
                                }
                            }
                            else if(image.StatusService == UpImage.OtherImage)
                            {
                                other.JobId = job.JobId;
                                other.Image = ImageUrl;
                                _context.OthrerImage.Add(other);
                                _context.SaveChanges();
                            }
                            var JobDbmonth = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage).Include(o => o.ImageService).Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).ToList();
                            ServiceImage serviceImage = new ServiceImage(serviceDb);
                            List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == job.JobId).ToList();
                            foreach(OthrerImage images in Jobimage)
                            {
                                OtherImage otherImage = new OtherImage(images);
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
                return BadRequest(ex.Message);
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
<<<<<<< HEAD
                int userId = UserId();
=======
                int userId = IdUser();
>>>>>>> DevOps-Dev
                CarWash.Models.DBModels.User userEmp = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
                var jobDb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                userEmp.Latitude = req.Latitude;
                userEmp.Longitude = req.Longitude;
                _context.User.Update(userEmp);
                _context.SaveChanges();
                Navigation navigation = new Navigation();
                navigation.CustomerLatitude = jobDb.Latitude;
                navigation.CustomerLongitude = jobDb.Longitude;
                NavigationResponse reponse = new NavigationResponse();
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
<<<<<<< HEAD
            int userId = UserId();
            HistoryResponse historyResponse = new HistoryResponse();
            CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
=======
            int UserId = IdUser();
            HistoryResponse historyResponse = new HistoryResponse();
            CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == UserId).FirstOrDefault();
>>>>>>> DevOps-Dev
            if(DateBegin == 0 && DateEnd == 0)
            {
                string date = DateTime.Now.ToString("ddMMyyyyHHmm");
                int month = Convert.ToInt32(date.Substring(2, 2));
                var JobDbmonth = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Package.ModelPackage).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage)
<<<<<<< HEAD
               .Where(o => o.EmployeeId == userId && o.JobDateTime.Month == month && o.Report == null).ToList();
=======
               .Where(o => o.EmployeeId == UserId && o.JobDateTime.Month == month && o.Report == null).ToList();
>>>>>>> DevOps-Dev
                List<JobHistory> jobDb = new List<JobHistory>();
                foreach(Job HistoryJob in JobDbmonth)
                {
                    JobHistory job = new JobHistory(HistoryJob);
                    List<ImageService> imageServices = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(ImageService sevice in imageServices)
                    {
                        ImageServicesModel imageFrontBefore = new ImageServicesModel();
                        imageFrontBefore.Image = sevice.FrontBefore;
                        job.ImagesBeforeService.Add(imageFrontBefore);
                        ImageServicesModel imageBackBefore = new ImageServicesModel();
                        imageBackBefore.Image = sevice.BackBefore;
                        job.ImagesBeforeService.Add(imageBackBefore);
                        ImageServicesModel imageLeftBefore = new ImageServicesModel();
                        imageLeftBefore.Image = sevice.LeftBefore;
                        job.ImagesBeforeService.Add(imageLeftBefore);
                        ImageServicesModel imageRightBefore = new ImageServicesModel();
                        imageRightBefore.Image = sevice.RightBefore;
                        job.ImagesBeforeService.Add(imageRightBefore);
                    }
                    foreach(ImageService sevice in imageServices)
                    {
                        AfterImage imageFrontAfter = new AfterImage();
                        imageFrontAfter.Image = sevice.FrontAfter;
                        job.ImagesAfterService.Add(imageFrontAfter);
                        AfterImage imageBackAfter = new AfterImage();
                        imageBackAfter.Image = sevice.BackAfter;
                        job.ImagesAfterService.Add(imageBackAfter);
                        AfterImage imageLeftAfter = new AfterImage();
                        imageLeftAfter.Image = sevice.LeftAfter;
                        job.ImagesAfterService.Add(imageLeftAfter);
                        AfterImage imageRightAfter = new AfterImage();
                        imageRightAfter.Image = sevice.RightAfter;
                        job.ImagesAfterService.Add(imageRightAfter);
                    }
                    List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(OthrerImage image in Jobimage)
                    {
                        OtherImage otherImage = new OtherImage(image);
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
            var JobDb = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage).Include(o => o.Package.ModelPackage)
<<<<<<< HEAD
           .Where(o => o.EmployeeId == userId).Where(o => o.JobDateTime.Date >= datebegin && o.JobDateTime.Date <= dateEnd).Where(o => o.Report == null).ToList();
=======
           .Where(o => o.EmployeeId == UserId).Where(o => o.JobDateTime.Date >= datebegin && o.JobDateTime.Date <= dateEnd).Where(o => o.Report == null).ToList();
>>>>>>> DevOps-Dev
            foreach(Job HistoryJob in JobDb)
            {
                if(HistoryJob == null)
                {
                    BaseResponse response = new BaseResponse();
                    response.Message = "ไม่พบข้อมูล";
                    response.Success = false;
                    return Json(response);
                }
                JobHistory job = new JobHistory(HistoryJob);
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                List<ImageService> imageSevices = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                foreach(ImageService sevice in imageSevices)
                {
                    ImageServicesModel imageFrontBefore = new ImageServicesModel();
                    imageFrontBefore.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageFrontBefore);
                    ImageServicesModel imageBackBefore = new ImageServicesModel();
                    imageBackBefore.Image = imageSevices.Select(o => o.BackBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageBackBefore);
                    ImageServicesModel imageLaftBefore = new ImageServicesModel();
                    imageLaftBefore.Image = imageSevices.Select(o => o.LeftBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageLaftBefore);
                    ImageServicesModel imageRightBefore = new ImageServicesModel();
                    imageRightBefore.Image = imageSevices.Select(o => o.RightBefore).FirstOrDefault();
                    job.ImagesBeforeService.Add(imageRightBefore);
                }
                foreach(ImageService sevice in imageSevices)
                {
                    AfterImage imageFrontAfter = new AfterImage();
                    imageFrontAfter.Image = imageSevices.Select(o => o.FrontAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageFrontAfter);
                    AfterImage imageBackAfter = new AfterImage();
                    imageBackAfter.Image = imageSevices.Select(o => o.BackAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageBackAfter);
                    AfterImage imageLaftAfter = new AfterImage();
                    imageLaftAfter.Image = imageSevices.Select(o => o.LeftAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageLaftAfter);
                    AfterImage imageRightAfter = new AfterImage();
                    imageRightAfter.Image = imageSevices.Select(o => o.RightAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(imageRightAfter);
                }
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage(image);
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
        public IActionResult JobAnswerAsync([FromBody] ReqJobStatus status)
        {
            JobRequestResponse jobRequest = new JobRequestResponse();
            jobRequest.Success = false;
            if(status.JobStatus == null)
            {
                jobRequest.Message = "ไม่ได้ส่งJobStatus";
                return Json(jobRequest);
            }
            else if(status.JobStatus != 0 && status.JobStatus != 2 && status.JobStatus != 1)
            {
                jobRequest.Message = "JobStatusไม่ถูกกต้อง";
                return Json(jobRequest);
            }
<<<<<<< HEAD
            int userId = UserId();
            JobRequset jobrequset = new JobRequset();
            string date = DateTime.Now.ToString("ddMMyyyyHHmm");
            int month = Convert.ToInt32(date.Substring(2, 2));
            HomeScore homeScoreSum = _context.HomeScore.Include(o => o.Employee).Where(o => o.EmployeeId == userId).FirstOrDefault();
            var homeScore = _context.HomeScore.Where(o => o.EmployeeId == userId).FirstOrDefault();
            Job job = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
=======
            int UserId = IdUser();
            JobRequset jobrequset = new JobRequset();
            string date = DateTime.Now.ToString("ddMMyyyyHHmm");
            int month = Convert.ToInt32(date.Substring(2, 2));
            HomeScore homeScoreSum = _context.HomeScore.Include(o => o.Employee).Where(o => o.EmployeeId == UserId).FirstOrDefault();
            var homeScore = _context.HomeScore.Where(o => o.EmployeeId == UserId).FirstOrDefault();
            Job job = _context.Job.Where(o => o.EmployeeId == UserId).OrderByDescending(o => o.JobId).FirstOrDefault();
>>>>>>> DevOps-Dev
            int sum = 1;
            var dateMonth = homeScoreSum.CreatedTime.Month;
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
                if(homeScoreSum.Cancellation == null)
                {
                    homeScore.Cancellation = 0;
                    _context.HomeScore.Update(homeScore);
                    _context.SaveChanges();
                }
<<<<<<< HEAD
                var userstate = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
=======
                var userstate = _context.User.Where(o => o.UserId == UserId).FirstOrDefault();
>>>>>>> DevOps-Dev
                userstate.State = State.On;
                homeScore.Cancellation = homeScoreSum.Cancellation + sum;
                homeScore.CreatedTime = DateTime.Now;
                homeScore.MaxJob = homeScoreSum.MaxJob + sum;
                _context.SaveChanges();
                job.StatusName = JobStatus.Desc.RejectJob;
                jobRequest.Success = false;
                jobRequest.Message = "สำเร็จ";
                jobRequest.Job = null;
                _context.SaveChanges();
                return Json(jobRequest);
            }
            else if(status.JobStatus == 1)
            {
                if(homeScoreSum.Acceptance == null)
                {
                    homeScore.Acceptance = 0;
                    _context.HomeScore.Update(homeScore);
                    _context.SaveChanges();
                }
                BaseResponse response = new BaseResponse();
                var joudb = _context.Job.Where(o => o.JobId == status.JobId).FirstOrDefault();
<<<<<<< HEAD
                joudb.EmployeeId = userId;
                _context.Job.Update(joudb);
                var user = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
=======
                joudb.EmployeeId = UserId;
                _context.Job.Update(joudb);
                var user = _context.User.Where(o => o.UserId == UserId).FirstOrDefault();
>>>>>>> DevOps-Dev
                user.State = State.Off;
                _context.User.Update(user);
                homeScore.Acceptance = homeScoreSum.Acceptance + sum;
                homeScore.MaxJob = homeScoreSum.MaxJob + sum;
                _context.HomeScore.Update(homeScore);
                job.StatusName = JobStatus.Desc.ReceiveJob;

                _context.SaveChanges();
                response.Success = true;
                response.Message = "สำเร็จ";
                return Json(response);
            }
            else if(status.JobStatus == 2)
            {
                if(homeScoreSum.Timeout == null)
                {
                    homeScore.Timeout = 0;
                    _context.SaveChanges();
                }
<<<<<<< HEAD
                var userstate = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
=======
                var userstate = _context.User.Where(o => o.UserId == UserId).FirstOrDefault();
>>>>>>> DevOps-Dev
                userstate.State = State.On;
                homeScore.Timeout = homeScoreSum.Timeout + sum;
                _context.SaveChanges();
                jobRequest.Success = false;
                jobRequest.Message = "หมดเวลารับงาน";
                jobRequest.Job = null;
                return Json(jobRequest);
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult StatusService()
        {
<<<<<<< HEAD
            int userId = UserId();
            BaseResponse response = new BaseResponse();
            Job JobStatusName = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
=======
            int UserId = IdUser();
            BaseResponse response = new BaseResponse();
            Job JobStatusName = _context.Job.Where(o => o.EmployeeId == UserId).OrderByDescending(o => o.JobId).FirstOrDefault();
>>>>>>> DevOps-Dev
            JobStatusName.StatusName = JobStatus.Desc.Arrive;
            _context.SaveChanges();
            response.Success = true;
            response.Message = "สำเร็จ";
            return Json(response);
        }

        [HttpPost]
        public IActionResult PaymentJob()
        {
<<<<<<< HEAD
            int userId = UserId();
            BaseResponse response = new BaseResponse();
            Job JobStatusName = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
            CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
=======
            int UserId = IdUser();
            BaseResponse response = new BaseResponse();
            Job JobStatusName = _context.Job.Where(o => o.EmployeeId == UserId).OrderByDescending(o => o.JobId).FirstOrDefault();
            CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == UserId).FirstOrDefault();
>>>>>>> DevOps-Dev
            user.State = State.On;
            JobStatusName.StatusName = JobStatus.Desc.Payment;
            response.Success = true;
            response.Message = "สำเร็จ";
            JobStatusName.StatusName = JobStatus.Desc.Arrive;
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
<<<<<<< HEAD
                int userId = UserId();
=======
                int userId = IdUser();
>>>>>>> DevOps-Dev
                CarWash.Models.DBModels.User user = _context.User.Where(o => o.UserId == userId).FirstOrDefault();
                Job job = _context.Job.Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                job.Report = req.Report;
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
<<<<<<< HEAD
                int userId = UserId();
=======
                int userId = IdUser();
>>>>>>> DevOps-Dev
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
            try
            {
<<<<<<< HEAD
                int userId = UserId();
=======
                int userId = IdUser();
>>>>>>> DevOps-Dev
                Job job = new Job();
                job.CustomerId = userId;
                job.JobDateTime = DateTime.Now;
                job.PackageId = req.PackageId;
                job.CarId = req.CarId;
                job.Latitude = req.Latitude;
                job.Longitude = req.Longitude;
                job.StatusId = 1;
                Package package = _context.Package.Where(o => o.PackageId == req.PackageId).FirstOrDefault();
                job.TotalPrice = (int)package.Price;
                job.StatusName = JobStatus.Desc.BookingJob;
                string location = await ServiceCheck.LocationAsync(req.Longitude, req.Latitude);
                job.Location = location;
                _context.Job.Add(job);
                _context.SaveChanges();
                JobRequset jobname = new JobRequset();
                JobRequestResponse json = new JobRequestResponse();
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
                        userEmplocation.Location = Location;
                    }
                    var users = _context.HomeScore.Where(o => o.EmployeeId == userEmplocation.UserId).OrderByDescending(o => o.Rating);
                    userEmplocation.Rating = users.Select(o => o.Rating).FirstOrDefault();
                    jobDb.Add(userEmplocation);
                }
                var filteredList = jobDb.OrderByDescending(o => o.Rating).ToList();
                var listCount = (filteredList.Count() < 5) ? filteredList.Count() : 5;
                for(int Index = 0; Index < listCount; Index++)
                {
<<<<<<< HEAD
                    var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Include(o => o.Package).Where(o => o.CustomerId == userId).OrderByDescending(o => o.JobId);
                    var EmpId = jobdb.Select(o => o.EmployeeId).FirstOrDefault();
=======
                    Job jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Package.ModelPackage).Include(o => o.Customer)
                        .Include(o => o.Package).Include(o => o.Car).Where(o => o.CustomerId == userId)
                        .OrderByDescending(o => o.JobId).FirstOrDefault();
                    var EmpId = jobdb.EmployeeId;
>>>>>>> DevOps-Dev
                    if(EmpId == null)
                    {
                        var statusid = jobdb.StatusId;
                        if(statusid == 1)
                        {

                            var statename = _context.User.Where(o => o.UserId == filteredList[Index].UserId);
                            var state = statename.Select(o => o.State).FirstOrDefault();
                            if(state == 1)
                            {

                                jobname.JobId = jobdb.JobId;
                                jobname.EmployeeId = filteredList[Index].UserId;
                                string receiveEmployee = "ReceiveEmployee" + filteredList[Index].UserId.ToString();
                                jobname.FullName = jobdb.Customer.FullName;
                                jobname.Phone = jobdb.Customer.Phone;
                                jobname.ImageProfile = jobdb.Customer.Image;
                                var latlon = _context.User.Where(o => o.UserId == filteredList[Index].UserId).FirstOrDefault();
                                double lon = latlon.Longitude ??= 0;
                                double lat = latlon.Latitude ??= 0;
                                string showDistance = await ServiceCheck.DistanceAsync(lon, lat, req.Longitude, req.Latitude);
                                jobname.Location = location;
                                jobname.Distance = showDistance;
                                jobname.PackageName = jobdb.Package.ModelPackage.PackageName;
                                jobname.VehicleRegistration = jobdb.Car.VehicleRegistration;
                                jobname.TotalPrice = jobdb.Package.Price.ToString() + ".00 ฿";
                                jobname.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                                json.Success = true;
                                json.Message = "สำเร็จ";
                                json.Job = jobname;
                                string result = JsonConvert.SerializeObject(json);
                                await EmployeeHup.Clients.All.SendAsync(receiveEmployee, result);
                                var userstate = _context.User.Where(o => o.UserId == filteredList[Index].UserId).FirstOrDefault();
                                userstate.State = State.Off;
                                _context.User.Update(userstate);
                                Thread.Sleep(20000);
                            }
                        }
                    }
                    var nameIdEmp = jobdb.EmployeeId;
                    if(nameIdEmp != null)
                    {
                        BaseResponse response = new BaseResponse();
                        response.Success = true;
                        response.Message = "สำเร็จ";
                        return Json(response);
                    }
                }
            }
            catch(Exception)
            {

            }
            BaseResponse baseResponse = new BaseResponse();
            baseResponse.Success = false;
            baseResponse.Message = "ไม่สำเร็จ";
            return Json(baseResponse);
        }

        [HttpGet]
        public async Task<IActionResult> FetchJobinfoAsync(int jobid)
        {
<<<<<<< HEAD
            int userId = UserId();
=======
            int userId = IdUser();
>>>>>>> DevOps-Dev
            JobRequestResponse reqresponse = new JobRequestResponse();
            Job job = _context.Job.Where(o => o.JobId == jobid && o.EmployeeId == userId).FirstOrDefault();
            if(job == null)
            {
                BaseResponse response = new BaseResponse();
                response.Success = false;
                response.Message = "ไม่พบข้อมูลในระบบ";
                return Json(response);
            }
            Job jobdbupdate = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Include(o => o.Car).Include(o => o.Package).Include(o => o.Package.ModelPackage).Where(o => o.EmployeeId == userId && o.JobId == jobid).FirstOrDefault();
            JobRequset jobrequset = new JobRequset();
            jobrequset.EmployeeId = userId;
            jobrequset.JobId = jobdbupdate.JobId;
            jobrequset.FullName = jobdbupdate.Customer.FullName;
            jobrequset.Phone = jobdbupdate.Customer.Phone;
            jobrequset.ImageProfile = jobdbupdate.Customer.Image;
            jobrequset.Latitude = jobdbupdate.Latitude;
            jobrequset.Longitude = jobdbupdate.Longitude;
            string location = await ServiceCheck.LocationAsync(jobdbupdate.Longitude, jobdbupdate.Latitude);
            jobrequset.Location = location;
            jobrequset.PackageName = jobdbupdate.Package.ModelPackage.PackageName;
            jobrequset.VehicleRegistration = jobdbupdate.Car.VehicleRegistration;
            jobrequset.TotalPrice = jobdbupdate.TotalPrice + ".00 ฿";
            jobrequset.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            reqresponse.Success = true;
            reqresponse.Message = "สำเร็จ";
            reqresponse.Job = jobrequset;
            return Json(reqresponse);
        }

        private int ImageidDB(int jobid)
        {
            ImageService image = new ImageService();
            image.JobId = jobid;
            _context.ImageService.Add(image);
            _context.SaveChanges();
            return image.ImageId;
        }
<<<<<<< HEAD
        private int UserId()
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = int.Parse(Id);
            return userId;
=======
        private int IdUser()
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int UserId = int.Parse(Id);
            return UserId;
>>>>>>> DevOps-Dev
        }
    }
}

