using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Momkn.Core.Enitities.MainEntity
{
    public class ApplicationUserToken : BaseEntity
    {
        public int ApplicationUserTokenID { get; set; }
        public string UserId { get; set; }
        public string FireBaseToken { get; set; }

    }
}
