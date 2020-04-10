using CarWash.Areas.Api.Models;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using CarWash.Models;
using Microsoft.AspNetCore.Authorization;
using MySqlX.XDevAPI.Common;

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
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        private Boolean VerifyPeopleID(String PID)
        {
            //ตรวจสอบว่าทุก ๆ ตัวอักษรเป็นตัวเลข
            if (PID.ToCharArray().All(c => char.IsNumber(c)) == false)
                return false;
            //ตรวจสอบว่าข้อมูลมีทั้งหมด 13 ตัวอักษร
            if (PID.Trim().Length != 13)
                return false;


            int sumValue = 0;
            for (int i = 0; i < PID.Length - 1; i++)
                sumValue += int.Parse(PID.ToString()) * (13 - i);
            int v = 11 - (sumValue % 11);
            return PID[12].ToString() == v.ToString();
        }



        [HttpPost]

        public async Task<IActionResult> Register([FromForm]ReqRegister req)
        {
            try
            {
                IdentityUser aspnetUserCheck = await _userManager.FindByNameAsync(req.Username.ToLower());
                User phoneCheck = _context.User.Where(o => o.Phone == req.Phone).FirstOrDefault();
                User idCardNamberCheck = _context.User.Where(o => o.IdCardNumber == req.IdCardNamber).FirstOrDefault();
                Boolean success = false;
                string message = "";

                if (req.Username.Length < 4)
                {
                    message = "กรุณากรอกชื่อผู้ใช้งานมากกว่า 4 ตัวอักษร";
                    return BadRequest();
                }
                else if (String.IsNullOrEmpty(req.Username))
                {
                    message = "กรุณากรอกUser";
                    return BadRequest();
                }

                else if (aspnetUserCheck != null)
                {
                    message = "มีผู้ใช้งานแล้ว";
                    return BadRequest();
                }

                else if (String.IsNullOrEmpty(req.Password))
                {
                    message = "กรุณากรอกPassword";
                    return BadRequest();
                }
                else if (req.Password.Length < 8)
                {
                    message = "กรุณากรอกชื่อผู้ใช้งานมากกว่า 8 ตัวอักษร";
                    return BadRequest();
                }
                else if (String.IsNullOrEmpty(req.Phone))
                {
                    message = "กรุณากรอกPhone";
                    return BadRequest();

                }
                else if (phoneCheck != null)
                {
                    message = "เบอร์นี้มีผู้ใช้งานแล้ว";
                    return BadRequest();
                }
                else if (req.Phone.Length == 10)
                {
                    var prefix = req.Phone.Substring(0, 2);
                    if (prefix != "08" && prefix != "09")

                        message = "กรุณาตรวจสอบเบอร์ของท่านอีกครั้ง";
                    return BadRequest();
                }


                else if (VerifyPeopleID(req.IdCardNamber))
                {
                    message = "กรุณากรอกเลขบัตรประชาชนให้ถูกต้อง";
                    return BadRequest();
                }
                else if (idCardNamberCheck != null)
                {
                    message = "เลขบัตรประชาชนซ้ำ";
                    return BadRequest();
                }
                else
                {
                    success = true;
                    message = "สมัครสมาชิกเรียบร้อยแล้ว";
                }
                IdentityUser aspnetUser = new IdentityUser();
                aspnetUser.UserName = req.Username;
                IdentityResult result = await _userManager.CreateAsync(aspnetUser, req.Password);

                if (result == IdentityResult.Success)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(aspnetUser, "Employee");
                    if (roleResult == IdentityResult.Success)
                    {
                        User user = new User();
                        user.AspNetRole = "Employee";
                        user.AspNetUserId = aspnetUser.Id;
                        user.CreatedTime = DateTime.Now;
                        user.FullName = req.FullName;
                        user.Username = req.Username;
                        user.Phone = req.Phone;
                        user.IdCardNumber = req.IdCardNamber;
                        _context.User.Add(user);
                        _context.SaveChanges();
                        return Json(result);

                    }
                }
            }
            catch (Exception e)
            {
                IdentityUser deleteUser = await _userManager.FindByNameAsync(req.Username.ToLower());
                if (deleteUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(deleteUser);
                    User user = _context.User.Where(o => o.Username == req.Username).FirstOrDefault();
                    if (user != null)
                    {
                        _context.User.Remove(user);
                        _context.SaveChanges();
                    }
                }
                return BadRequest();
            }
            return Ok();
        }

    }

}








                    












               




             
















































