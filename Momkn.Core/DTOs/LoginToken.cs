using System;
using System.Collections.Generic;
using System.Text;

namespace Momkn.Core.DTOs
{
    public class LoginToken
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime expiresIn { get; set; }
    }
}
