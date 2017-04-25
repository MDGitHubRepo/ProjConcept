using System;
using System.Web;

namespace ProjConcept.CustomAuth
{
    public static class CookieManager
    {
        private static string CookieName = "ProjConceptCookie";
        private static string LoginIdKey = "LoginId";
        private static string DateCreatedKey = "DateCreated";

        public static void CreateCookie(HttpContextBase httpContext, string userLoginId)
        {
            HttpCookie cookie = new HttpCookie(CookieName);
            cookie.Values.Add(LoginIdKey, userLoginId);
            cookie.Values.Add(DateCreatedKey, DateTime.Now.ToString());
            SetCookieExpiration(cookie);
            httpContext.Response.Cookies.Set(cookie);
        }

        private static void SetCookieExpiration(HttpCookie cookie)
        {
            cookie.Expires = DateTime.Now.AddMinutes(10);
        }

        //public static void UpdateCookieExpiration(HttpCookieCollection cookieCollection)
        //{
        //    HttpCookie cookie = cookieCollection[CookieName];
        //    if (cookie != null)
        //        SetCookieExpiration(cookie);
        //}

        public static void UpdateCookieExpiration(HttpContextBase httpContext, string userLoginId)
        {
            HttpCookie cookie = httpContext.Request.Cookies[CookieName];
            if (cookie != null)
                SetCookieExpiration(cookie);
            else
                CreateCookie(httpContext, userLoginId);

        }

        public static HttpCookie GetCookie(HttpCookieCollection cookieCollection)
        {
            return cookieCollection[CookieName];
        }

        public static string GetUserLoginId(HttpCookieCollection cookieCollection)
        {
            HttpCookie cookie = cookieCollection[CookieName];
            if (cookie != null)
                return cookie.Values.Get(LoginIdKey);
            else
                return "";
        }

        public static void DeleteCookie(HttpContextBase httpContext)
        {
            CreateCookie(httpContext, GetUserLoginId(httpContext.Request.Cookies));
            httpContext.Response.Cookies.Get(CookieName).Expires = DateTime.Now.AddDays(-1);
        }
    }
}