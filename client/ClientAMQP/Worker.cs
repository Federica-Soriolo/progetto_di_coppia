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

namespace ClientAMQP
{
    public class Worker : BackgroundService
    {
        
        private readonly ILogger<Worker> _logger;
        private static System.Timers.Timer batterytimer;
        private static VirtualSpeedSensor speed = new VirtualSpeedSensor();
        private static BatterySensor battery = new BatterySensor();
        private static PositionSensor position = new PositionSensor();

        private static int status = 100;
        private string clientId = "Scooter1";
        
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Amqp protocol = new Amqp("Scooter");
            SetTimer();
            battery.check = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                
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
                protocol.Send(json);
                await Task.Delay(20000, stoppingToken);
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
