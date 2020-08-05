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
using Newtonsoft.Json;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]
    [Route("Api/Customer/[Action]")]


    public class CustomerController : Controller
    {

        private CarWashContext _context;

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
            ShowModelCar showModel = new ShowModelCar();
            var Car = _context.CarModel.Include(o => o.Brand).Include(o => o.Size).ToList();
            foreach(CarModel carIn in Car)
            {
                CarModels models = new CarModels();
                models.Model_Id = carIn.Model_Id;
                models.ModelName = carIn.ModelName;
                models.BrandId = carIn.Brand.BrandId;
                models.BrandName = carIn.Brand.BrandName;
                models.SizeId = carIn.Size.SizeId;
                models.SizeName = carIn.Size.SizeName;
                showModel.models.Add(models);
            }
            var province = _context.Province.Include(o => o.Car).ToList();
            foreach(Province provincename in province)
            {
                Listprovince listprovince = new Listprovince();
                listprovince.ProvinceId = provincename.ProvinceId;
                listprovince.ProvinceName = provincename.ProvinceName;
                showModel.province.Add(listprovince);
            }
            ShowInformationResponse reponse = new ShowInformationResponse();
            reponse.Success = true;
            reponse.Message = "สำเร็จ";
            reponse.Shows = showModel;
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
                var package = _context.Package.Include(o => o.ModelPackage).Include(o=>o.Size).Where(o => o.SizeId == modlesize.SizeId).ToList();
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
                if(check==null)
                {
                    ShowPackageAllResponse showPackage = new ShowPackageAllResponse();
                    List<ListPackageAll> listPackageDb = new List<ListPackageAll>();
                    List<ListPackageAll> listPackageDbV1 = new List<ListPackageAll>();
                    var packageall = _context.Package.Include(o => o.ModelPackage).Include(o => o.Size).Where(o => o.ModelPackageId == 1).ToList();
                    foreach(Package package in packageall)
                    {
                        ListPackageAll listPackage = new ListPackageAll();
                        listPackage.Packagename = package.ModelPackage.PackageName;
                        listPackage.SizeName = package.Size.SizeName;
                        listPackage.Price = package.Price.ToString();
                        listPackageDbV1.Add(listPackage);
                    }
                    var packageall1 = _context.Package.Include(o => o.ModelPackage).Include(o => o.Size).Where(o => o.ModelPackageId == 2).ToList();
                    foreach(Package package in packageall)
                    {
                        ListPackageAll listPackage = new ListPackageAll();
                        listPackage.Packagename = package.ModelPackage.PackageName;
                        listPackage.SizeName = package.Size.SizeName;
                        listPackage.Price = package.Price.ToString();
                        listPackageDb.Add(listPackage);
                    }
                    showPackage.Message = "สำเร็จ";
                    showPackage.Success = true;
                    showPackage.PackageCarV1 = listPackageDbV1;
                    showPackage.packageCar = listPackageDb;
                    return Json(showPackage);

                }
                else if(check!=null)
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



    }
}