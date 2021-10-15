using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Enitities.MainEntity;
using Momkn.Core.Identity;
using Momkn.Core.Interfaces.MainInterface;

namespace Momkn.Core.Interfaces.UserInterface
{
    public interface IApplicationUserTokenRepository : IRepository<ApplicationUserToken>
    {
        bool AddFirBaseToken(string fireBaseToken, string userId);
        List<string> GetFirebasetokensByUserId(string userId);
        List<ApplicationUserToken> GetTokensToDeleteFirBaseToken(string fireBaseToken);
        ApplicationUser GetUserByMail(string email);

        List<string> GetAllTokens();
        void DeleteFirebaseToken(string fireBaseToken);
    }
}
