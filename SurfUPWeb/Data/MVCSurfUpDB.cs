using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SurfUPWeb.Models.Domain;
using System.Reflection;

namespace SurfUPWeb.Data
{
    public class MVCSurfUpDB : DbContext
    {
        public MVCSurfUpDB(DbContextOptions<MVCSurfUpDB> options, ILogger<MVCSurfUpDB> logger) : base(options)
        {
        }

        public DbSet<SurfBoards> SurfBoards { get; set; }
    }
}
