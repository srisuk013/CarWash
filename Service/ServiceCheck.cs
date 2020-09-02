using CarWash.Areas.Api.Models;
using CarWash.Areas.Api.Models.Models;
using CarWash.Areas.Api.Models.ModelsConst;
using CarWash.Models.DBModels;
using Firebase.Auth;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Image = System.Drawing.Image;


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
                code = "กรุณารอการอนุมัติ";
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
            if(Id == UpImage.FrontBefore)
            {
                folderNameImage = UpImage.Desc.FrontBefore;
            }
            else if(Id == UpImage.BackBefore)
            {
                folderNameImage = UpImage.Desc.BackBefore;
            }
            else if(Id == UpImage.LeftBefore)
            {
                folderNameImage = UpImage.Desc.LeftBefore;
            }
            else if(Id == UpImage.RightBefore)
            {
                folderNameImage = UpImage.Desc.RightBefore;
            }
            else if(Id == UpImage.FrontAfter)
            {
                folderNameImage = UpImage.Desc.FrontAfter;
            }
            else if(Id == UpImage.BackAfter)
            {
                folderNameImage = UpImage.Desc.BackAfter;
            }
            else if(Id == UpImage.LeftAfter)
            {
                folderNameImage = UpImage.Desc.LeftAfter;
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
        public static async Task<string> LocationAsync(Double lon, Double lat)
        {

            string Longitude = "lon=" + lon;
            string Latitude = "&lat=" + lat;
            string noelevation = "&noelevation=1";
            string key = "&key=c1d2a99899af37a0e2b5b1a3a1b1088e";
            string Baseurl = "https://api.longdo.com/map/services/address?" + Longitude+ Latitude + noelevation + key;
            Locations CusInFo = new Locations();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(Baseurl);
                if(Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    CusInFo = JsonConvert.DeserializeObject<Locations>(EmpResponse);
                }
                return CusInFo.subdistrict;
            }

        }
        public static async Task<string> DistanceAsync(double emplon, double emplat, double cuslon, double cuslat)
        {
            string key = "&mode=t&type=25&locale=th&key=c1d2a99899af37a0e2b5b1a3a1b1088e";
            string empLongitude = "flon=" + emplon.ToString();
            string empLatitude = "&flat=" + emplat.ToString();
            string cusLongitude = "&tlon=" + cuslon.ToString();  
            string cusLatitude = "&tlat=" + cuslat.ToString();
            
            string Baseurl = "https://mmmap15.longdo.com/mmroute/json/route/guide?" + empLongitude + empLatitude+ cusLongitude + cusLatitude + key;
            LocationReponse EmpInfo = new LocationReponse();
            using(var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync(Baseurl);
                //Checking the response is successful or not which is sent using HttpClient  
                if(Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<LocationReponse>(EmpResponse);
                }
                double Distance = EmpInfo.data.Select(o => o.distance).FirstOrDefault();
                Double DistanceSum = (Distance / 1000);
                string showDistance = String.Format("{0:0.0} km", DistanceSum);
                return showDistance;
            }
        }
        public static double CalculateDistance(double cuslat, double cuslon, double emplat, double emplon)
        {
            GeoCoordinate customer = new GeoCoordinate(cuslat, cuslon);
            GeoCoordinate Employee = new GeoCoordinate(emplat, emplon);

            double distanceBetween = customer.GetDistanceTo(Employee);
            double DistanceSum = (distanceBetween / 1000);

            return DistanceSum;
        }
       

    }
}
