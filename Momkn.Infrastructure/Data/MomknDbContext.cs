

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


using Momkn.Core.Enitities.MainEntity;

using Momkn.Core.Identity;

namespace Momkn.Infrastructure.Data
{
    public class MomknDbContext : IdentityDbContext<ApplicationUser>
    {
        public MomknDbContext()
        {

        }

        public MomknDbContext(DbContextOptions<MomknDbContext> options) : base(options)
        {

        }

        public DbSet<Step> Steps { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
  
    

     

    }
}
