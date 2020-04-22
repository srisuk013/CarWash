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
using Org.BouncyCastle.Ocsp;

namespace CarWash.Areas.Account
{
    [Area("Api")]
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly CarWashContext _context;
        private Role reqRole;

        public AccountController(CarWashContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        private string RunnigCodeId(int role)
        {
         
            int countRunning = _context.User.Where(o => o.Role == role
            && o.CreatedTime.Year == DateTime.Now.Year
            && o.CreatedTime.Month == DateTime.Now.Month).Count() + 1;
            string codeSum = DateTime.Now.ToString("yyMM") + countRunning.ToString().PadLeft(4, '0');
            string code = "";
            if (role == Role.Admin)
            {
                code = "Adm" + codeSum;
            }
            else if (role == Role.Customer)
            {
                code = "Cus" + codeSum;
            }
            else if (role == Role.Employee)
            {
                code = "Emp" + codeSum;
            }
            return code;
        }
        private Boolean VerifyPeopleID(String pid)
        {
            string idc = pid.Substring(0, 12);

            int sumValue = 0;
            for (int i = 0; i < idc.Length; i++)
            {
                sumValue += (13 - i) * int.Parse(idc[i].ToString());
            }
            int v = (11 - (sumValue % 11))%10;
            string realIdentityCard = (idc + v);
            return realIdentityCard != pid;
        }
       
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] ReqRegister req)
        {
            try
            {
                IdentityUser aspnetUserCheck = await _userManager.FindByNameAsync(req.Username.ToLower());
                User phoneCheck = _context.User.Where(o => o.Phone == req.Phone).FirstOrDefault();
                User idCardNumberCheck = _context.User.Where(o => o.IdCardNumber == req.IdCardNumber).FirstOrDefault();
                Boolean success = false;
                string message = "";

                if (req.Username.Length < 4)
                {
                    message = "กรุณากรอกชื่อผู้ใช้งานมากกว่า 4 ตัวอักษร";
                    return BadRequest(message);

                }
                if (String.IsNullOrEmpty(req.Username))
                {
                    message = "กรุณากรอกUser";
                    return BadRequest(message);
                }

                if (aspnetUserCheck != null)
                {
                    message = "มีผู้ใช้งานแล้ว";
                    return BadRequest(message);
                }

                if (String.IsNullOrEmpty(req.Password))
                {
                    message = "กรุณากรอกPassword";
                    return BadRequest(message);

                }
                if (req.Password.Length < 8)
                {
                    message = "กรุณากรอกชื่อผู้ใช้งานมากกว่า 8 ตัวอักษร";
                    return BadRequest(message);

                }
                if (String.IsNullOrEmpty(req.Phone))
                {
                    message = "กรุณากรอกPhone";
                    return BadRequest(message);

                }
                if (phoneCheck != null)
                {
                    message = "เบอร์นี้มีผู้ใช้งานแล้ว";
                    return BadRequest(message);

                }
                if (req.Phone.Length != 10)
                {

                    message = "กรุณากรอกเบอร์โทรศัพท์ให้ถูกต้อง";
                    return BadRequest(message);

                }
                if (String.IsNullOrEmpty(req.IdCardNumber))
                {
                    message = "กรุณากรอกIdCardNumber";
                    return BadRequest(message);
                }

                if (req.Phone.Length == 10)
                {
                    var prefix = req.Phone.Substring(0, 2);
                    if (prefix != "08" || prefix != "09" || prefix != "06")
                    {
                        message = "เบอร์โทรถูกต้อง";
                    }
                    else
                    {
                        message = "กรุณาตรวจสอบเบอร์ของท่านอีกครั้ง";
                        return BadRequest(message);
                    }
                }
                if (idCardNumberCheck != null)
                {
                    message = "เลขบัตรประชาชนซ้ำ";
                    return BadRequest(message);
                }
                
                if (req.IdCardNumber.Length != 13)
                {
                    message = "เลขบัตรประชาชนให้ครบ13";
                    return BadRequest(message);
                }
                if (VerifyPeopleID(req.IdCardNumber))
                {
                    message = "กรุณากรอกเลขบัตรประชาชนให้ถูกต้อง";
                    return BadRequest(message);

                }
                success = true;
                message = "สมัครสมาชิกเรียบร้อยแล้ว";

                IdentityUser aspnetUser = new IdentityUser();
                aspnetUser.UserName = req.Username;
                IdentityResult result = await _userManager.CreateAsync(aspnetUser, req.Password);
                if (result == IdentityResult.Success)
                {
                    string roleName = "";
                    if (req.Role == Role.Admin)
                    {
                        roleName = Role.Desc.Admin;
                    }
                    else if (req.Role == Role.Customer)
                    {
                        roleName = Role.Desc.Customer;

                    }
                    else if (req.Role == Role.Employee)
                    {
                        roleName = Role.Desc.Employee;
                    }
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(aspnetUser, roleName);
                    if (roleResult == IdentityResult.Success)
                    {
                        User user = new User();
                        user.AspNetRole = roleName;
                        user.AspNetUserId = aspnetUser.Id;
                        user.CreatedTime = DateTime.Now;
                        user.UpdatedTime = DateTime.Now;
                        user.State = State.Offline;               
                        user.Role = req.Role;
                        user.Status = Status.InActive;
                        user.Code = RunnigCodeId(req.Role);
                        user.FullName = req.FullName;
                        user.Username = req.Username;
                        user.Phone = req.Phone;
                        user.IdCardNumber = req.IdCardNumber;
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
                return BadRequest(e.Message);
            }
            return Ok();
            
        }

    }
    

}






































































































