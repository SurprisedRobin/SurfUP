using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Mvc;
using SurfUPWeb.Data;
using SurfUPWeb.Models.Domain;
using SurfUPWeb.Models;
using NuGet.Protocol;

namespace SurfUPWeb.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly MvcReservationDB mvcReservationDB;

        public ReservationsController(MvcReservationDB mvcReservationDB)
        {
            this.mvcReservationDB = mvcReservationDB;
        }
        //public async Task<IActionResult> Index()
        //{ 
        //    IEnumerable<Reservation> reservations = await mvcReservationDB.Reservations.ToListAsync();
        //    return View(reservations);
        //}

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        //Find list of reservations to display on the Table in the Reservation, Index view.
        [HttpGet]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString)
         {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";

            if (searchString == null)
            {
                var emptylist = Enumerable.Empty<Reservation>();
                return View(emptylist);
            }

            ViewBag.CurrentFilter = searchString;

            var reservation = from s in mvcReservationDB.Reservations
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                reservation = reservation.Where(s => s.CostumerName.Contains(searchString)
                                       || s.SurfboardID.Contains(searchString)
                                       || s.UserEmail.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    reservation = reservation.OrderByDescending(s => s.CostumerName);
                    break;
                case "Date":
                    reservation = reservation.OrderBy(s => s.SurfboardID);
                    break;
                case "date_desc":
                    reservation = reservation.OrderByDescending(s => s.SurfboardID);
                    break;
                case "Email":
                    reservation = reservation.OrderBy(s => s.SurfboardID);
                    break;
                case "email_desc":
                    reservation = reservation.OrderByDescending(s => s.SurfboardID);
                    break;
                default:  // Name ascending 
                    reservation = reservation.OrderBy(s => s.CostumerName);
                    break;
            }

            return View(reservation);
        }

        //Delete for the delete button on the Table of Reservations
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var reservations = await mvcReservationDB.Reservations.FindAsync(id);

            if (reservations != null)
            {
                mvcReservationDB.Reservations.Remove(reservations);
                await mvcReservationDB.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
