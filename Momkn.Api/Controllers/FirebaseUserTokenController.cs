// This project is developed by I-Valley software development

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Momkn.Core.Identity;
using Momkn.Core.Interfaces.UserInterface;

namespace Momkn.API.Controllers
{
    public class FirebaseUserTokenController : ControllerBase
    {
        private readonly IApplicationUserTokenRepository _firebaseUserTokenRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public FirebaseUserTokenController(IApplicationUserTokenRepository firebaseUserTokenRepository, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _firebaseUserTokenRepository = firebaseUserTokenRepository;
            _userManager = userManager;


        }

        [Route("AddFCMToken")]
        [HttpPost]
        public void AddFCMToken(string FCMToken)
        {
            ApplicationUser applicationUser = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            _firebaseUserTokenRepository.AddFirBaseToken(FCMToken, applicationUser.Id);
        }

        [Route("DeleteFCMToken")]
        [HttpDelete]
        public void DeleteFCMToken(string FCMToken)
        {
            _firebaseUserTokenRepository.DeleteFirebaseToken(FCMToken);
        }
    }
}
