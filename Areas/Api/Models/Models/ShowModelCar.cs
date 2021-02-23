using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ShowModelCar
    {
        public List<Brand> models;
         



        public ShowModelCar()
        {
            this.models = new List<Brand>();
           
        }



    }
}
