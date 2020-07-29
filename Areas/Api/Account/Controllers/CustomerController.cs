using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsReponse;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]
    [Route("Api/Customer/[Action]")]
    [ServiceFilter(typeof(CarWashAuthorization))]

    public class CustomerController : Controller
    {

        private CarWashContext _context;

        public CustomerController(CarWashContext context)
        {
            _context = context;
        }
        [HttpGet]
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
            ShowInformationReponse reponse = new ShowInformationReponse();
            reponse.Success = true;
            reponse.Message = "สำเร็จ";
            reponse.Shows = showModel;
            return Json(reponse);
        }

        [HttpPost]
        public IActionResult CarInformation([FromBody] ReqCarInformation req )
        {
            string claimuserid = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            string Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userid = int.Parse(Id);
            Car car = new Car();
            car.UserId = userid;
            car.ProvinceId = req.province;
            car.BrandId =req.BaBrandId ;
            car.Image = req.ImageCarUrl;
            car.Model_Id = req.ModelId;
            car.VehicleRegistration = req.VehicleRegistration;
            return Ok();
        }
    }
}