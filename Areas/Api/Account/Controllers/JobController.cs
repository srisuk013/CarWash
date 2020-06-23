using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsConst;
using CarWash.Areas.Api.Models.ModelsReponse;
using CarWash.Areas.Api.Models.ModelsReq;
using CarWash.Models.DBModels;
using CarWash.Service;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Account.Controllers
{

    [Area("Api")]
    [Route("Api/Job/[Action]")]
    [ServiceFilter(typeof(CarWashAuthorization))]
    public class JobController : CarWashController
    {
        private CarWashContext _context;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        [Obsolete]
        private readonly IHostingEnvironment _env;
        private static string ApiKey = "AIzaSyA0xBPLP9vDxXdbsQ1PYkBROfs4-vYvB1M";
        private static string Bucket = "carwash-1e810.appspot.com";
        private static string AuthEmail = "Srisuk013@gmail.com";
        private static string AuthPassword = "ssss1111";

        [Obsolete]
        public JobController(CarWashContext context, UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }
        [HttpPost]
        public IActionResult DeleteServiceImage([FromBody] ReqDeleteImage req)
        {
            BaseResponse response = new BaseResponse();
            response.Success = false;
            if(req.ImageId == 0)
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
                    imageSevice.RightBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.BackAfter)
                {
                    imageSevice.RightBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.LaftAfter)
                {
                    imageSevice.RightBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                else if(req.ImageId == UpImage.RightAfter)
                {
                    imageSevice.RightBefore = null;
                    _context.ImageSevice.Update(imageSevice);
                    _context.SaveChanges();
                }
                ImageServiceReponse responses = new ImageServiceReponse();
                ServiceImage service = new ServiceImage();
                var jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == userId).OrderByDescending(o => o.JobId);
                var serviceDb = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == imageSevice.JobId);
                service.FrontBefore = serviceDb.Select(o => o.FrontBefore).FirstOrDefault();
                service.BackBefore = serviceDb.Select(o => o.BackBefore).FirstOrDefault();
                service.LaftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                service.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                service.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                service.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                service.LaftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                service.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == jobDb.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    service.OtherImageSevice.Add(otherImage);
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
            if(req.ImageId < 0)
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
                service.LaftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                service.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                service.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                service.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                service.LaftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                service.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == userEmp.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    service.OtherImageSevice.Add(otherImage);
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
                imageservice.LaftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                imageservice.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                imageservice.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                imageservice.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                imageservice.LaftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                imageservice.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == job.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    imageservice.OtherImageSevice.Add(otherImage);
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
        [Obsolete]
        public async Task<IActionResult> Uploadimageservice([FromForm] StatusServiceImage image)
        {
            FileStream fs = null;
            try
            {
                if(image.File.Length > 0)
                {
                    string folderName = "FirebaseFilesV1";
                    string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");

                    if(Directory.Exists(path))
                    {
                        using(fs = new FileStream(Path.Combine(path, image.File.FileName), FileMode.Create))
                        {
                            await image.File.CopyToAsync(fs);
                        }
                        fs = new FileStream(Path.Combine(path, image.File.FileName), FileMode.Open);
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                    var cancellation = new CancellationTokenSource();
                    string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                    String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    int idName = int.Parse(Id);
                    var nameImage = ServiceCheck.CheckImage(image.StatusService);
                    Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
                    OthrerImage other = new OthrerImage();
                    ImageSevice service = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == job.JobId).FirstOrDefault();
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
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true
                        })
                        .Child(nameImage)
                        .Child($"{(name)}.jpg")
                        .PutAsync(fs, cancellation.Token);
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
                            service.FrontBefore = ImageUrl;
                            _context.ImageSevice.Update(service);
                            _context.SaveChanges();
                        }
                        else if(image.StatusService == 4)
                        {
                            service.FrontBefore = ImageUrl;
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
                    serviceImage.LaftBefore = serviceDb.Select(o => o.LaftBefore).FirstOrDefault();
                    serviceImage.RightBefore = serviceDb.Select(o => o.RightBefore).FirstOrDefault();
                    serviceImage.FrontAfter = serviceDb.Select(o => o.FrontAfter).FirstOrDefault();
                    serviceImage.BackAfter = serviceDb.Select(o => o.BackAfter).FirstOrDefault();
                    serviceImage.LaftAfter = serviceDb.Select(o => o.LaftAfter).FirstOrDefault();
                    serviceImage.RightAfter = serviceDb.Select(o => o.RightAfter).FirstOrDefault();
                    List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == job.JobId).ToList();
                    foreach(OthrerImage images in Jobimage)
                    {
                        OtherImage otherImage = new OtherImage();
                        otherImage.ImageId = images.ImageId;
                        otherImage.Image = images.Image;
                        serviceImage.OtherImageSevice.Add(otherImage);
                    }
                    UpImageResponse response = new UpImageResponse();
                    response.Success = true;
                    response.Message = "อัพรูปสำเร็จ";
                    response.ServiceImage = serviceImage;
                    return Json(response);
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
                Double? flag = 0;
                if(req.Latitude == flag)
                {
                    response.Message = "ไม่ได้ส่งตำแหน่ง";
                    return Json(response);
                }
                else if(req.Longitude == flag)
                {
                    response.Message = "ไม่ได้ส่งตำแหน่ง";
                    return Json(response);
                }
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                CarWash.Models.DBModels.User userEmp = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                var JobN = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId);
                userEmp.Latitude = req.Latitude;
                userEmp.Longitude = req.Longitude;
                _context.User.Update(userEmp);
                _context.SaveChanges();
                Navigation navigation = new Navigation();
                navigation.CustomerLatitude = JobN.Select(o => o.Customer.Latitude).FirstOrDefault();
                navigation.CustomerLongitude = JobN.Select(o => o.Customer.Longitude).FirstOrDefault();
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
                List<JobHistory> jobs1 = new List<JobHistory>();
                foreach(Job HistoryJob in JobDbmonth)
                {
                    JobHistory job = new JobHistory(HistoryJob);
                    List<ImageSevice> imageSevices = _context.ImageSevice.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(ImageSevice sevice in imageSevices)
                    {
                        ImageSevices image1 = new ImageSevices();
                        image1.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                        job.ImageBeforeService.Add(image1);
                        ImageSevices image2 = new ImageSevices();
                        image2.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                        job.ImageBeforeService.Add(image2);
                        ImageSevices image3 = new ImageSevices();
                        image3.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                        job.ImageBeforeService.Add(image3);
                        ImageSevices image4 = new ImageSevices();
                        image4.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                        job.ImageBeforeService.Add(image4);
                    }
                    foreach(ImageSevice sevice in imageSevices)
                    {
                        AfterImage image1 = new AfterImage();
                        image1.Image = imageSevices.Select(o => o.FrontAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(image1);
                        AfterImage image2 = new AfterImage();
                        image2.Image = imageSevices.Select(o => o.BackAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(image2);
                        AfterImage image3 = new AfterImage();
                        image3.Image = imageSevices.Select(o => o.LaftAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(image3);
                        AfterImage image4 = new AfterImage();
                        image4.Image = imageSevices.Select(o => o.RightAfter).FirstOrDefault();
                        job.ImagesAfterService.Add(image4);
                    }
                    List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(OthrerImage image in Jobimage)
                    {
                        OtherImage otherImage = new OtherImage();
                        otherImage.ImageId = image.ImageId;
                        otherImage.Image = image.Image;
                        job.OtherImageService.Add(otherImage);
                    }
                    jobs1.Add(job);
                }
                historyResponse.Success = true;
                historyResponse.Message = "สำเร็จ";
                historyResponse.Histories = jobs1;
                return Json(historyResponse);
            }
            List<JobHistory> jobs = new List<JobHistory>();
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
                    ImageSevices image1 = new ImageSevices();
                    image1.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                    job.ImageBeforeService.Add(image1);
                    ImageSevices image2 = new ImageSevices();
                    image2.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                    job.ImageBeforeService.Add(image2);
                    ImageSevices image3 = new ImageSevices();
                    image3.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                    job.ImageBeforeService.Add(image3);
                    ImageSevices image4 = new ImageSevices();
                    image4.Image = imageSevices.Select(o => o.FrontBefore).FirstOrDefault();
                    job.ImageBeforeService.Add(image4);
                }
                foreach(ImageSevice sevice in imageSevices)
                {
                    AfterImage image1 = new AfterImage();
                    image1.Image = imageSevices.Select(o => o.FrontAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(image1);
                    AfterImage image2 = new AfterImage();
                    image2.Image = imageSevices.Select(o => o.BackAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(image2);
                    AfterImage image3 = new AfterImage();
                    image3.Image = imageSevices.Select(o => o.LaftAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(image3);
                    AfterImage image4 = new AfterImage();
                    image4.Image = imageSevices.Select(o => o.RightAfter).FirstOrDefault();
                    job.ImagesAfterService.Add(image4);
                }
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.ImageId = image.ImageId;
                    otherImage.Image = image.Image;
                    job.OtherImageService.Add(otherImage);
                }
                jobs.Add(job);
            }
            historyResponse.Success = true;
            historyResponse.Message = "สำเร็จ";
            historyResponse.Histories = jobs;
            return Json(historyResponse);
        }

        [HttpPost]
        public IActionResult JobQuestion()
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            JobRequset jobreq = new JobRequset();
            jobreq.JobId = job.JobId;
            jobreq.FullName = "Srisuk NAN";
            jobreq.ImageProfile = "https://s.isanook.com/no/0/ud/149/747091/747091-thumbnail-20191121161226.jpg";
            jobreq.Latitude = job.Latitude;
            jobreq.Longitude = job.Longitude;
            jobreq.PackageName = "ล้างสี+ดูดฝุ่น";
            jobreq.VehicleRegistration = "อด285";
            jobreq.Distance = "8.5 km";
            jobreq.Price = "฿ 140.00";
            jobreq.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            JobRequestResponse jobRequest = new JobRequestResponse();
            jobRequest.Success = true;
            jobRequest.Message = "สำเร็จ";
            jobRequest.Job = jobreq;
            return Json(jobRequest);
        }

        [HttpPost]

        public IActionResult JobAnswer([FromBody] ReqJobStatus status)
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            JobRequset jobname = new JobRequset();
            string date = DateTime.Now.ToString("ddMMyyyyHHmm");
            int month = Convert.ToInt32(date.Substring(2, 2));
            JobRequestResponse jobRequest = new JobRequestResponse();
            var homeScoreSum = _context.HomeScore.Include(o => o.Employee).Where(o => o.EmployeeId == idName);
            HomeScore homeScore = _context.HomeScore.Where(o => o.EmployeeId == idName).FirstOrDefault();
            var JobN = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId);
            Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            int sum = 1;
            var dateMonth = homeScoreSum.Select(o => o.CreatedTime.Month).FirstOrDefault();
            if(dateMonth != month)
            {
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
                jobname.JobId = JobN.Select(o => o.JobId).FirstOrDefault();
                jobname.FullName = JobN.Select(o => o.Customer.FullName).FirstOrDefault();
                jobname.Phone = JobN.Select(o => o.Customer.Phone).FirstOrDefault();
                jobname.ImageProfile = JobN.Select(o => o.Customer.Image).FirstOrDefault();
                jobname.Latitude = JobN.Select(o => o.Latitude).FirstOrDefault();
                jobname.Longitude = JobN.Select(o => o.Longitude).FirstOrDefault();
                jobname.PackageName = JobN.Select(o => o.Package.PackageImage).FirstOrDefault();
                jobname.VehicleRegistration = JobN.Select(o => o.Car.VehicleRegistration).FirstOrDefault();
                jobname.Price = JobN.Select(o => o.Price.ToString()).FirstOrDefault();
                jobname.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                jobRequest.Success = true;
                jobRequest.Message = "สำเร็จ";
                jobRequest.Job = jobname;
                _context.Job.Update(job);
                _context.SaveChanges();
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

    }
}

