using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Policy;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace LawetaWeb.Models
{
    public class DataModel
    {
        private string _filePath;
        private IWebHostEnvironment _webHostEnviroment;
        public string? CompanyName { get; set; }
        public string? Title { get; set; }
        public string ImageBackgroundPath { get; set; }
        public string Image1Path { get; set; }
        public string Image2Path { get; set; }
        public Tile[] Tiles { get; set; }
        public string Price { get; set; }
        public Dictionary<int,string> PhoneNumbers { get; set; }
        public List<TransportInfo> Transports { get; set; }
        public string FooterText { get; set; }

        public void Initialize(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnviroment = webHostEnvironment;
            _filePath = Path.Combine(_webHostEnviroment.WebRootPath, "data", "webcontent.json");

            OverrideProperties(JsonFileUtils.Read<DataModel>(_filePath));
        }
        public void SaveChanges() 
        {
            JsonFileUtils.Write(this, _filePath);
        }
        public void OverrideProperties(DataModel data)
        {
            if (data != null && data.GetType() == typeof(DataModel))
            {
                Type objType = typeof(DataModel);
                var properties = objType.GetProperties();
                foreach (var property in properties) 
                { 
                    var propVal = property.GetValue(data);
                    if(propVal != null)
                        property.SetValue(this, propVal);
                }
            }            
        }
    }
}
