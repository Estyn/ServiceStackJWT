﻿using IdentityServer4.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSvrHost.UI.Logout
{
    public class LoggedOutViewModel
    {
        public LoggedOutViewModel()
        {
            SignOutIFrameUrls = Enumerable.Empty<string>();
        }

        public ClientReturnInfo ReturnInfo { get; set; }
        public string ClientName { get; set; }

        public IEnumerable<string> SignOutIFrameUrls { get; set; }
    }
}
