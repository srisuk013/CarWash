﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Hubs
{
    public class TimeHub:Hub
    {
        private IHubContext<TimeHub> TimeHup { get; set; }
    }
}
