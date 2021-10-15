using System;
using System.Collections.Generic;
using System.Text;

namespace Momkn.Core.DTOs
{
    public class RefreshTokenRequest
    {
        public string refreshToken { get; set; }
        public string UserLanguage { get; set; }
        public string FirebaseToken { get; set; }
    }
}
