using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LawetaWeb.Models;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.Xml;

namespace LawetaWeb.Controllers
{
    public class EditDataController : Controller
    {
        private readonly DataModel _dataModel;
        private readonly IWebHostEnvironment _webHostEnvironment;
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

            Action<DataModel, string, IFormFile> changeProperty = (dataModel, propertyName, image) =>
            {
                string pathForModel = default;
                if (image != null && image.Length > 0)
                {                    
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "data/images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    pathForModel = "/data/images/" + uniqueFileName;
                    string oldPath = default;
                    switch (propertyName)
                    {
                        case nameof(dataModel.ImageBackgroundPath):
                            if (dataModel.ImageBackgroundPath != null)
                            {
                                oldPath =
                                Path.Combine(_webHostEnvironment.WebRootPath, dataModel.ImageBackgroundPath.TrimStart('/'));
                            }
                            dataModel.ImageBackgroundPath = pathForModel;
                            break;
                        case nameof(dataModel.Image1Path):
                            if (dataModel.Image1Path != null)
                            {
                                oldPath =
                                Path.Combine(_webHostEnvironment.WebRootPath, dataModel.Image1Path.TrimStart('/'));
                            }
                            dataModel.Image1Path = pathForModel;
                            break;
                        case nameof(dataModel.Image2Path):
                            if (dataModel.Image2Path != null)
                            {
                                oldPath =
                                Path.Combine(_webHostEnvironment.WebRootPath, dataModel.Image2Path.TrimStart('/'));
                            }
                            dataModel.Image2Path = pathForModel;
                            break;
                        default:
                            throw new ArgumentException("Unknow property name", nameof(propertyName));
                    }

                    if(System.IO.File.Exists(oldPath)) 
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
            };

            changeProperty(_dataModel, nameof(_dataModel.ImageBackgroundPath), imageBg);
            changeProperty(_dataModel, nameof(_dataModel.Image1Path), image1);
            changeProperty(_dataModel, nameof(_dataModel.Image2Path), image2);

            _dataModel.SaveChanges();

            return RedirectToAction("Images");
        }
    }
}
