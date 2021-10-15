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
    public class ApplicationUserTokenRepository : Repository<ApplicationUserToken>, IApplicationUserTokenRepository
    {
        private readonly MomknDbContext _dbContext;
        public ApplicationUserTokenRepository(MomknDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddFirBaseToken(string FirBaseToken, string UserId )
        {

            try
            {
                if (_dbContext.ApplicationUserTokens.Where(x => x.FireBaseToken == FirBaseToken && x.UserId == UserId).FirstOrDefault() == null)
                {
                    ApplicationUserToken applicationUserToken = new ApplicationUserToken();
                    applicationUserToken.FireBaseToken = FirBaseToken;
                    applicationUserToken.UserId = UserId;
                    applicationUserToken.IsDeleted = false;
                    applicationUserToken.CreatedDate = DateTime.Now;
                    _dbContext.ApplicationUserTokens.Add(applicationUserToken);
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                    return false;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public List<string> GetFirebasetokensByUserId(string userId)
        {
            List<string> UserTokens = new List<string>();
            try
            {
                UserTokens = _dbContext.ApplicationUserTokens.Where(x => x.UserId == userId && (x.IsDeleted == null || x.IsDeleted == false)).ToList().Select(x => x.FireBaseToken).ToList();
                return UserTokens;
            }

            catch (Exception ex)
            {
                return UserTokens;
            }
        }
        public List<ApplicationUserToken> GetTokensToDeleteFirBaseToken(string fireBaseToken)
        {

            List<ApplicationUserToken> ApplicationUserTokens = _dbContext.ApplicationUserTokens.Where(x => x.FireBaseToken == fireBaseToken && x.IsDeleted != true).ToList();
            return ApplicationUserTokens;

        }
        public List<string> GetAllTokens()
        {

            List<string> ApplicationTokens = _dbContext.ApplicationUserTokens.Where(x => x.IsDeleted != true).Select(x => x.FireBaseToken).ToList();
            return ApplicationTokens;

        }

        public ApplicationUser GetUserByMail(string email)
        {
            ApplicationUser User = _dbContext.Users.Where(x => x.Email == email && x.UserName == email).FirstOrDefault();
            return User;
        }

        public void DeleteFirebaseToken(string fireBaseToken)
        {
            List<ApplicationUserToken> ApplicationUserTokens = _dbContext.ApplicationUserTokens.Where(x => x.FireBaseToken == fireBaseToken && x.IsDeleted != true).ToList();
            foreach(var item in ApplicationUserTokens)
            {
                Delete(item);
            }
        }
    }
}
