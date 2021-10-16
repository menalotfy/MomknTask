

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Momkn.Core.Enitities.MainEntity;


namespace Momkn.Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
  
        public string FullName { get; set; }
        public string ProfileImagePath { get; set; }
        public bool? IsDeleted { get; set; }
  
        public DateTime? CreatedDate { get; set; }

       
       

    }
}
