﻿using CarWash.Areas.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsReponse
{
    public class UpImageResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ServiceImage ServiceImage { get; set; }

    }
}
