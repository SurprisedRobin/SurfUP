using Microsoft.EntityFrameworkCore;
using SurfUPWeb.Models.Domain;

namespace SurfUPWeb.Data
{
    public class MVCSurfUpDB : DbContext
    {
        public MVCSurfUpDB(DbContextOptions options) : base(options)
        {
        }


        public DbSet<SurfBoards> SurfBoards { get; set; }
    }
}
