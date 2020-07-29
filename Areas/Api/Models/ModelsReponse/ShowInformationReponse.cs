using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class ShowInformationReponse :BaseResponse
    {
        public ShowModelCar Shows { get; set; }
    }
}
