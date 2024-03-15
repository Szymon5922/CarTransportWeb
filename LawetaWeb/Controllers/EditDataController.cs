using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LawetaWeb.Models;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.Xml;
using System.Reflection;

namespace LawetaWeb.Controllers
{
    public class EditDataController : Controller
    {
        private readonly DataModel _dataModel;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _uploadsFolder => Path.Combine(_webHostEnvironment.WebRootPath, "data/images");
        public EditDataController(DataModel dataModel, IWebHostEnvironment webHostEnvironment) 
        {
            _dataModel = dataModel;
            _webHostEnvironment = webHostEnvironment;
        }        
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index","Login");

            return View(_dataModel);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult General()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            return View(_dataModel);
        }
        [HttpPost]
        public ActionResult EditGeneral(DataModel dataModel)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            try
            {
                _dataModel.SaveChanges(dataModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }        
        public ActionResult AddNumber() 
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            return View();
        }
        [HttpPost]
        public ActionResult AddNumber(int number,string name)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            _dataModel.PhoneNumbers.Add(number, name);
            _dataModel.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public ActionResult DeleteNumber(int number) 
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            if (_dataModel.PhoneNumbers.ContainsKey(number))
                _dataModel.PhoneNumbers.Remove(number);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Images()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            return View(_dataModel);
        }
        public ActionResult ChangeImages(IFormFile imageBg, IFormFile image1, IFormFile image2)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            ImageChangeProcess(nameof(_dataModel.ImageBackgroundPath), imageBg);
            ImageChangeProcess(nameof(_dataModel.Image1Path), image1);
            ImageChangeProcess(nameof(_dataModel.Image2Path), image2);

            _dataModel.SaveChanges();

            void ImageChangeProcess(string propName,IFormFile image) 
            {
                if (image == null || image.Length == 0)
                    return;

                Type objType = typeof(DataModel);
                var propertyToUpdate = objType.GetProperty(propName);

                if (propertyToUpdate == null)
                    throw new Exception("Unknow property");

                DeleteImage(propertyToUpdate);
                string newPath = AddImage(image);
                SetPath(propertyToUpdate, newPath);
            }
            string AddImage(IFormFile image) 
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(_uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
                
                string pathForDataModel = "/data/images/" + uniqueFileName;
                return pathForDataModel;
            }
            void DeleteImage(PropertyInfo property)
            {
                var propValue = property.GetValue(_dataModel);

                if (propValue == null)
                    return;
                
                string imageToDeletePath = Path.Combine(_webHostEnvironment.WebRootPath, propValue.ToString().TrimStart('/'));

                if (System.IO.File.Exists(imageToDeletePath))
                    System.IO.File.Delete(imageToDeletePath);
            }
            void SetPath(PropertyInfo property, string path)
            {
                property.SetValue(_dataModel, path);
            }

            return RedirectToAction("Images");
        }
    }
}
