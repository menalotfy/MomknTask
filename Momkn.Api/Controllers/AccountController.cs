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
        private readonly IUserRefreshTokenRepository _userRefreshTokens;

        public AccountController(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
        
            IApplicationUserTokenRepository ApplicationUserTokenRepository,
            IUserRefreshTokenRepository UserRefreshTokens
      
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
      
            _ApplicationUserTokenRepository = ApplicationUserTokenRepository;
            _userRefreshTokens = UserRefreshTokens;
      
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

            // generate refresh token to user and save it to db
            //var refreshToken = TokenGenerator.GenerateRefreshToken();
            //_userRefreshTokens.Insert(new UserRefreshToken() { UserId = user.Id, RefreshToken = refreshToken });

            loginToken.refreshToken = ""; //refreshToken;
            loginToken.accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            loginToken.expiresIn = token.ValidTo;
       
           
          
            loginToken.userData = new UserViewModel
            {
                username = user.UserName,
                email = user.Email,
                userRole = string.Join(",", roles.Select(a => a.ToLower())),
                uid = user.Id,
                displayName = user.UserName,
                phone = user.PhoneNumber,
                photoURL = user.ProfileImagePath,
            
            };

            return loginToken;
        }
        //////////////////////////////////////////////////////////
     
        [AllowAnonymous]
        [HttpPost]
        [Route("API/[controller]/[action]")]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var user = _userRefreshTokens.GetUser(request.refreshToken);
            if (user != null)
            {

                LoginToken LoginResponseDTO = await GenerateLoginToken(user);
               
                //delete old token
                _userRefreshTokens.DeleteUserToken(request.refreshToken);
                if (!string.IsNullOrWhiteSpace(request.FirebaseToken))
                {
                    ApplicationUserToken applicationUserToken = _ApplicationUserTokenRepository.GetAll().Where(x => x.IsDeleted != true && x.UserId == user.Id && x.FireBaseToken == request.FirebaseToken).FirstOrDefault();
                    if (applicationUserToken == null)
                    {
                        applicationUserToken = new ApplicationUserToken();
                        applicationUserToken.FireBaseToken = request.FirebaseToken;
                        applicationUserToken.UserId = user.Id;
                        applicationUserToken.CreatedDate = DateTime.Now;
                        _ApplicationUserTokenRepository.Add(applicationUserToken);
                    }
                }
                return Ok(new { data = LoginResponseDTO });
            }
            return Ok(new { StatusCode = "UserNotFound" });
        }


     
        [HttpGet]
        public async Task<ActionResult> DeleteUser(string userId, [FromHeader] string language = "en")
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser != null)
            {
                applicationUser.IsDeleted = true;
                await _userManager.UpdateAsync(applicationUser);
            }
            return Ok(ResponseHelper.Success(data: applicationUser));
        }
        
    

        [HttpGet]
        public ActionResult GetAllRoles([FromHeader] string language = "en")
        {
            if (!(_roleManager.Roles.Any(r => r.Name == "Doctor")))
            {
                _roleManager.CreateAsync(new IdentityRole("Doctor")).GetAwaiter().GetResult();
            }
            if (!(_roleManager.Roles.Any(r => r.Name == "Patient")))
            {
                _roleManager.CreateAsync(new IdentityRole("Patient")).GetAwaiter().GetResult();
            }

            List<IdentityRole> roles = _roleManager.Roles.ToList();
            return Ok(ResponseHelper.Success(data: roles));
        }
       

  

        

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
