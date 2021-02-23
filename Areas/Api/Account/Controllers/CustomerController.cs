using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsReponse;
using CarWash.Areas.Api.Models.ModelsReq;
using CarWash.Areas.Api.Models.ModelsResponse;
using CarWash.Models.DBModels;
using CarWash.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]
    [Route("Api/Customer/[Action]")]


    public class CustomerController : Controller
    {

        private readonly CarWashContext _context;

        public CustomerController(CarWashContext context)
        {
            _context = context;
        }
        [HttpGet]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult ListCarInformation()
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            ShowModelCar brandcar = new ShowModelCar();
            var Car = _context.CarBrand.ToList();
            foreach(CarBrand carIn in Car)
            {
                Brand models = new Brand();
                models.BrandId = carIn.BrandId;
                models.BrandName = carIn.BrandName;
                brandcar.models.Add(models);
            }
            ShowInformationResponse reponse = new ShowInformationResponse();
            reponse.Success = true;
            reponse.Message = "สำเร็จ";
            reponse.Brand = brandcar;
            return Json(reponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult CarInformation([FromBody] ReqCarInformation req)
        {
            string claimuserid = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userid = int.Parse(Id);
            BaseResponse response = new BaseResponse();
            if(req.ImageId == 0)
            {
                Car car = new Car();
                car.UserId = userid;
                car.ProvinceId = req.province;
                car.BrandId = req.BaBrandId;
                car.Model_Id = req.ModelId;
                car.VehicleRegistration = req.VehicleRegistration;
                _context.Car.Add(car);
                _context.SaveChanges();
                response.Message = "เพิ่มข้อมูลรถสำเร็จ";
                response.Success = true;
            }
            else
            {
                Car carupdate = _context.Car.Where(o => o.CarId == req.ImageId).FirstOrDefault();
                carupdate.ProvinceId = req.province;
                carupdate.BrandId = req.BaBrandId;
                carupdate.Model_Id = req.ModelId;
                carupdate.VehicleRegistration = req.VehicleRegistration;
                _context.Car.Update(carupdate);
                _context.SaveChanges();
                response.Message = "อัพข้อมูลรถสำเร็จ";
                response.Success = true;
            }
            return Json(response);
        }

        [HttpPost]
        public IActionResult Ratings([FromBody] ReqRatings req)
        {
            BaseResponse response = new BaseResponse();
            if(String.IsNullOrEmpty(req.Comment))
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
                Job job = _context.Job.Where(o => o.CustomerId == userId).OrderByDescending(o => o.JobId).FirstOrDefault();
                HomeScore score = _context.HomeScore.Where(o => o.EmployeeId == job.EmployeeId).FirstOrDefault();
                int? scoresum = score.Score;
                score.Score = req.Score + scoresum;
                job.Comment = req.Comment;
                _context.HomeScore.Update(score);
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

        [HttpGet]

        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult ShowPackage()
        {
            try
            {
                string claimuserid = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int userid = int.Parse(Id);
                ShowPackageResponse showPackage = new ShowPackageResponse();
                List<ListPackage> listPackageDb = new List<ListPackage>();
                BaseResponse response = new BaseResponse();
                var ChackRole = _context.User.Where(o => o.UserId == userid && o.Role == Role.Customer).FirstOrDefault();
                if(ChackRole == null)
                {
                    response.Message = "เฉพาะลูกค้า";
                    response.Success = false;
                    return Json(response);
                }
                Car checkcar = _context.Car.Where(o => o.UserId == userid).FirstOrDefault();
                if(checkcar == null)
                {
                    var packageall = _context.Package.Include(o => o.ModelPackage).ToList();
                    foreach(Package package1 in packageall)
                    {
                        ListPackage listPackage = new ListPackage();
                        listPackage.PackageId = package1.PackageId;
                        listPackage.Packagename = package1.ModelPackage.PackageName;
                        listPackage.SizeId = package1.SizeId;
                        listPackage.Description = package1.Description;
                        listPackage.Price = package1.Price.ToString();
                        listPackageDb.Add(listPackage);
                    }
                    showPackage.Message = "สำเร็จ";
                    showPackage.Success = true;
                    showPackage.packageCar = listPackageDb;
                    return Json(showPackage);
                }
                var modle = checkcar.Model_Id;
                var modlesize = _context.CarModel.Where(o => o.Model_Id == modle).FirstOrDefault();
                var package = _context.Package.Include(o => o.ModelPackage).Include(o => o.Size).Where(o => o.SizeId == modlesize.SizeId).ToList();
                foreach(Package show in package)
                {
                    ShowPackageCar packageCar = new ShowPackageCar();
                    ListPackage listPackage = new ListPackage();
                    listPackage.PackageId = show.PackageId;
                    listPackage.Packagename = show.ModelPackage.PackageName;
                    listPackage.SizeId = show.SizeId;
                    listPackage.Description = show.Description;
                    listPackage.Price = show.Price.ToString();
                    listPackageDb.Add(listPackage);
                }
                showPackage.Message = "สำเร็จ";
                showPackage.Success = true;
                showPackage.packageCar = listPackageDb;
                return Json(showPackage);
            }
            catch(Exception)
            {

            }
            return Ok();

        }
        [HttpGet]
        public IActionResult ShowPackageAll(int? model)
        {
            try
            {
                var check = _context.CarModel.Where(o => o.Model_Id == model).FirstOrDefault();
                if(check == null)
                {
                    ShowPackageAllResponse showPackage = new ShowPackageAllResponse();
                    List<ListPackageAll> listPackageDb = new List<ListPackageAll>();
                    List<ListPackageAll> listPackageDbV1 = new List<ListPackageAll>();
                    var packageall = _context.Package.Include(o => o.ModelPackage).Include(o => o.Size).Where(o => o.ModelPackageId == 1).ToList();
                    foreach(Package package in packageall)
                    {
                        ListPackageAll listPackage = new ListPackageAll();
                        listPackage.Price_s = package.Price.ToString();
                        listPackageDbV1.Add(listPackage);
                    }
                    var packageall1 = _context.Package.Include(o => o.ModelPackage).Include(o => o.Size).Where(o => o.ModelPackageId == 2).ToList();
                    foreach(Package package in packageall)
                    {
                        ListPackageAll listPackage = new ListPackageAll();
                        listPackage.Price_s = package.Price.ToString();
                        listPackageDb.Add(listPackage);
                    }
                    showPackage.Message = "สำเร็จ";
                    showPackage.Success = true;
                    showPackage.PackageCarV1 = listPackageDbV1;
                    showPackage.packageCar = listPackageDb;
                    return Json(showPackage);
                }
                else if(check != null)
                {
                    var sizecar = check.SizeId;
                    var package = _context.Package.Where(o => o.SizeId == sizecar).ToList();
                }

            }
            catch(Exception)
            {

            }
            return Ok();
        }

        [HttpGet]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult ChooseCar()
        {
            string claimuserid = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userid = int.Parse(Id);
            var mycar = _context.Car.Include(o => o.Brand).Include(o => o.Model_).Include(o=>o.Province).Where(o => o.UserId == userid).ToList();
            ChooseCarResponse response = new ChooseCarResponse();
            List<ChooseMyCar> chooseMyCars = new List<ChooseMyCar>();
            foreach(Car car in mycar)
            {
                ChooseMyCar cars = new ChooseMyCar();
                cars.CarId = car.CarId;
                cars.Brand = car.Brand.BrandName;
                cars.Modelcar = car.Model_.ModelName;
                cars.VehicleRegistration = car.VehicleRegistration;
                cars.Province = car.Province.ProvinceName;
                chooseMyCars.Add(cars);
            }
            response.Success = true;
            response.Message = "สำเร็จ";
            response.MyCar = chooseMyCars;

            return Json(response);
        }
        [HttpGet]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult History(long? DateBegin, long? DateEnd)
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userid = int.Parse(Id);
            HistoryResponseV2 historyResponse = new HistoryResponseV2();
            User user = _context.User.Where(o => o.UserId == userid).FirstOrDefault();
            if(DateBegin == 0 && DateEnd == 0)
            {
                string date = DateTime.Now.ToString("ddMMyyyyHHmm");
                int month = Convert.ToInt32(date.Substring(2, 2));
                var JobDbmonth = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Package.ModelPackage).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage)
               .Where(o => o.CustomerId == userid && o.JobDateTime.Month == month && o.Report == null && o.EmployeeId != null).ToList();
                List<HistoryCustomer> jobDb = new List<HistoryCustomer>();
                foreach(Job HistoryJob in JobDbmonth)
                {
                    HistoryCustomer job = new HistoryCustomer(HistoryJob);
                    List<ImageService> imageSevices = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                    foreach(ImageService sevice in imageSevices)
                    {
                        ImageServicesModel imageFrontBefore = new ImageServicesModel();
                        imageFrontBefore.Image = sevice.FrontBefore;
                        job.ImagesBeforeService.Add(imageFrontBefore);
                        ImageServicesModel imageBackBefore = new ImageServicesModel();
                        imageBackBefore.Image = sevice.BackBefore;
                        job.ImagesBeforeService.Add(imageBackBefore);
                        ImageServicesModel imageLaftBefore = new ImageServicesModel();
                        imageLaftBefore.Image = sevice.LeftBefore;
                        job.ImagesBeforeService.Add(imageLaftBefore);
                        ImageServicesModel imageRightBefore = new ImageServicesModel();
                        imageRightBefore.Image = sevice.RightBefore;
                        job.ImagesBeforeService.Add(imageRightBefore);
                    }
                    foreach(ImageService sevice in imageSevices)
                    {
                        AfterImage imageFrontAfter = new AfterImage();
                        imageFrontAfter.Image = sevice.FrontAfter;
                        job.ImagesAfterService.Add(imageFrontAfter);
                        AfterImage imageBackAfter = new AfterImage();
                        imageBackAfter.Image = sevice.BackAfter;
                        job.ImagesAfterService.Add(imageBackAfter);
                        AfterImage imageLaftAfter = new AfterImage();
                        imageLaftAfter.Image = sevice.LeftAfter;
                        job.ImagesAfterService.Add(imageLaftAfter);
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
            List<HistoryCustomer> jobdb = new List<HistoryCustomer>();
            DateTime datebegin = ServiceCheck.DateTime(DateBegin.Value);
            DateTime dateEnd = ServiceCheck.DateTime(DateEnd.Value);
            var JobDb = _context.Job.Include(o => o.Car).Include(o => o.Package).Include(o => o.Employee).Include(o => o.Customer).Include(o => o.OthrerImage).Include(o => o.Package.ModelPackage)
           .Where(o => o.EmployeeId == userid).Where(o => o.JobDateTime.Date >= datebegin && o.JobDateTime.Date <= dateEnd).Where(o => o.Report == null).ToList();
            foreach(Job HistoryJob in JobDb)
            {
                if(HistoryJob == null)
                {
                    BaseResponse response = new BaseResponse();
                    response.Message = "ไม่พบข้อมูล";
                    response.Success = false;
                    return Json(response);
                }
                HistoryCustomer job = new HistoryCustomer(HistoryJob);
                List<OthrerImage> Jobimage = _context.OthrerImage.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                List<ImageService> imageSevices = _context.ImageService.Include(o => o.Job).Where(o => o.JobId == HistoryJob.JobId).ToList();
                foreach(ImageService sevice in imageSevices)
                {
                    ImageServicesModel imageFrontBefore = new ImageServicesModel();
                    imageFrontBefore.Image = sevice.FrontBefore;
                    job.ImagesBeforeService.Add(imageFrontBefore);
                    ImageServicesModel imageBackBefore = new ImageServicesModel();
                    imageBackBefore.Image = sevice.BackBefore;
                    job.ImagesBeforeService.Add(imageBackBefore);
                    ImageServicesModel imageLaftBefore = new ImageServicesModel();
                    imageLaftBefore.Image = sevice.LeftBefore;
                    job.ImagesBeforeService.Add(imageLaftBefore);
                    ImageServicesModel imageRightBefore = new ImageServicesModel();
                    imageRightBefore.Image = sevice.RightBefore;
                    job.ImagesBeforeService.Add(imageRightBefore);
                }
                foreach(ImageService sevice in imageSevices)
                {
                    AfterImage imageFrontAfter = new AfterImage();
                    imageFrontAfter.Image = sevice.FrontAfter;
                    job.ImagesAfterService.Add(imageFrontAfter);
                    AfterImage imageBackAfter = new AfterImage();
                    imageBackAfter.Image = sevice.BackAfter;
                    job.ImagesAfterService.Add(imageBackAfter);
                    AfterImage imageLaftAfter = new AfterImage();
                    imageLaftAfter.Image = sevice.LeftAfter;
                    job.ImagesAfterService.Add(imageLaftAfter);
                    AfterImage imageRightAfter = new AfterImage();
                    imageRightAfter.Image = sevice.RightAfter;
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

        [HttpGet]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Province()
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userid = int.Parse(Id);
            ShowProvince show = new ShowProvince();
            var Car = _context.Province.ToList();
            foreach(Province carIn in Car)
            {
                ProvinceModel models = new ProvinceModel();
                models.ProvinceId = carIn.ProvinceId;
                models.ProvinceName = carIn.ProvinceName;
                show.models.Add(models);
            }
            ProvinceResponse reponse = new ProvinceResponse();
            reponse.Success = true;
            reponse.Message = "สำเร็จ";
            reponse.Shows = show;
            return Json(reponse);
        }
        [HttpGet]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Model(int? brandid)
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userid = int.Parse(Id);

            ShowCarModel show = new ShowCarModel();
            var model = _context.CarModel.Where(o => o.BrandId == brandid).ToList();
            foreach(CarModel carIn in model)
            {
                ListCarModel models = new ListCarModel();
                models.Model_Id = carIn.Model_Id;
                models.ModelName = carIn.ModelName;
                show.carmodels.Add(models);
            }
            CarModelRespons reponse = new CarModelRespons();
            reponse.Success = true;
            reponse.Message = "สำเร็จ";
            reponse.Carmodel = show;
            return Json(reponse);
        }
        [HttpGet]
        public IActionResult PackageAll()
        {
            PackageAllResponse packageAll = new PackageAllResponse();
            List<PackageAll> all = new List<PackageAll>();
            // List<ListPrice> listPackageDb = new List<ListPrice>();
            List<Package> packageallp = _context.Package.Include(o => o.Size).Include(o=>o.ModelPackage).OrderBy(o=>o.ModelPackageId).ToList();

            PackageAll listPackageDb = new PackageAll();
            foreach(Package package in packageallp)
            {

                
                ListPackageName listPackage = new ListPackageName();
                listPackage.Name = package.ModelPackage.PackageName;
                listPackageDb.packageNames.Add(listPackage);
               
               
            }
            List<Package> packageallprice = _context.Package.Include(o => o.ModelPackage).OrderBy(o => o.PackageId).ToList();
            foreach(Package package1 in packageallprice)
            {
                ListPrice listPackage1 = new ListPrice();
                listPackage1.Prices = package1.Price.ToString();
                listPackageDb.price.Add(listPackage1);
            }
            all.Add(listPackageDb);

            packageAll.Message = "สำเร็จ";
            packageAll.Success = true;
            packageAll.Packagealls = all;
            return Json(packageAll);
        }

        private int UserId()
        {
            string claimUserId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = int.Parse(Id);
            return userId;
        }
        [HttpGet]
        public IActionResult CheckEmployee(int Id)
        {
            IdResponse response = new IdResponse();
            Job jobdb = _context.Job.Include(o => o.Employee).Include(o => o.Package.ModelPackage).Include(o => o.Customer)
                        .Include(o => o.Package).Include(o => o.Car).Where(o => o.CustomerId == Id)
                        .OrderByDescending(o => o.JobId).FirstOrDefault();
            var EmpId = jobdb.EmployeeId;
            response.Id = EmpId.GetValueOrDefault(); ;
            return Json(response);
        }
        [HttpGet]
        public IActionResult Userinfo(int Id)
        {
            try
            {
                User user = _context.User.Where(o => o.UserId == Id).FirstOrDefault();
                UserInfo userInfo = new UserInfo();
                userInfo.UserId = user.UserId;
                userInfo.FullName = user.FullName;
                userInfo.IdCardNumber = user.IdCardNumber;
                userInfo.Phone = user.Phone;
                userInfo.Code = user.Code;
                userInfo.Image = user.Image;
                return Json(userInfo);
            }
            catch(Exception)
            {

            }
            return Ok();
        }
        [HttpPost]
        public IActionResult getemp([FromBody] ReqChat req)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                Chat chat = new Chat();
                chat.Name = req.name;
                chat.Message = req.message;
                _context.Chat.Add(chat);
                _context.SaveChanges();
                response.Message = "สำเร็จ";
                response.Success = true;
            }
            catch
            {
                response.Message = "ไม่สำเร็จ";
                response.Success = false;
            }
            return Json(response);
        }

    }

}
