﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public static Boolean PhoneCheck(string phone)
        {
            bool Check = false;

            var prefix = phone.Substring(0, 2);
            if (String.IsNullOrEmpty(phone))
            {
                Check = false;
            }
            else if (phone.Length != 10)
            {
                Check = false;
            }
            else if (prefix == "08" || prefix == "09" || prefix == "06")
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
            for (int i = 0; i < idc.Length; i++)
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
            if (String.IsNullOrEmpty(FullName))
            {
                Check = false;
            }
            else if (FullName.Length < 2)
            {
                Check = false;
            }
            else if (FullName.Length < 3)
            {
                Check = false;
            }
            else if (FullName.Length < 4)
            {
                Check = false;
            }
            else if (FullName.Length < 5)
            {
                Check = false;
            }
            else if (FullName.Length < 6)
            {
                Check = false;
            }
            else if (FullName.Length < 7)
            {
                Check = false;
            }
            else if (FullName.Length < 8)
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
            if (String.IsNullOrEmpty(PassWord))
            {
                Check = false;
            }
            else if (PassWord.Length < 8)
            {
                Check = false;
            }
            else
            {
                Check = true;
            }
            return Check;
        }
       /* public static Image ResizeImage(Image image,Size newsize)
        {
            Image newImage = new Bitmap(newsize.Width,newsize.Height);
            using (Graphics GFX = Graphics.FromImage((Bitmap)newImage))
            {
                GFX.DrawImage(image, new System.Drawing.Rectangle(System.Drawing.Point.Empty,newsize));
            }
            return newImage;
        }*/
        public static Boolean CheckState(int state)
        {
            bool Check = false;
            if (String.IsNullOrEmpty(state.ToString()))
            {
                Check = false;
            }
            else if (state != 0 && state !=1)
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
             if (String.IsNullOrEmpty(Role.ToString()))
            {
                Check = false;
            }
            else if (Role != 1 && Role != 3 && Role !=2)
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
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}