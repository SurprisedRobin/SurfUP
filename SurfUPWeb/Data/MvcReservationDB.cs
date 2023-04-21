using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SurfUPWeb.Models.Domain;
using System.Reflection;

namespace SurfUPWeb.Data
{
    public class MvcReservationDB : DbContext
    {
        public MvcReservationDB(DbContextOptions<MvcReservationDB> options, ILogger<MvcReservationDB> logger) : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
