using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using System.Timers;

namespace Client
{
    class Program
    {
        private static System.Timers.Timer batterytimer;
        private static VirtualSpeedSensor speed = new VirtualSpeedSensor();
        private static BatterySensor battery = new BatterySensor();
        private static PositionSensor position = new PositionSensor();

        private static int status = 100;
        private string clientId = "Scooter1";

        static void Main(string[] args)
        {
            SetTimer();
            battery.check = false;

            while (true)
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44392/scooters/1");
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                Detection d = new Detection()
                {
                    DeviceId = clientId,
                    Battery = status,
                    Speed = speed.GetSpeed(),
                    Latitude = position.GetPosition().Latitude,
                    Longitude = position.GetPosition().Longitude,
                };
                battery.check = true;

                var json = JsonConvert.SerializeObject(d);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Console.Out.WriteLine(json);

                httpResponse.Close();
                System.Threading.Thread.Sleep(2000);

            }


        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            batterytimer = new System.Timers.Timer(10000);
            // Hook up the Elapsed event for the timer. 
            batterytimer.Elapsed += OnTimedEvent;
            batterytimer.AutoReset = true;
            batterytimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            status = battery.GetBattery(status);

        }

    }
}
