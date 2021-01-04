using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Models
{
    public class TableModel : TableEntity
    {
        public TableModel()
        {

        }

        public TableModel(Guid id, string deviceId, int speed, int battery, double latitude, double longitude)
        {
            
            DeviceId = deviceId;
            Speed = speed;
            Battery = battery;
            Latitude = latitude;
            Longitude = longitude;

            PartitionKey = deviceId;
            RowKey = id.ToString();
        }
        public string DeviceId { get; set; }
        public int Speed { get; set; }
        public int Battery { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        

    }
}
