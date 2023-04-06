namespace SurfUPWeb.Models
{
	public class UpdateSurfBoardViewModel
	{
        public Guid ID { get; set; }
        public string Name { get; set; }
		public string Price { get; set; }
		public string Type { get; set; }
		public string Width { get; set; }
        public double LengthFeet { get; set; }
        public double LengthInch { get; set; }
        public string Thicc { get; set; }
		public string Volume { get; set; }
		public string Exstra { get; set; }
		public IFormFile Image { get; set; }
	}
}
