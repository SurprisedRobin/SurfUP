using Microsoft.AspNetCore.Mvc;
using SurfUPWeb.Data;
using SurfUPWeb.Models;
using SurfUPWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using SurfUPWeb.Interfaces;
using X.PagedList;
using Microsoft.Data.SqlClient;
using X.PagedList.Mvc;
using System.Data;
using System.Diagnostics.Metrics;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CloudinaryDotNet.Actions;
using System.Drawing.Printing;
using SurfUPWeb.Areas.Identity.Data;

namespace SurfUPWeb.Controllers
{
    public class SurfBoardsController : Controller
    {
        private readonly MVCSurfUpDB mvcSurfBoardDB;
        private readonly IPhotoService photoService;
        private readonly MvcReservationDB mvcReservationDB;
        private readonly UserDbContext mvcUserDbContext;

        public SurfBoardsController(MVCSurfUpDB mvcSurfBoardDB, IPhotoService photoService, MvcReservationDB mvcReservationDB)
        {
            this.mvcSurfBoardDB = mvcSurfBoardDB;
            this.photoService = photoService;
            this.mvcReservationDB = mvcReservationDB;
        }
        //Our string converter because there was some difficulties with the Inputtype "Number"(Deleting ./, in the numbers inputted)
        double Stringconverter(string text)
        {
            double.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double returnvalue);
            return returnvalue;
        }


        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        //Get a list of the different objects and put them into a list and display it on the table.
        [HttpGet]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var surfBoards = from s in mvcSurfBoardDB.SurfBoards
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                surfBoards = surfBoards.Where(s => s.Name.Contains(searchString)
                                       || s.ID.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    surfBoards = surfBoards.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    surfBoards = surfBoards.OrderBy(s => s.ID);
                    break;
                case "date_desc":
                    surfBoards = surfBoards.OrderByDescending(s => s.ID);
                    break;
                default:  // Name ascending 
                    surfBoards = surfBoards.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(surfBoards.ToPagedList(pageNumber, pageSize));
        }

        //Display the Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// This Is Command where we create a new surfboard to save to the database
        /// This comes from getting the AddSurfBoadsRequest that it generates from the infor 
        /// the user puts into the site under the "Add" funktion
        /// </summary>
        /// <param name="AddSurfBoadsRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddSurfBoardViewModel AddSurfBoadsRequest)
        {
            var result = await photoService.AddPhotoAsync(AddSurfBoadsRequest.Image);

            var SurfBoard = new SurfBoards()
            {
                //Reason we use string converter is because there was some problems with the 
                //Website converting it to a decimal when it returned the value of an input type "Number"
                //This is done on Price,Thickness, volume, and width since they can be Decimals
                ID = Guid.NewGuid(),
                Name = AddSurfBoadsRequest.Name,
                Price = Stringconverter(AddSurfBoadsRequest.Price),
                Type = AddSurfBoadsRequest.Type,
				Width = Stringconverter(AddSurfBoadsRequest.Width),
                LengthFeet = AddSurfBoadsRequest.LengthFeet,
                LengthInch= AddSurfBoadsRequest.LengthInch,
                Thicc = Stringconverter(AddSurfBoadsRequest.Thicc),
                Volume = Stringconverter(AddSurfBoadsRequest.Volume),
                Exstra = "" + AddSurfBoadsRequest.Exstra,
                Image = result.Url.ToString()
            };
            
            //Vores ifstatement er i vores HTML der tvinger brugeren til at skrive et valid tal (Add.Cshtml i input statements)
                await mvcSurfBoardDB.SurfBoards.AddAsync(SurfBoard);
                await mvcSurfBoardDB.SaveChangesAsync();

            return RedirectToAction("Add");
        }

