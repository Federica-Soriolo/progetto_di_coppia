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
        Random rng = new Random();
        int Lat;
        int Lon;
        public Position GetPosition()
        {
            Lat = rng.Next(516400146, 630304598);
            Lon = rng.Next(224464416, 341194152);

            Position p = new Position()
            {
                Latitude = 18d + Lat / 1000000000d,
                Longitude = -72d - Lon / 1000000000d
            };

            return p;

            
        }
    }
}
