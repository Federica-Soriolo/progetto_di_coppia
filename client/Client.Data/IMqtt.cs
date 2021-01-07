using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Data
{
    public interface IMqtt
    {
        void Subscribe(string topic);
        void Publish(string topic, string message);

    }
}
