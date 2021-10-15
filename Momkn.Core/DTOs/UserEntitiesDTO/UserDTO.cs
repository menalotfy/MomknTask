﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momkn.Core.DTOs.UserEntitiesDTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public bool? IsActive { get; set; }
   

        public string FullName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    

        public string UserName { get; set; }



    }

   

  

}