using SurfUPWeb.Areas.Identity.Data;
using SurfUPWeb.Models.Domain;

namespace SurfUPWeb.Models
{
	//Class used for our View surfboards View to make sure we have all information if they decide to make a reservation
	public class UpdateSurfBoardViewModel
	{
        public Guid ID { get; set; }
        public string Name { get; set; }
		public string Price { get; set; }
		public string Type { get; set; }
		public string Width { get; set; }
        public string LengthFeet { get; set; }
        public string LengthInch { get; set; }
        public string Thicc { get; set; }
		public string Volume { get; set; }
		public string Exstra { get; set; }
		public IFormFile Image { get; set; }
		public string Url { get; set; }
		public Reservation CostumerReservation { get; set; }
		public string UserEmail { get; set; }
	}
}
