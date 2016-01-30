﻿using IdentityServer4.Core.Services;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSvrHost.UI.Login
{
    public class SignInResult : IActionResult
    {
        private readonly string _requestId;

        public SignInResult(string requestId)
        {
            _requestId = requestId;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var interaction = context.HttpContext.RequestServices.GetRequiredService<SignInInteraction>();
            await interaction.ProcessResponseAsync(_requestId, new IdentityServer4.Core.Models.SignInResponse());
        }
    }
}
