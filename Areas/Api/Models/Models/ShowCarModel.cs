using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class ShowCarModel
    {
        public List<ListCarModel> carmodels;




        public ShowCarModel()
        {
            this.carmodels = new List<ListCarModel>();

        }
    }
}
