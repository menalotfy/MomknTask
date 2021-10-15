using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Identity;

namespace Momkn.Core.Enitities.MainEntity
{
    public class UserRefreshToken : BaseEntity
    {
        public int UserRefreshTokenId { get; set; }
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public ApplicationUser User { get; set; }
    }
}
