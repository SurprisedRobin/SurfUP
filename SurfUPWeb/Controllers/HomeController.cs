using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurfUPWeb.Areas.Identity.Data;
using SurfUPWeb.Data;
using SurfUPWeb.Interfaces;
using SurfUPWeb.Models;
using System.Diagnostics;
using X.PagedList;
using X.PagedList.Mvc;

namespace SurfUPWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly MVCSurfUpDB mvcSurfBoardDB;
        private readonly IPhotoService photoService;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MVCSurfUpDB mvcSurfBoardDB, IPhotoService photoService)
        {
            _logger = logger;
            this.mvcSurfBoardDB = mvcSurfBoardDB;
            this.photoService = photoService;
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

        [Authorize(Roles = "admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}