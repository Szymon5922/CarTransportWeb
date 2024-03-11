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
        public string ImageBackgroundPath { get; set; } = "";
        public string Image1Path { get; set; } = "";
        public string Image2Path { get; set; } = "";
        public Tile[] Tiles { get; set; }
        public string Price { get; set; }
        public Dictionary<int,string> PhoneNumbers { get; set; }
        public List<TransportInfo> Transports { get; set; }
        public string FooterText { get; set; }
        public DataModel() 
        {
            Tiles = new Tile[5];
            PhoneNumbers = new();
            Transports = new();
        }
        public void Initialize(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnviroment = webHostEnvironment;
            _filePath = Path.Combine(_webHostEnviroment.WebRootPath, "data", "webcontent.txt");
            
            string jsonData = File.ReadAllText(_filePath);

            OverrideAllProperties(JsonConvert.DeserializeObject<DataModel>(jsonData));
        }
        public void SaveChanges(DataModel data)
        {
            OverrideMainProperties(data);
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(_filePath, json);
        }
        public void SaveChanges() 
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(_filePath, json);
        }
        public void OverrideAllProperties(DataModel data)
        {
            if (data != null)
            {
                CompanyName = data.CompanyName;
                Title = data.Title;
                Price = data.Price;
                FooterText = data.FooterText;
                Image1Path = data.Image1Path;
                Image2Path = data.Image2Path;
                ImageBackgroundPath = data.ImageBackgroundPath;
                if(data.PhoneNumbers != null && data.PhoneNumbers.Count>0)
                {
                    PhoneNumbers = data.PhoneNumbers;
                }
                if (data.Tiles != null)
                {
                    for (int i = 0; i < data.Tiles.Length; i++)
                    {
                        Tiles[i] = data.Tiles[i];
                    }
                }
                if(Transports != null && data.Transports.Count > 0)
                {
                    foreach(TransportInfo transport in data.Transports)
                    {
                        Transports.Add(transport);
                    }
                }
            }            
        }
        public void OverrideMainProperties(DataModel data)
        {
            if (data != null)
            {
                CompanyName = data.CompanyName;
                Title = data.Title;
                Price = data.Price;
                FooterText = data.FooterText;
                if (data.PhoneNumbers != null && data.PhoneNumbers.Count > 0)
                {
                    PhoneNumbers = data.PhoneNumbers;
                }
                if (data.Tiles != null)
                {
                    for (int i = 0; i < data.Tiles.Length; i++)
                    {
                        Tiles[i] = data.Tiles[i];
                    }
                }                
            }
        }
    }
}
