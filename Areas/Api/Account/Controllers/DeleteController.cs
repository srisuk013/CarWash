using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Areas.Api.Account.Controllers
{
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

        public async Task<IActionResult> DeleteAsp()
        {
             string aspNetDelete = "";
            IdentityUser deleteUser = await _userManager.FindByNameAsync(aspNetDelete);
            if (deleteUser != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(deleteUser);
                if (result == IdentityResult.Success)
                {

                
                }
                return Ok();

            }
            return Ok();
        }
        
    }
}