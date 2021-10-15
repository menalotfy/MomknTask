using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Identity;

namespace Momkn.Core.Enitities.MainEntity
{
    public class FirebaseUserToken : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public string FireBaseToken { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
