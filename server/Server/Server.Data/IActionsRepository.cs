using Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{
    public interface IActionsRepository
    {
        Task TableServiceAsync(string deviceId, DataModel data);
    }
}
