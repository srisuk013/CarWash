using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

Skip to content
Search or jump to…

Pull requests
Issues
Marketplace
Explore

@srisuk013
Learn Git and GitHub without any code!
Using the Hello World guide, you’ll start a branch, write comments, and open a pull request.


srisuk013
/
CarWash
1
10
 Code Issues 0 Pull requests 0 Actions Projects 0 Wiki Security Insights Settings
CarWash/Areas/Api/Account/Controllers/AccountController.cs
@srisuk013 srisuk013 dentityRole
c665c2b 4 days ago
85 lines(68 sloc)  2.23 KB
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using CarWash.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CarWash.Areas.Api.Models;

namespace CarWash.Areas.Account
{
    [Area("Api")]
    //[Authorize(Roles = "Employee, Customer")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly CarWashContext _context;
        public AccountController(CarWashContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityUser> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [HttpPost]

        public async Task<IActionResult> Register([FromForm]ReqRegister req)

        {
          
            IdentityUser Users = new IdentityUser();
            Users.UserName = req.Username;
          

            IdentityResult result = await _userManager.CreateAsync(Users,req.Password);
            
            if (result == IdentityResult.Success)
            {
               

                //return Ok();
            }
            else
            {
                //return BadRequest();
            }
            return Json(result);

        }

    }

}


                

        


