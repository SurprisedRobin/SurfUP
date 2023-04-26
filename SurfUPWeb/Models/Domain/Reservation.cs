using System.Globalization;
using PagedList;

namespace SurfUPWeb.Models.Domain
{
    public class Reservation
    {
        public Guid ReservationId { get; set; }
        public string SurfboardID { get; set; }
        public string CostumerName { get; set; }
        public DateTime ReservationDate { get; set; }

    }
}
