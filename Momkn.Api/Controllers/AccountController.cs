using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Momkn.API.Helpers;
using Momkn.API.ViewModel;
using Momkn.Core.DTOs;

using Momkn.Core.Enitities.MainEntity;

using Momkn.Core.Identity;

using Momkn.Core.Interfaces.MainInterface;

using Momkn.Core.Interfaces.UserInterface;


namespace Momkn.API.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationUserTokenRepository _ApplicationUserTokenRepository;


        public AccountController(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
        
            IApplicationUserTokenRepository ApplicationUserTokenRepository
          
      
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
      
            _ApplicationUserTokenRepository = ApplicationUserTokenRepository;
         
      
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginViewModel request)
        {
           
                var user = await _userManager.FindByNameAsync(request.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))  
            {
              

                bool isExist = false;
                var oldToken = _ApplicationUserTokenRepository.GetFirebasetokensByUserId(user.Id);
                if (oldToken.Count > 0)
                {
                    foreach (string item in oldToken)
                    {
                        if (request.FCMToken == item)
                        {
                            isExist = true;
                        }
                    }
                }
 
                if (!isExist)
                {
                    _ApplicationUserTokenRepository.AddFirBaseToken(request.FCMToken, user.Id);
                }


                LoginToken logintoken = await GenerateLoginToken(user);

              
                return Ok(ResponseHelper.Success(data: logintoken));
            }
            return NotFound(ResponseHelper.Success(message: "Wrong Username or Password"));
        }

        //////////////////////////////////////////////////////////
       
        private async Task<LoginToken> GenerateLoginToken(ApplicationUser user)
        {
            LoginToken loginToken = new LoginToken();
            var roles = await _userManager.GetRolesAsync(user);
            var claim = new[] {
                 new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // random key for such a token 
                    new Claim(JwtRegisteredClaimNames.Typ, string.Join(",", roles.Select(a=> a.ToLower()))), // user roles
                    new Claim(JwtRegisteredClaimNames.Email, user.Email), // email
                    //new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddSeconds(3).ToString()) // expiry
                };
            var signinKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            int expiryInMinutes = Convert.ToInt32(_configuration["Tokens:ExpiryInMinutes"]);

            var token = new JwtSecurityToken(
              issuer: _configuration["Tokens:Issuer"],
              audience: _configuration["Tokens:Audience"],
              expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
              signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
              claims: claim
            );

         

            loginToken.refreshToken = ""; 
            loginToken.accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            loginToken.expiresIn = token.ValidTo;
       
           
          
        

            return loginToken;
        }
        //////////////////////////////////////////////////////////
     
     
    
       

  

        

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddFirBaseToken(string FCMToken, string userID)
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(userID);
            bool isExist = false;
            isExist = _ApplicationUserTokenRepository.GetFirebasetokensByUserId(applicationUser.Id).Any(x => x.Contains(FCMToken));

            if (!isExist)
            {
                _ApplicationUserTokenRepository.AddFirBaseToken(FCMToken, applicationUser.Id);
            }
            return Ok(ResponseHelper.Success());
        }

      
        [HttpGet]
        [AllowAnonymous]
        public void logout(string FCMToken)
        {
            if (FCMToken != null && FCMToken != "")
            {
                _ApplicationUserTokenRepository.DeleteFirebaseToken(FCMToken);
            }
        }

    }
}
