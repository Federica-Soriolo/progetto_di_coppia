using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Client.Sensors
{
    public class BatterySensor : BatteryInterface, SensorInterface
    {
        public bool check { get; set; }
        public int GetBattery(int status)
        {
            if (!check)
            {
                check = true;
                return status;
            }
            else
            {
                status--;
                return status;
            }
        }
    }
}
