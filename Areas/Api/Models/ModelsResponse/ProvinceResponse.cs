﻿using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsResponse
{
    public class ProvinceResponse  :BaseResponse
    {
        public ShowProvince Shows { get; set; }
    }
}
