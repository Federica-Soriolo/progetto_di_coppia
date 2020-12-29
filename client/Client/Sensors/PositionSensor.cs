using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Client.Models;

namespace Client.Sensors
{
    public class PositionSensor : PositionInterface, SensorInterface
    {
        public Position GetPosition()
        {
            Position p = new Position()
            {
                Latitude = 51.00089832M,
                Longitude = 0.001427437M
            };

            return p;
        }
    }
}
