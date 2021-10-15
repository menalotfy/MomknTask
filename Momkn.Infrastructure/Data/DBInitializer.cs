
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Momkn.Core.Enitities.MainEntity;
using Momkn.Core.Identity;
using Momkn.Core.Interfaces;

namespace Momkn.Infrastructure.Data
{
   public class DBInitializer : IDBInitializer
    {
        private readonly MomknDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(MomknDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    ///  _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
            }
            if (!_db.Roles.Any(r => r.Name == "Admin"))
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();




            Seed(_db);
        }
           
        public void Seed(MomknDbContext context)
        {
            // Seed initial data

            if (!context.Steps.Any())
            {
                context.AddRange(
                    new Step { Name = "Step", Number= 1, CreatedDate = DateTime.Now, IsDeleted = false },
                    new Step { Name = "Step", Number= 2, CreatedDate = DateTime.Now, IsDeleted = false },
                    new Step { Name = "Step", Number= 3, CreatedDate = DateTime.Now, IsDeleted = false }
                    );
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.AddRange(
                    new ApplicationUser { FullName = "admin",Email= "admin@gmail.com", PasswordHash = "AQAAAAEAACcQAAAAEFCYfcpF2yflTphJrmOgl6/Xz8XaFRkOOme1CMx9YHKE9N2u4TUzpcPLr8doAy3HqQ==",SecurityStamp= "fd3e9482-dd26-4e50-9ac7-b0ec45378c85", UserName= "admin@gmail.com", CreatedDate = DateTime.Now, IsDeleted = false }
              
                    );
                context.SaveChanges();
            }



        }
    }
}
