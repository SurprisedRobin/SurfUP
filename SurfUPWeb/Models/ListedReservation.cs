namespace SurfUPWeb.Models
{
    //List of information needed for our "Make sure board isnt reservated on the same date" funktion
    public class ListedReservation
    {
        public string SurfboardID { get; set; }
        public DateTime ReservationDate { get; set; }

    }
}
