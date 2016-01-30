﻿using IdentityServer4.Core;
using IdentityServer4.Core.Services;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSvrHost.UI.Error
{
    public class ErrorController : Controller
    {
        private readonly ErrorInteraction _errorInteraction;

        public ErrorController(ErrorInteraction errorInteraction)
        {
            _errorInteraction = errorInteraction;
        }

        [Route(Constants.RoutePaths.Error, Name ="Error")]
        public async Task<IActionResult> Index(string id)
        {
            var vm = new ErrorViewModel();

            if (id != null)
            {
                var message = await _errorInteraction.GetRequestAsync(id);
                if (message != null)
                {
                    vm.Error = message;
                }
            }

            return View("Error", vm);
        }
    }
}
