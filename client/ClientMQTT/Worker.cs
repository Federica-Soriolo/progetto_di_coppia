using Client.Data;
using Client.Models;
using Client.Sensors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using uPLibrary.Networking.M2Mqtt;

namespace ClientMQTT
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static System.Timers.Timer batterytimer;
        private static VirtualSpeedSensor speed = new VirtualSpeedSensor();
        private static BatterySensor battery = new BatterySensor();
        private static PositionSensor position = new PositionSensor();

        private string clientId = "Scooter1";
        private string topicSummary = "Scooter/Scooter1/Summary";
        private string topicRace = "Scooter/Scooter1/Cmd/Race";
        private string topicScooter = "Scooter/Scooter1/Cmd/Scooter";
        private string topicLed = "Scooter/Scooter1/Cmd/Led";
        private string topicDisplay = "Scooter/Scooter1/Cmd/Display";
        

        private static int status = 100;
        private static double lastLongitude;
        private static double lastLatitude;
        private static int delay;


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            Mqtt mqtt = new Mqtt(clientId);

            mqtt.Subscribe(topicRace);
            mqtt.Subscribe(topicScooter);
            mqtt.Subscribe(topicLed);
            mqtt.Subscribe(topicDisplay);
            mqtt.Subscribe(topicSummary);

            battery.check = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (mqtt.Scooter && status > 0)
                {
                    if (mqtt.Race)
                    {
                        SetTimer(10000);

                        lastLongitude = position.GetPosition().Longitude;
                        lastLatitude = position.GetPosition().Latitude;

                        Detection d = new Detection()
                        {
                            DeviceId = clientId,
                            Battery = status,
                            Speed = speed.GetSpeed(),
                            Longitude = lastLongitude,
                            Latitude = lastLatitude
                        };

                        battery.check = true;
                        var json = JsonConvert.SerializeObject(d);

                        mqtt.Publish(topicSummary, json);

                        delay = 5000;

                    }
                    else
                    {
                        SetTimer(30000);

                        Detection d = new Detection()
                        {
                            DeviceId = clientId,
                            Battery = status,
                            Speed = 0,
                            Longitude = lastLongitude,
                            Latitude = lastLatitude
                        };

                        battery.check = true;
                        var json = JsonConvert.SerializeObject(d);

                        mqtt.Publish(topicSummary, json);

                        delay = 20000;
                    }
               }

                await Task.Delay(delay, stoppingToken);
            }
        }

        private static void SetTimer(int interval)
        {
            // Create a timer with a two second interval.
            batterytimer = new System.Timers.Timer(interval);
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
