using Microsoft.AspNetCore.Mvc;
using SurfUPWeb.Data;
using SurfUPWeb.Models;
using SurfUPWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using SurfUPWeb.Interfaces;
using CloudinaryDotNet;

namespace SurfUPWeb.Controllers
{
    public class SurfBoardsController : Controller
    {
        private readonly MVCSurfUpDB mvcSurfBoardDB;
        private readonly IPhotoService photoService;

        public SurfBoardsController(MVCSurfUpDB mvcSurfBoardDB, IPhotoService photoService)
        {
            this.mvcSurfBoardDB = mvcSurfBoardDB;
            this.photoService = photoService;
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
        public async Task<IActionResult> Index(string searchString)
        {
            var surfBoards = from s in mvcSurfBoardDB.SurfBoards
                             select s;
            
            //Search functionality for get name. Displays name
            if (!String.IsNullOrEmpty(searchString))
            {
                surfBoards = surfBoards.Where(s => s.Name!.Contains(searchString));
            }

			return View(await surfBoards.ToListAsync());

			/*try
            {
				var surfBoards = await mvcSurfBoardDB.SurfBoards.ToListAsync();

                if (surfBoards.Count == 0)
                {
                    TempData["InfoMessage"] = "Currently products not available in the Database";
                }
                else
                {
                    if (string.IsNullOrEmpty(searchValue))
                    {
                        TempData["InfoMessage"] = "Please provide the search value.";
                        return View(surfBoards);
                    }
                    else
                    {
                        if(searchBy.ToLower() == "ID")
                        {
                            var searchByID = surfBoards.Where(p => p.ID.ToString().Contains(searchValue.ToLower()));
                            return View(searchByID);
                        }
						else if (searchBy.ToLower() == "Name")
						{
							var searchByName = surfBoards.Where(p => p.Name.ToLower().Contains(searchValue.ToLower()));
							return View(searchByName);
                        }
						else if (searchBy.ToLower() == "Price")
						{
							var searchByPrice = surfBoards.Where(p => p.Price == double.Parse(searchValue));
							return View(searchByPrice);
						}

					}
                }
				return View(surfBoards);
			}
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }*/
		}

		//

		/*public List<SurfBoards> SurfBoards { get; set; }

		public void OnGet()
		{
			SurfBoards = new List<SurfBoards>();
		}

        [HttpGet]
		public async Task<IActionResult> OnPostAsync()
		{
			var searchString = Request.Form["searchString"];

			var SurfBoards = await mvcSurfBoardDB.SurfBoards.Where(x => x.Name.Contains(searchString)).ToListAsync();

            return await Task.Run(() => View("Index", SurfBoards));
			var surfBoards = await mvcSurfBoardDB.SurfBoards.ToListAsync();
			return View(surfBoards);
		}*/


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
        public async Task<IActionResult> View(Guid id)
        {
            var surfBoard = await mvcSurfBoardDB.SurfBoards.FirstOrDefaultAsync(x => x.ID == id);

            if (surfBoard != null)
            {
                var viewModel = new UpdateSurfBoardViewModel()
                {
                    ID = surfBoard.ID,
                    Name = surfBoard.Name,
                    Price = surfBoard.Price.ToString(),
                    Type = surfBoard.Type,
                    Width = surfBoard.Width.ToString(),
                    LengthFeet = surfBoard.LengthFeet,
                    LengthInch = surfBoard.LengthInch,
                    Thicc = surfBoard.Thicc.ToString(),
                    Volume = surfBoard.Volume.ToString(),
                    Exstra = surfBoard.Exstra,
                };

                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// This is what makes the changes u make to the items update to the database. and saves the changes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> View(UpdateSurfBoardViewModel model)
        {
            var surfBoard = await mvcSurfBoardDB.SurfBoards.FindAsync(model.ID);

            if (surfBoard != null)
            {
                surfBoard.Name= model.Name;
                surfBoard.Price= Stringconverter(model.Price);
                surfBoard.Type= model.Type;
                surfBoard.Width = Stringconverter(model.Width);
                surfBoard.LengthFeet= model.LengthFeet; 
                surfBoard.LengthInch= model.LengthInch;
                surfBoard.Thicc= Stringconverter(model.Thicc);
                surfBoard.Volume= Stringconverter(model.Volume);

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
    }
}
