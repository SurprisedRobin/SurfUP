using Microsoft.AspNetCore.Mvc.Rendering;
using SurfUPWeb.Models.Domain;

namespace SurfUPWeb.Models
{
    public class FilterSurfBoardViewModel
    {
        public List<SurfBoards>? SurfBoards { get; set; }
        public SelectList? Type { get; set; }

        public string? SurfboardFilter { get; set; }
        public string? SearchString { get; set; }
    }
}
