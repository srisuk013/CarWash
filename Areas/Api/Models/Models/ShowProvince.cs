using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ShowProvince
    {
        public List<ProvinceModel> models;




        public ShowProvince()
        {
            this.models = new List<ProvinceModel>();

        }
    }
}
