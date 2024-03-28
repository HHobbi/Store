using Dto.Response.Payment;
using Microsoft.AspNetCore.Http;
using System;

namespace Endpoint.Site.Utilities
{

    public class CookiMangment
    {
        public bool Add(HttpContext context, string Key, string Value)
        {

            if (context.Request.Cookies.ContainsKey(Key))
            {
                return false;
            }
            else
            {
                context.Response.Cookies.Append(Key, Value, GetCookieOptions(context));
                return true;
            };

        }
        public string GetValue(HttpContext context, string Key)
        {
            string Value;
            if (context.Request.Cookies.TryGetValue(Key, out Value))
            {
                return Value;
            }
            else
            {
                return null;
            }

        }

        public Guid GetBrowserId(HttpContext context)
        {

            string browserId = GetValue(context, "BowserId");
            if (browserId == null)
            {
                string value = Guid.NewGuid().ToString();
                Add(context, "BowserId", value);
                browserId = value;
            }
            Guid guidBowser;
            Guid.TryParse(browserId, out guidBowser);
            return guidBowser;
        }

        public void Remove(HttpContext context, string Key)
        {
            if (context.Request.Cookies.ContainsKey(Key))
                context.Response.Cookies.Delete(Key);

        }

        public bool Contains(HttpContext context, string Key)
        {
            return context.Request.Cookies.ContainsKey(Key);
        }

        public CookieOptions GetCookieOptions(HttpContext context)
        {
            return new CookieOptions
            {
                Path = context.Request.PathBase.HasValue ? context.Request.PathBase.ToString() : "/",
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                Expires = DateTime.Now.AddDays(100)

            };
        }


    }
}
