using System;
using System.Collections.Generic;
using System.Text;

namespace Momkn.Core.SearchModel
{
    public class UserSearchModel
    {
        public bool? IsActive { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
    }
}
