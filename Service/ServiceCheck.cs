using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.ModelsConst;
using CarWash.Models.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;

namespace CarWash.Service
{

    public class ServiceCheck
    {

        private readonly CarWashContext _context;
        public ServiceCheck(CarWashContext context)
        {
            _context = context;
        }
        public static Boolean PhoneCheck(string phone)
        {
            bool Check = false;
          
            if(phone.Length != 10)
            {
                Check = true;
            }
            return Check;
        }
        public static Boolean ValidatePhone(string phone)
        {
            bool Check = false;
            var prefix = phone.Substring(0, 2);
            if(prefix == "08" || prefix == "09" || prefix == "06")
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            return Check;
        }

        public static Boolean VerifyPeopleID(String pid)
        {
            string idc = pid.Substring(0, 12);

            int sumValue = 0;
            for(int i = 0; i < idc.Length; i++)
            {
                sumValue += (13 - i) * int.Parse(idc[i].ToString());
            }
            int v = (11 - (sumValue % 11)) % 10;
            string realIdentityCard = (idc + v);
            return realIdentityCard != pid;
        }
        public static Boolean CheckfluFullName(string FullName)
        {
            bool Check = false;
            if(String.IsNullOrEmpty(FullName))
            {
                Check = false;
            }
            else if(FullName.Length < 8)
            {
                Check = false;
            }
            else
            {
                Check = true;
            }

            return Check;
        }
        public static Boolean CheckPassWord(string PassWord)
        {
            bool Check = false;
            if(String.IsNullOrEmpty(PassWord))
            {
                Check = false;
            }
            else if(PassWord.Length < 8)
            {
                Check = false;
            }
            else
            {
                Check = true;
            }
            return Check;
        }
        public static Boolean CheckState(int state)
        {
            bool Check = false;
            if(String.IsNullOrEmpty(state.ToString()))
            {
                Check = false;
            }
            else if(state != 0 && state != 1)
            {
                Check = false;
            }
            else
            {
                Check = true;
            }
            return Check;
        }
        public static Boolean CheckRole(int Role)
        {
            bool Check = false;
            if(String.IsNullOrEmpty(Role.ToString()))
            {
                Check = false;
            }
            else if(Role != 1 && Role != 3 && Role != 2)
            {
                Check = false;
            }
            else
            {
                Check = true;
            }
            return Check;
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using(var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using(var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public static Image resizeImage(Image image, Size size)
        {
            return (Image)(new Bitmap(image, size));
        }
        public static String Check(String phone, int? Id)
        {
            String code = null;

            if(Id == Status.Active)
            {
                code = "เบอร์นี้มีผู้ใช้งานอยู่แล้ว";
            }
            else if(Id == Status.InActive)
            {
                code = "เบอร์นี้มีผู้ใช้งานอยู่แล้ว";
            }
            else if(Id == Status.PendingApproval)
            {
                code ="กรุณารอการอนุมัติ";
            }
            return code;
        }
        public static DateTime DateTime(long longdatetime)
        {
            long unixDate = longdatetime;
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
            return date.Date;
        }
        public static String CheckImage(int Id)
        {
            String folderNameImage = null;
            if(Id==UpImage.FrontBefore)
            {
                folderNameImage = UpImage.Desc.FrontBefore;
            }
            else if(Id == UpImage.BackBefore)
            {
                folderNameImage = UpImage.Desc.BackBefore;
            }
            else if(Id == UpImage.LaftBefore)
            {
                folderNameImage = UpImage.Desc.LaftBefore;
            }
            else if(Id == UpImage.RightBefore)
            {
                folderNameImage = UpImage.Desc.RightBefore;
            }
            else if(Id==UpImage.FrontAfter)
            {
                folderNameImage = UpImage.Desc.FrontAfter;
            }
            else if(Id == UpImage.BackAfter)
            {
                folderNameImage = UpImage.Desc.BackAfter;
            }
            else if(Id == UpImage.LaftAfter)
            {
                folderNameImage = UpImage.Desc.LaftAfter;
            }
            else if(Id == UpImage.RightAfter)
            {
                folderNameImage = UpImage.Desc.RightAfter;
            }
            else if(Id == UpImage.OtherImage)
            {
                folderNameImage = UpImage.Desc.OtherImage;
            }
            return folderNameImage;

        }
      
        public static int CheckIntImage(int type)
        {
            int code = 0;

            if(type == UpImage.FrontBefore)
            {
                code = UpImage.FrontBefore;
            }
            else if(type == UpImage.BackBefore)
            {
                code = UpImage.BackBefore;
            }
            else if(type == UpImage.LaftBefore)
            {
                code = UpImage.LaftBefore;
            }
            else if(type == UpImage.RightBefore)
            {
                code = UpImage.RightBefore;
            }
            else if(type == UpImage.FrontAfter)
            {
                code = UpImage.FrontAfter;
            }
            else if(type == UpImage.BackAfter)
            {
                code = UpImage.BackAfter;
            }
            else if(type == UpImage.LaftAfter)
            {
                code = UpImage.LaftAfter;
            }
            else if(type == UpImage.RightBefore)
            {
                code = UpImage.RightBefore;
            }
            return code;
        }

    }
}
