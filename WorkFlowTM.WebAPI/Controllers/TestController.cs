using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WorkFlowTM.WebAPI.Controllers
{
    public class TestController : Controller
    {
        //Added comments on method name
        public IActionResult Index()
        {
            return View();
        }
    }
}