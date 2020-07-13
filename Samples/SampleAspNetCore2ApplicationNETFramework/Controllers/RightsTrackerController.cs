﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAspNetCore2ApplicationNETFramework.Data;

namespace SampleAspNetCore2ApplicationNetFramework.Controllers
{
    // RT: this is here to demonstrate how Idp-Init flow might work
    // e.g. 
    // SAML2 options.SPOptions.ReturnUrl = new Uri("/RightsTracker/Hello", UriKind.Relative);
    // makes Idp-Init go here

    public class RightsTrackerController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public RightsTrackerController(SignInManager<ApplicationUser> signInManager,
            ILogger<RightsTrackerController> logger)
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Hello()
        {
            var req = HttpContext.Request;
            var user = HttpContext.User;

            // put a breakpoint here to examine those
            return new OkResult();
        }
    }
}
