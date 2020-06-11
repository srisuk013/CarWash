using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models.ModelsConst
{
    public class UpImage
    {
        public const int Front = 1;
        public const int Back = 2;
        public const int Laft = 3;
        public const int Right = 4;
        public const int OtherImage = 5;
        public class Desc
        {
            public const string Front = "ImageFront";
            public const string Back = "ImageBack";
            public const string Left = "ImageLeft";
            public const string Right = "ImageRight";
            public const string OtherImage = "ImageOtherImage";

        }
    }

   

}
