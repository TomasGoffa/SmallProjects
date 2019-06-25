/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using APIS2.Models;

    public class WebAPIController : ApiController
    {
        [Route("reporting")]
        public HttpResponseMessage Post(Order finalManufacturingReport)
        {

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
