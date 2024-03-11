using Grpc.Core;
using LawetaWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LawetaWeb.Controllers
{
    public class TransportsController : Controller
    {
        private readonly DataModel _dataModel;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TransportsController(DataModel dataModel, IWebHostEnvironment environment)
        {
            _webHostEnvironment = environment;
            _dataModel = dataModel;
        }        
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            return View(_dataModel);
        }
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            TransportInfo transport = new();
            return View(transport);
        }
        public ActionResult Add(TransportInfo transport, IFormFile imageFile)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            if (imageFile != null && imageFile.Length > 0) 
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "data/images");
                string uniqueFileName = Guid.NewGuid().ToString()+"_"+imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath,FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
                transport.ImagePath = "/data/images/" + uniqueFileName;
            }
            
            _dataModel.Transports.Insert(0, transport);
            _dataModel.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id) 
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            string filePath = default;
            if (_dataModel.Transports[id].ImagePath != null)
            {
                filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                _dataModel.Transports[id].ImagePath.TrimStart('/'));
            }

            if(System.IO.File.Exists(filePath)) 
            {
                System.IO.File.Delete(filePath);
            }

            _dataModel.Transports.RemoveAt(id);
            _dataModel.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
