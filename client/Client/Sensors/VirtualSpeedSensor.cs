using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Client.Sensors
{
    public class VirtualSpeedSensor : SpeedSensorInterface, SensorInterface
    {
        public int GetSpeed()
        {
            var random = new Random();
            return random.Next(30);
        }
    }
}
