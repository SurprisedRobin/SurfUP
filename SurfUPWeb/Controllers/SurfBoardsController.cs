using Microsoft.AspNetCore.Mvc;
using SurfUPWeb.Data;
using SurfUPWeb.Models;
using SurfUPWeb.Models.Domain;

namespace SurfUPWeb.Controllers
{
    public class SurfBoardsController : Controller
    {
        private readonly MVCSurfUpDB mvcSurfBoardDB;

        public SurfBoardsController(MVCSurfUpDB mvcSurfBoardDB)
        {
            this.mvcSurfBoardDB = mvcSurfBoardDB;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSurfBoardViewModel AddSurfBoadsRequest)
        {
            var SurfBoard = new SurfBoards()
            {
                ID = Guid.NewGuid(),
                Name = AddSurfBoadsRequest.Name,
                Price = AddSurfBoadsRequest.Price,
                Type = AddSurfBoadsRequest.Type,
                Hight = AddSurfBoadsRequest.Hight,
                Length = AddSurfBoadsRequest.Length,
                Thicc = AddSurfBoadsRequest.Thicc,
                Volume = AddSurfBoadsRequest.Volume,
                Exstra = "" + AddSurfBoadsRequest.Exstra,
            };

            await mvcSurfBoardDB.AddAsync(SurfBoard);
            await mvcSurfBoardDB.SaveChangesAsync();
            return RedirectToAction("Add");
        }

        

    }
}
