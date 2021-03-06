﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.Service
{
    public class Config
    {
        public string EventSource { get; set; }
        public string EventLog { get; set; }
        public string ID { get; set; }
        public string PASS { get; set; }
        public double Lat { get; set; }
        public double lng { get; set; }
        public string TwilightUri { get; set; }
        public string SunrisePosition { get; set; }
        public string SunsetPosition { get; set; }
        public List<Device> Devices { get; set; }
    }

    public class Device
    {
        public string Name { get; set; }
        public string SunrisePosition { get; set; }
        public string SunsetPosition { get; set; }
    }
}