        /// <summary>
        /// This is the function to get the individual item you press the "View" button on on the 
        /// screen that each item has when you see the overview and gives it to the view 
        /// where we can see the different values already beeing in the different spots.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult View(Guid id)
        {
            var surfBoard = mvcSurfBoardDB.SurfBoards.FirstOrDefault(x => x.ID == id);

            if (surfBoard != null)
            {
                var viewModel = new UpdateSurfBoardViewModel()
                {
                    ID = surfBoard.ID,
                    Name = surfBoard.Name,
                    Price = surfBoard.Price.ToString(),
                    Type = surfBoard.Type,
                    Width = surfBoard.Width.ToString(),
                    LengthFeet = surfBoard.LengthFeet.ToString(),
                    LengthInch = surfBoard.LengthInch.ToString(),
                    Thicc = surfBoard.Thicc.ToString(),
                    Volume = surfBoard.Volume.ToString(),
                    Url = surfBoard.Image,
                    Exstra = surfBoard.Exstra,
                };

                return View("View", viewModel);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// This is what makes the changes u make to the items update to the database. and saves the changes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update(UpdateSurfBoardViewModel model)
        {

            var surfBoard = await mvcSurfBoardDB.SurfBoards.FindAsync(model.ID);

            if (surfBoard != null)
            {
                surfBoard.Name = model.Name;
                surfBoard.Price = Stringconverter(model.Price);
                surfBoard.Type = model.Type;
                surfBoard.Width = Stringconverter(model.Width);
                surfBoard.LengthFeet = Stringconverter(model.LengthFeet);
                surfBoard.LengthInch = Stringconverter(model.LengthInch);
                surfBoard.Thicc = Stringconverter(model.Thicc);
                surfBoard.Volume = Stringconverter(model.Volume);
                surfBoard.Exstra = model.Exstra+"";
                if (model.Image == null)
                {
                    //TempData["SuccessMessage"] = "Add a photo before updating";
                    //return View(model.ID);

                    await mvcSurfBoardDB.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                var photoResult = await photoService.AddPhotoAsync(model.Image);
                surfBoard.Image = photoResult.Url.ToString();

                await mvcSurfBoardDB.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete is the function that deletes the individual item from the database and saves 
        /// the changes to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateSurfBoardViewModel model)
        {
            var surfBoard = await mvcSurfBoardDB.SurfBoards.FindAsync(model.ID);
            
            if (surfBoard != null)
            {
                mvcSurfBoardDB.SurfBoards.Remove(surfBoard);
                await mvcSurfBoardDB.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateSurfBoardViewModel model)
        {
            bool success = true;

            var requestReservation = new Reservation()
            {
                ReservationId = Guid.NewGuid(),
                ReservationDate = model.CostumerReservation.ReservationDate,
                CostumerName = model.CostumerReservation.CostumerName,
                SurfboardID = model.CostumerReservation.SurfboardID.ToString(),

            };


            List<ListedReservation> reservationList = new List<ListedReservation>();
            reservationList = (List<ListedReservation>)GetList(requestReservation);

            foreach(ListedReservation item in reservationList)
            {
                 if(requestReservation.ReservationDate == item.ReservationDate)
                 {
                    success = false;
                 }
            }

            if (success == true)
            {
                await mvcReservationDB.Reservations.AddAsync(requestReservation);
                await mvcReservationDB.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reservation Made successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["SuccessMessage"] = "Reservation Could not be made Please try another date";
                return View(model.ID);
            }
        }

        public IEnumerable<ListedReservation> GetList(Reservation wishedReservation)
        {

            string query = "SELECT * FROM Reservations WHERE SurfboardID = @id";

            DataTable dataTable = new DataTable();

            string connStr = "Server=(localdb)\\mssqllocaldb;Database=SurfUpReservations;Trusted_Connection=True;MultipleActiveResultSets=true";

            string searchID = wishedReservation.SurfboardID;

            List<ListedReservation> reservation = new List<ListedReservation>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = wishedReservation.SurfboardID;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListedReservation addedReservation = new ListedReservation(); 
                    addedReservation.ReservationDate = (DateTime)reader["ReservationDate"];
                    addedReservation.SurfboardID = (string)reader["SurfboardID"];
                    reservation.Add(addedReservation);
                }

            }

            return reservation;
        }

    }
}
