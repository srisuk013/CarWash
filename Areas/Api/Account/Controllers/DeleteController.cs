using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Areas.Api.Account.Controllers
{
    [Area("Api")]

    public class DeleteController : Controller
    {

        private CarWashContext _context;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public DeleteController(CarWashContext context, UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsp([FromBody]DeleteAspNet delete)
        {

            IdentityUser deleteUser = await _userManager.FindByNameAsync(delete.Username);
            string message = "";
            if (deleteUser == null)
            {
                message = "ลบสำเร็จ";
                return BadRequest(message);
            }
            else if (deleteUser != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(deleteUser);

                if (result == IdentityResult.Success)
                {
                    User user = _context.User.Where(o => o.Username == delete.Username).FirstOrDefault();
                    _context.User.Remove(user);
                    _context.SaveChanges();
                }
            }
            return Ok();
        }

    }
}