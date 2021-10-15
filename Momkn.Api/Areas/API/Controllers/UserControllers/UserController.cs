using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Momkn.API.Helpers;
using Momkn.Core.DTOs.UserEntitiesDTO;

using Momkn.Core.Identity;


namespace Momkn.API.Areas.API.Controllers.UserControllers
{
    [Route("API/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
    


        public UserController(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager,
            IConfiguration configuration
          
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserDTO model, [FromHeader] string language = "en")
        {
            ApplicationUser oldUserByEmail = await _userManager.FindByNameAsync(model.Email);
            ApplicationUser oldUserByUser = await _userManager.FindByNameAsync(model.UserName);
            if (oldUserByEmail != null&& oldUserByUser!=null)
            {
                return BadRequest(ResponseHelper.Fail(message: "User Exist"));

            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.Now,
           
                EmailConfirmed = true,
       

            };

            IdentityRole role = await _roleManager.FindByNameAsync(model.RoleName);


            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                model.Password = null;

                await _userManager.AddToRoleAsync(user, role.Name);
                user.RoleName = role.Name;
             

              
                
                return Ok(ResponseHelper.Success(data: user));

            }
            else
            {
                if (result.Errors.FirstOrDefault().Description.Contains("Passwords"))
                {
                    return BadRequest(ResponseHelper.Fail(message: "Password Error"));

                }
                if (result.Errors.FirstOrDefault().Description.Contains("Email"))
                {
                    return BadRequest(ResponseHelper.Fail(message: "Email Error"));

                }
                return BadRequest(ResponseHelper.Fail(message: result.Errors.FirstOrDefault().Description.ToString()));
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetUserById(string userId, [FromHeader] string language = "en")
        {
            try
            {

                ApplicationUser applicationUser = await _userManager.FindByIdAsync(userId);


                var roles = await _userManager.GetRolesAsync(applicationUser);
                applicationUser.RoleName = roles.ElementAt(0);
                return Ok(ResponseHelper.Success(data: applicationUser));
            }
            catch(Exception ex)
            {
                return BadRequest(ResponseHelper.Success(message: ex.ToString()));

            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser([FromBody] UserDTO model, [FromHeader] string language = "en")
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(model.Id);
            if (applicationUser == null)
            {
                return BadRequest(ResponseHelper.Fail(message: "User Not Exist"));
            }
            if (applicationUser.Email != model.Email)
            {
                ApplicationUser currentUser = await _userManager.FindByEmailAsync(model.Email);
                if (currentUser != null)
                {
                    return BadRequest(ResponseHelper.Fail(message: "This mail is already exist"));
                }
                
            }
            applicationUser.UserName = model.UserName;
            applicationUser.FullName = model.FullName;
            applicationUser.PhoneNumber = model.PhoneNumber;
        

            await _userManager.UpdateAsync(applicationUser);

            await UpdateUserRole(applicationUser, model.RoleName);

        


            applicationUser.RoleName = model.RoleName;
            return Ok(ResponseHelper.Success(data: applicationUser));
        }

        [HttpPost]
        public async Task UpdateUserRole(ApplicationUser user,string newRole)
        {

            var roles = await _userManager.GetRolesAsync(user);
            if(roles!=null&&!roles.Contains(newRole))
            {
               await _userManager.AddToRolesAsync(user, new List<string> { newRole });
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string userId, [FromHeader] string language = "en")
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null)
            {
                return BadRequest(ResponseHelper.Fail(message: "User Not Exist"));
            }
            applicationUser.IsDeleted = true;
            await _userManager.UpdateAsync(applicationUser);
            return Ok(ResponseHelper.Success( data: applicationUser));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers([FromHeader] string language = "en")
        {
            List<ApplicationUser> applicationUsers = _userManager.Users.Where(x => x.IsDeleted != true).ToList();
         

            return Ok(ResponseHelper.Success(data: applicationUsers));
        }

       

      
    }
}
