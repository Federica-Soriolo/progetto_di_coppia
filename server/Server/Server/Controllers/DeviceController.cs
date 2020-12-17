using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class DeviceController : Controller
    {

        [Route("/scooters/{deviceId}")]
        [HttpPost]
        public ActionResult Post(int id, [FromBody] DataModel data)
        {
            if (data != null)
            {
                //Console.WriteLine(data);
                return Ok();
            }
            else return BadRequest();

        }

        
    }
}
