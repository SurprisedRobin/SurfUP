using System.Globalization;

namespace SurfUPWeb.Models.Domain
{
    public class SurfBoards
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        public double Hight { get; set; }
        public double Length { get; set; }
        public double Thicc { get; set; }
        public double Volume { get; set; }
        public string Exstra { get; set; }  
    }
}
