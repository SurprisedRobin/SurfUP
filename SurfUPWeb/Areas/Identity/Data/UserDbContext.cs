using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SurfUPWeb.Areas.Identity.Data;
using SurfUPWeb.Data;
using SurfUPWeb.Helpers;
using SurfUPWeb.Models.Domain;
using System.Reflection.Emit;

namespace SurfUPWeb.Areas.Identity.Data;

public class UserDbContext : IdentityDbContext<ApplicationUsers>
{
    public UserDbContext(DbContextOptions<UserDbContext> options, ILogger<UserDbContext> logger) : base(options)
    {
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUsers>
{
    public void Configure(EntityTypeBuilder<ApplicationUsers> builder)
    {
        

    }
}
