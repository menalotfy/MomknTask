using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Enitities.MainEntity;

using Momkn.Core.Identity;
using Momkn.Core.Interfaces.UserInterface;
using Momkn.Infrastructure.Data;

namespace Momkn.Infrastructure.Repositories.UserRepositories
{
    public class UserRefreshTokenRepository : Repository<UserRefreshToken>, IUserRefreshTokenRepository
    {
        private readonly MomknDbContext _dbContext;
        public UserRefreshTokenRepository(MomknDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public ApplicationUser GetUser(string refreshToken)
        {
            var query = from a in _dbContext.UserRefreshTokens
                        where a.RefreshToken == refreshToken
                        select a.User;
            return query.FirstOrDefault();
        }

        public void DeleteUserToken(string refreshToken)
        {
            UserRefreshToken token = _dbContext.UserRefreshTokens.Where(a => a.RefreshToken == refreshToken).FirstOrDefault();
            if (token != null)
                _dbContext.UserRefreshTokens.Remove(token);
        }
    }
}
