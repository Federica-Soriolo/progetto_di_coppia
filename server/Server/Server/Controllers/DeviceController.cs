using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IActionsRepository _actionsRepository;
        public DeviceController(IActionsRepository actionsRepository)
        {
            _actionsRepository = actionsRepository;
        }
        [Route("/scooters/{deviceId}")]
        [HttpPost]
        public ActionResult Post(string deviceId, [FromBody] DataModel data)
        {
            if (data != null)
            {
                _actionsRepository.TableServiceAsync(deviceId, data);
                return Ok();
            }
            else return BadRequest();

        }
        
    }
}
