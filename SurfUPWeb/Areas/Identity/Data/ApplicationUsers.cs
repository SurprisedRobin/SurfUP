using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//Application users are our users for the identity.
namespace SurfUPWeb.Areas.Identity.Data
{
    public class ApplicationUsers : IdentityUser
    {
        
    }
}
