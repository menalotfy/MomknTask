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
    public interface IUserRefreshTokenRepository : IRepository<UserRefreshToken>
    {
        ApplicationUser GetUser(string refreshToken);
        void DeleteUserToken(string refreshToken);
    }
}
