using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Identity;

namespace Momkn.Infrastructure.Repositories
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserResolverService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetUser()
        {
            return await _userManager.FindByEmailAsync(_context.HttpContext.User?.Identity?.Name);
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            return  _userManager.Users;
        }
    }
}
