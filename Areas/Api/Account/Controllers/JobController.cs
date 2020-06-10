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
        private int idName;
        private readonly IHostingEnvironment _env;
        private static string ApiKey = "AIzaSyA0xBPLP9vDxXdbsQ1PYkBROfs4-vYvB1M";
        private static string Bucket = "carwash-1e810.appspot.com";
        private static string AuthEmail = "Srisuk013@gmail.com";
        private static string AuthPassword = "ssss1111";

        public JobController(CarWashContext context, UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }
        public string RunnigJob()
        {
            string code = "JOB";
            int countRunning = _context.Job.Where(o => o.JobDateTime.Year == DateTime.Now.Year
            && o.JobDateTime.Month == DateTime.Now.Month).Count() + 1;
            string codeSum = DateTime.Now.ToString("yyMM") + countRunning.ToString().PadLeft(4, '0');
            return code + codeSum;
        }

        [HttpPost]
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
                    var upload = new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true
                        })
                        .Child(nameImage)
                        .Child($"{(job.JobId)}.jpg")
                        .PutAsync(fs, cancellation.Token);

                    var ImageUrl = await upload;
                    if(image.StatusService == UpImage.Front)
                    {
                        job.ImageFront = ImageUrl;
                    }
                    else if(image.StatusService == UpImage.Back)
                    {
                        job.ImageBack = ImageUrl;
                    }
                    else if(image.StatusService == UpImage.Laft)
                    {
                        job.ImageLeft = ImageUrl;
                    }
                    else if(image.StatusService == UpImage.Right)
                    {
                        job.ImageRight = ImageUrl;
                    }

                    _context.Job.Update(job);
                    _context.SaveChanges();
                    var JobDbmonth = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage)
                    .Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).ToList();
                    ServiceImage service = new ServiceImage();
                    service.ImageFront = job.ImageFront;
                    service.ImageBack = job.ImageBack;
                    service.ImageLeft = job.ImageLeft;
                    service.ImageRight = job.ImageRight;
                    UpImageResponse response = new UpImageResponse();
                    response.Success = true;
                    response.Message = "อัพรูปสำเร็จ";
                    response.ServiceImage = service;
                    return Json(response);

                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();
        }

        //[HttpGet]
        //public IActionResult Job()
        //{
        //    string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
        //    String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    int idName = int.Parse(Id);
        //    Job job = _context.Job.Where(o => o.JobId == idName).FirstOrDefault();
        //    job.CodeJob = RunnigJob();
        //    _context.Job.Add(job);
        //    _context.SaveChanges();
        //    BaseResponse response = new BaseResponse();
        //    response.Success = true;
        //    response.Message = "เปลียนstateสำเร็จ";
        //    return Json(response);
        //}
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
            catch(Exception e)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult History(long? DateBegin , long? DateEnd )
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
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
                    List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(OthrerImage image in Jobimage)
                    {
                        OtherImage otherImage = new OtherImage();
                        otherImage.Image = image.Image;
                        job.OtherImages.Add(otherImage);
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
           .Where(o => o.EmployeeId == idName).Where(o => o.JobDateTime >= datebegin && o.JobDateTime <= dateEnd).ToList();
            foreach(Job HistoryJob in JobDb)
            {
                JobHistory job = new JobHistory(HistoryJob);
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                foreach(OthrerImage image in Jobimage)
                {
                    OtherImage otherImage = new OtherImage();
                    otherImage.Image = image.Image;
                    job.OtherImages.Add(otherImage);
                }
                jobs.Add(job);
            }
            historyResponse.Success = true;
            historyResponse.Message = "สำเร็จ";
            historyResponse.Histories = jobs;
            return Json(historyResponse);
        }

        [HttpPost]
        public IActionResult JobRequest()
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            JobRequset jobreq = new JobRequset();
            jobreq.JobId = job.JobId;
            jobreq.FullName = "Srisuk NAN";
            jobreq.ImageProfile = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQmwhJ_cqL4z3zxh8mxxn_5Iy7wYIwaAoJ0Yg6VI77u36WLi2QA&usqp=CAU";
            jobreq.Latitude = job.Latitude;
            jobreq.Longitude = job.Longitude;
            jobreq.PackageName = "ล้างสี+ดูดฝุ่น";
            jobreq.VehicleRegistration = "อด285";
            jobreq.Price = "฿ 140.00";
            jobreq.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            JobRequestResponse jobRequest = new JobRequestResponse();
            jobRequest.Success = true;
            jobRequest.Message = "สำเร็จ";
            jobRequest.Job = jobreq;
            return Json(jobRequest);
        }

        [HttpPost]
        public IActionResult JobResponse([FromBody] ReqJobStatus status)
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            JobRequset jobname = new JobRequset();
            JobRequestResponse jobRequest = new JobRequestResponse();
            var JobN = _context.Job.Include(o => o.Employee).Include(o => o.Customer).Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId);
            Job job = _context.Job.Where(o => o.EmployeeId == idName).OrderByDescending(o => o.JobId).FirstOrDefault();
            if(status.JobStatus == 0)
            {
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
                job.StatusName = JobStatus.Desc.ReceiveJob;
                jobname.JobId = JobN.Select(o => o.JobId).FirstOrDefault();
                jobname.FullName = JobN.Select(o => o.Customer.FullName).FirstOrDefault();
                jobname.ImageProfile = JobN.Select(o => o.Customer.Image).FirstOrDefault();
                jobname.Latitude = job.Latitude;
                jobname.Longitude = job.Longitude;
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
        public IActionResult SetStastus()
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
        public IActionResult SetStastuspayment()
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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

    }

}