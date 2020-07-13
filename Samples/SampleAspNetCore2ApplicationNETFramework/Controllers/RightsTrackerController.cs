using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAspNetCore2ApplicationNETFramework.Data;
using Sustainsys.Saml2;

namespace SampleAspNetCore2ApplicationNetFramework.Controllers
{
    // RT: this is here to demonstrate how Idp-Init flow might work
    // e.g. 
    // SAML2 options.SPOptions.ReturnUrl = new Uri("/RightsTracker/Hello", UriKind.Relative);
    // makes Idp-Init go here
    // BUT NOT WORKING WELL ENOUGH...

    public class RightsTrackerController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public RightsTrackerController(SignInManager<ApplicationUser> signInManager,
            ILogger<RightsTrackerController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        //[BindProperty]
        //public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }



        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Hello()
        {
            var req = HttpContext.Request;
            var user = HttpContext.User;

            // put a breakpoint here to examine those

            // Hmmm. We're not seeing any claims stuff here
            // There's no forms data (etc) ... nothing :((
            //

            return new OkResult();
        }

        public async Task<IActionResult> HelloV2(string returnUrl = null, string remoteError = null)
        {
            // where I tried copying the code from ExternalLogin ...  did not help :((

            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // YES - IT IS NULL :((
                return RedirectToPage("./Login");
            }

            _signInManager.ClaimsFactory = new Saml2ClaimsFactory(_signInManager.ClaimsFactory, info);

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                // RT: TODO - make the {Name} work properly (currently null)
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // RT: - here is where we would catch a new user (not in Assetry) and do all the stuff we want to do to them:
                // (1) Create them with minimal permissions,
                // (2) route them to a "Welcome. Please ask you administrator to grant you permissions",
                // (3) email their administrator and Bcc Mike :) 
                //
                // In their example this is what they do:
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    //Input = new InputModel
                    //{
                    //    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    //};
                }

                //return Page();
                return new OkResult();
            }
        }



        class Saml2ClaimsFactory : IUserClaimsPrincipalFactory<ApplicationUser>
        {
            IUserClaimsPrincipalFactory<ApplicationUser> _inner;
            ExternalLoginInfo _externalLoginInfo;

            public Saml2ClaimsFactory(
                IUserClaimsPrincipalFactory<ApplicationUser> inner,
                ExternalLoginInfo externalLoginInfo)
            {
                _inner = inner;
                _externalLoginInfo = externalLoginInfo;
            }

            public async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
            {
                var principal = await _inner.CreateAsync(user);

                var logoutInfo = _externalLoginInfo.Principal.FindFirst(Saml2ClaimTypes.LogoutNameIdentifier);
                var sessionIndex = _externalLoginInfo.Principal.FindFirst(Saml2ClaimTypes.SessionIndex);

                var identity = principal.Identities.Single();
                identity.AddClaim(logoutInfo);
                identity.AddClaim(sessionIndex);

                // RT:
                // Here we can add all the claims info from the SAML2 post into the User claims
                // e.g.
                foreach (var role in _externalLoginInfo.Principal.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                {
                    identity.AddClaim(role);
                }
                // e.g. 2 - hard coded
                identity.AddClaim(new Claim("Hello", "World"));
                // e.g. 3 - (Note if "TenantId" is missing this will throw an exception)
                identity.AddClaim(_externalLoginInfo.Principal.FindFirst("TenantId"));

                return principal;
            }
        }
    }
}
