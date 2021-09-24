using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiPedidos.Controllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("echo")]
        public IHttpActionResult Echo()
        {
            return Ok(true);
        } 
     
    }
}
