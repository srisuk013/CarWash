using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CarWash.Areas.Api
{
    public class Api_RouteConfig
    {
        public static IEndpointRouteBuilder Config(IEndpointRouteBuilder endpoints)
        {
            // Area - Api

            endpoints.MapAreaControllerRoute(
            name: "Api - Default",
            areaName: "Api",
            pattern: "Api/{controller}/{action}/{id?}",
            defaults: new { area = "Api", action = "Index" }
            );

            return endpoints;
        }
    }
}
