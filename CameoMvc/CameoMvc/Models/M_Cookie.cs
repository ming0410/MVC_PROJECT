using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;


namespace CameoMvc.Models
{
    public class M_Cookie
    {
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            // For security purpose, we need to encrypt the value.
            //HttpCookie encodedCookie = HttpSecureCookie.Encode(new HttpCookie(key, value));
            HttpCookie encodedCookie = new HttpCookie(key, HttpContext.Current.Server.UrlEncode(value));

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
        }

        public static string GetCookie(string key)
        {
            string value = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                // For security purpose, we need to encrypt the value.
                //HttpCookie decodedCookie = HttpSecureCookie.Decode(cookie);
                HttpCookie decodedCookie = cookie;
                value = HttpContext.Current.Server.UrlDecode(decodedCookie.Value);
            }
            return value;
        }
    }
}