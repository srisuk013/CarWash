﻿using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class ShowInformationResponse :BaseResponse
    {
        public ShowModelCar Brand { get; set; }
    }
}
