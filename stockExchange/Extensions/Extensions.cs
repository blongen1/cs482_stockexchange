using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace stockExchange.Extensions
{
    public static class Extensions
    {
        public static string GetCash(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("Cash");
            return (claim != null) ? claim.Value : string.Empty;
        }

    }
}