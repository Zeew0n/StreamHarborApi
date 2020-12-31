using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkFlowTaskManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }


        /// <summary>
        /// Gets the action result value and returns it in desired format
        /// </summary>
        /// <param name="errorMessage">error message</param>
        /// <param name="statusCode">http status code</param>
        /// <returns>new object</returns>
        protected object HandleActionResult(string errorMessage, int statusCode)
        {
            return new { statusCode, errorMessage };
        }
    }
}