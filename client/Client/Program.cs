using System;
using System.Collections.Generic;
using Client.Sensors;
using System.Net;
using System.IO;
using System.Collections;
using Client.Models;
using Newtonsoft.Json;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var speed = new VirtualSpeedSensor();
            var battery = new BatterySensor();
            var position = new PositionSensor();

            while (true)
            {
                Detection d = new Detection();

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44392/scooters/1");
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                d.Battery = battery.GetBattery();
                d.Speed = speed.GetSpeed();
                d.Latitude = position.GetPosition().Latitude;
                d.Longitude = position.GetPosition().Longitude;

                var json = JsonConvert.SerializeObject(d);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Console.Out.WriteLine(httpResponse.StatusCode);

                httpResponse.Close();
                System.Threading.Thread.Sleep(10000);

            }

        }

    }

}
