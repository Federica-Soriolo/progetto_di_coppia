﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Sensors
{
    public interface BatteryInterface
    {
        int GetBattery(int status);
    }
}
