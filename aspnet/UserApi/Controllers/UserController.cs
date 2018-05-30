using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [Route("api/User")]
    public class UserController : Controller
    {
        // GET api/user
        [HttpGet]
        public string Get()
        {
            return "From UserApi Service：李四";
        }

      
    }
}
