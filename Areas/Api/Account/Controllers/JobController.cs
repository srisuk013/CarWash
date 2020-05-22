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

    public class JobController : Controller
    {

        private CarWashContext _context;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public JobController(CarWashContext context, UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public string RunnigJob()
        {
            string code = "JOB";
            int countRunning = _context.Job.Where(o => o.JobDateTime.Year == DateTime.Now.Year
            && o.JobDateTime.Month == DateTime.Now.Month).Count() + 1;
            string codeSum = DateTime.Now.ToString("yyMM") + countRunning.ToString().PadLeft(4, '0');

            return code + codeSum;
        }

        [HttpGet]
        [Route("/api/Job")]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Job()
        {
            string userId = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            String Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idName = int.Parse(Id);
            Job job = _context.Job.Where(o => o.JobId == idName).FirstOrDefault();
            job.CodeJob = RunnigJob();
            _context.Job.Add(job);
            _context.SaveChanges();
            BaseResponse response = new BaseResponse();
            response.Success = true;
            response.Message = "เปลียนstateสำเร็จ";
            return Json(response);


           
        }

        [HttpPost]
        [Route("/api/payment")]
        [ServiceFilter(typeof(CarWashAuthorization))]
        public IActionResult Payment()
        {
            BaseResponse response = new BaseResponse();
            response.Success = true;
            response.Message = "Payment success";
            return Json(response);
        }


    }
}