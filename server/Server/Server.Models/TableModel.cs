﻿using Microsoft.Azure.Cosmos.Table;
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

        public TableModel(Guid id, string deviceId, int speed, int battery, decimal longitude, decimal latitude)
        {
            
            DeviceId = deviceId;
            Speed = speed;
            Battery = battery;
            Longitude = longitude;
            Latitude = latitude;

            PartitionKey = deviceId;
            RowKey = id.ToString();
        }
        public string DeviceId { get; set; }
        public int Speed { get; set; }
        public int Battery { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

    }
}
