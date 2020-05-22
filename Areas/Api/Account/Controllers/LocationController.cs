using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]

    public class LocationController : Controller
    {

        private CarWashContext _context;


        public LocationController(CarWashContext context
           )
        {
            _context = context;

        }
        [HttpPost]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Location([FromBody]ReqLocation location)
        {
             if (String.IsNullOrEmpty(location.latitude.ToString()))
            {
                return BadRequest();
            }
            else if(String.IsNullOrEmpty(location.longitude.ToString()))
            {
                return BadRequest();
            }
  
            try
            {
                string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int idName = int.Parse(Id);
                User user = _context.User.Where(o => o.UserId == idName).FirstOrDefault();
                user.Latitude = location.latitude;
                user.Longitude = location.latitude;
                _context.User.Update(user);
                _context.SaveChanges();
                BaseResponse response = new BaseResponse();
                response.Success = true;
                response.Message = "บันทึกตำแหน่ง";
                return Json(response);
            }
            catch (Exception e)
            {

            }
            return Ok();
        }

    }
}