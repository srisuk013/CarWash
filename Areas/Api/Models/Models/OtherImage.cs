﻿using CarWash.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.Models
{
    public class OtherImage
    {
        public int ImageId { get; set; }
        public string Image { get; set; }

        public OtherImage(OthrerImage otherImage)
        {
            ImageId = otherImage.ImageId;
            Image = otherImage.Image;
        }
    }
}
