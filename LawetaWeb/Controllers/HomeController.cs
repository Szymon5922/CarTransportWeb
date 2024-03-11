using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using LawetaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LawetaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataModel _data;
        public HomeController(DataModel data)
        {            
            _data = data;
        }

        public IActionResult Index()
        {
            return View(_data);
        }
    }    
}
