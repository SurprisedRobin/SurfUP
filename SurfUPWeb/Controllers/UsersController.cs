using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SurfUPWeb.Areas.Identity.Data;
using SurfUPWeb.Data;
using SurfUPWeb.Migrations.UserDb;
using SurfUPWeb.Models;
using SurfUPWeb.Models.Domain;
using System.Data;

namespace SurfUPWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserDbContext MvcUserDB;
        private readonly MvcReservationDB mvcReservationDB;

        public UsersController(UserDbContext mvcUserDB, MvcReservationDB mvcReservationDB)
        {
            this.MvcUserDB = mvcUserDB;
            this.mvcReservationDB = mvcReservationDB;
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        //Get the list of users too display in the table on the view.
        [HttpGet]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString == null)
            {
                var emptylist = Enumerable.Empty<ApplicationUsers>();
                return View(emptylist);
            }

            ViewBag.CurrentFilter = searchString;

            var users = from s in MvcUserDB.ApplicationUsers
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Email.Contains(searchString)
                                       || s.Id.Contains(searchString)
                                       );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.Id);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.Id);
                    break;
                default:  // Name ascending 
                    users = users.OrderBy(s => s.Email);
                    break;
            }

            return View(users);
        }

        //Delete user function for the admin
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await MvcUserDB.ApplicationUsers.FindAsync(id);

            if (user != null)
            {
                foreach(var reservation in GetUsersReservationList(id))
                {
                    Reservation? reservations = await mvcReservationDB.Reservations.FindAsync(reservation.ReservationId);
                    mvcReservationDB.Reservations.Remove(reservations);
                }
                await mvcReservationDB.SaveChangesAsync();

                MvcUserDB.ApplicationUsers.Remove(user);
                await MvcUserDB.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //Funktion to get reservations assosiated with the user so if the user is deleted so is the users Reservations
        public IEnumerable<Reservation> GetUsersReservationList(string userID)
        {

            string query = "SELECT * FROM Reservations WHERE UserID = @id";

            DataTable dataTable = new DataTable();

            string connStr = "Server=(localdb)\\mssqllocaldb;Database=SurfUpReservations;Trusted_Connection=True;MultipleActiveResultSets=true";

            List<Reservation> reservation = new List<Reservation>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = userID;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Reservation addedReservation = new Reservation();
                    addedReservation.ReservationDate = (DateTime)reader["ReservationDate"];
                    addedReservation.ReservationId = (Guid)reader["ReservationId"];
                    addedReservation.SurfboardID = (string)reader["SurfboardID"];
                    reservation.Add(addedReservation);
                }

            }

            return reservation;
        }

        [HttpGet]
        public async Task<IActionResult> View(ApplicationUsers User)
        {
            var viewUser = await MvcUserDB.ApplicationUsers.FindAsync(User.Id);
            return View("View", viewUser);
        }

    }
}
