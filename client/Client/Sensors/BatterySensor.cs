using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Client.Sensors
{
    public class BatterySensor : BatteryInterface, SensorInterface
    {
        public int GetBattery()
        {
            var random = new Random();
            return random.Next(100);
        }
    }
}
