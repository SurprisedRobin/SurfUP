using System.Globalization;

namespace SurfUPWeb.Models.Domain
{
    public class SurfBoards
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Type { get; set; }
        public double? Width { get; set; }
        public double? LengthFeet { get; set; }
        public double? LengthInch { get; set; }
        public double? Thicc { get; set; }
        public double? Volume { get; set; }
        public string? Exstra { get; set; }  
        public string? Image { get; set; }

    }
}
