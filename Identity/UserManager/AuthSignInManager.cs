using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.UserManager
{
    public class AuthSignInManager<T> : SignInManager<AppUser> where T : class
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _context;

        public AuthSignInManager(
                                UserManager<AppUser> userManager, 
                                IHttpContextAccessor contextAccessor, 
                                IUserClaimsPrincipalFactory<AppUser> claimsFactory, 
                                IOptions<IdentityOptions> optionsAccessor, 
                                ILogger<SignInManager<AppUser>> logger, 
                                IAuthenticationSchemeProvider schemes, 
                                IUserConfirmation<AppUser> confirmation,
                                DataContext context
                                ) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool shouldLockout)
        {
            //var user = _userManager.FindByEmailAsync(userName).Result;
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return await Task.FromResult(SignInResult.LockedOut);
            }

            if (user != null && ((user.IsEnabled.HasValue && !user.IsEnabled.Value) || !user.IsEnabled.HasValue))
            {
                return await Task.FromResult(SignInResult.LockedOut);

            }

            return await base.PasswordSignInAsync(userName, password, rememberMe, shouldLockout);
        }
    }
}
