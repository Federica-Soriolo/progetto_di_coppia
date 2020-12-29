using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Models
{
    public class Detection
    {
        public int Speed { get; set; }
        public int Battery { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
