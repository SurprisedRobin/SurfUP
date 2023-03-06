using Microsoft.AspNetCore.Mvc;
using SurfUPWeb.Data;
using SurfUPWeb.Models;
using SurfUPWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace SurfUPWeb.Controllers
{
    public class SurfBoardsController : Controller
    {
        private readonly MVCSurfUpDB mvcSurfBoardDB;

        public SurfBoardsController(MVCSurfUpDB mvcSurfBoardDB)
        {
            this.mvcSurfBoardDB = mvcSurfBoardDB;
        }
        //Our string converter because there was some difficulties with the Inputtype "Number"(Deleting ./, in the numbers inputted)
        double stringconverter(string text)
        {
            double.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double returnvalue);
            return returnvalue;
        }

        //Get a list of the different ombjects and put them into a list and display it on the table.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var surfBoards = await mvcSurfBoardDB.SurfBoards.ToListAsync();
            return View(surfBoards);
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
           
            var SurfBoard = new SurfBoards()
            {
                //Reason we use string converter is because there was some problems with the 
                //Website converting it to a decimal when it returned the value of an input type "Number"
                //This is done on Price,Thickness, volume, and width since they can be Decimals
                ID = Guid.NewGuid(),
                Name = AddSurfBoadsRequest.Name,
                Price = stringconverter(AddSurfBoadsRequest.Price),
                Type = AddSurfBoadsRequest.Type,
				Width = stringconverter(AddSurfBoadsRequest.Width),
                LengthFeet = AddSurfBoadsRequest.LengthFeet,
                LengthInch= AddSurfBoadsRequest.LengthInch,
                Thicc = stringconverter(AddSurfBoadsRequest.Thicc),
                Volume = stringconverter(AddSurfBoadsRequest.Volume),
                Exstra = "" + AddSurfBoadsRequest.Exstra,
            };
            //This is to make sure the price gets converted to a legit number.
            //Because if the price string cant be converted then it becomes 0 so the item dosnt get uploaded with a price of 0
            if (SurfBoard.Price > 0 && SurfBoard.Width > 0 && SurfBoard.Thicc > 0 && SurfBoard.Volume > 0)
            {
                await mvcSurfBoardDB.SurfBoards.AddAsync(SurfBoard);
                await mvcSurfBoardDB.SaveChangesAsync();
                return RedirectToAction("Add");
            }

            //Test to for making sure get get the right input.
            return RedirectToAction("Index");
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


            if(surfBoard != null)
            {
                var viewModel = new UpdateSurfBoardViewModel()
                {
                    ID = surfBoard.ID,
                    Name = surfBoard.Name,
                    Price = surfBoard.Price.ToString(),
                    Type = surfBoard.Type,
					Width = surfBoard.Width.ToString(),
                    LengthFeet = surfBoard.LengthFeet,
                    LengthInch= surfBoard.LengthInch,
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
                surfBoard.Price= stringconverter(model.Price);
                surfBoard.Type= model.Type;
                surfBoard.Width = stringconverter(model.Width);
                surfBoard.LengthFeet= model.LengthFeet; 
                surfBoard.LengthInch= model.LengthInch;
                surfBoard.Thicc= stringconverter(model.Thicc);
                surfBoard.Volume= stringconverter(model.Volume);

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
