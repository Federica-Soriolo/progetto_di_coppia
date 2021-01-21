using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Data
{
    public interface IAmqp
    {
        void Send(string data);
    }
}
