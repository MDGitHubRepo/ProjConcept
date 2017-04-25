using ProjConcept.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjConcept.CustomAuth
{
    /// <summary>
    /// Checks db for user access levels and sets user type in Response.Headers["ViewControl"]
    /// </summary>
    internal class AuthorizeUser
    {
        private List<User> Users;
        private DateTime LastDateTime;
        private int MaximumWebUsersCacheTimeSecs = 180;
        private bool dbAccessAvailable;
        private List<KeyValuePair<string, string>> userControllerAndActions;
        private List<KeyValuePair<string, string>> readOnlyControllerAndActions;

        internal AuthorizeUser()
        {
            this.LastDateTime = DateTime.Now;
            this.ReloadUsersList();
            this.LoadControllerAndActionsList();
        }

        /// <summary>
        /// Returns a bool if user is authorized for controller/action access.  Sets user type in Response.Headers["ViewControl"].
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns>If user is authorized for controller/action access.</returns>
        internal bool AuthorizeSecurityLevel(AuthorizationContext filterContext)
        {
            this.ReloadUsersList();
            User webUser = null;
            if (this.dbAccessAvailable)
            {
                string idName = CookieManager.GetUserLoginId(filterContext.HttpContext.Request.Cookies);

                filterContext.HttpContext.Response.AppendHeader("ViewUser", idName);
                // Usernames should be compared in a case-insensitive manner.
                webUser = this.Users.FirstOrDefault(user => idName.ToLower() == user.UserLoginId.ToLower());
            }


            if (this.dbAccessAvailable && webUser != null)
            {
                filterContext.Controller.ViewBag.ViewUser = webUser.UserLoginId;
                CookieManager.UpdateCookieExpiration(filterContext.HttpContext, webUser.UserLoginId);
                return this.AccessAuthorized(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                                filterContext.ActionDescriptor.ActionName,
                                                webUser.AuthorizationLevel,
                                                filterContext);
            }
            else
            {
                filterContext.Controller.ViewBag.ViewUser = null;
                return this.AccessAuthorized(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                                 filterContext.ActionDescriptor.ActionName,
                                                 null,
                                                 filterContext);
            }                
        }

        private void LoadControllerAndActionsList()
        {
            this.readOnlyControllerAndActions = new List<KeyValuePair<string, string>>();
            this.readOnlyControllerAndActions.Add(new KeyValuePair<string, string>("Home", "Index"));
            this.readOnlyControllerAndActions.Add(new KeyValuePair<string, string>("Home", "About"));
            this.readOnlyControllerAndActions.Add(new KeyValuePair<string, string>("Home", "Contact"));
            this.readOnlyControllerAndActions.Add(new KeyValuePair<string, string>("Home", "Error"));
            this.readOnlyControllerAndActions.Add(new KeyValuePair<string, string>("Home", "Unauthorized"));
            this.readOnlyControllerAndActions.Add(new KeyValuePair<string, string>("Register", "Login"));
            this.readOnlyControllerAndActions.Add(new KeyValuePair<string, string>("Register", "Create"));

            this.userControllerAndActions = new List<KeyValuePair<string, string>>();
            this.userControllerAndActions.AddRange(this.readOnlyControllerAndActions);
            this.userControllerAndActions.Add(new KeyValuePair<string, string>("Register", "Logoff"));
            this.userControllerAndActions.Add(new KeyValuePair<string, string>("Register", "Edit"));

            this.userControllerAndActions.Add(new KeyValuePair<string, string>("UserNotes", "Index"));
            this.userControllerAndActions.Add(new KeyValuePair<string, string>("UserNotes", "Create"));
            this.userControllerAndActions.Add(new KeyValuePair<string, string>("UserNotes", "Edit"));
            this.userControllerAndActions.Add(new KeyValuePair<string, string>("UserNotes", "Delete"));
        }

        // Returns access authorization and applies control header
        private bool AccessAuthorized(string controllerRequested, string actionRequested, int? secLevel, AuthorizationContext filterContext)
        {
            switch (secLevel)
            {
                case 40: //Developer
                    filterContext.HttpContext.Response.AppendHeader("ViewControl", "Developer");
                    filterContext.Controller.ViewBag.ViewControl = new UserAuthorization(filterContext.HttpContext.Response.Headers["ViewControl"]);
                    return true;
                case 30: //Administrator
                    filterContext.HttpContext.Response.AppendHeader("ViewControl", "Administrator");
                    filterContext.Controller.ViewBag.ViewControl = new UserAuthorization(filterContext.HttpContext.Response.Headers["ViewControl"]);
                    return true;
                case 20: //User
                    if (this.userControllerAndActions.Any(u => u.Key == controllerRequested && u.Value == actionRequested))
                    {
                        filterContext.HttpContext.Response.AppendHeader("ViewControl", "User");
                        filterContext.Controller.ViewBag.ViewControl = new UserAuthorization(filterContext.HttpContext.Response.Headers["ViewControl"]);
                        return true;
                    }
                    else
                        return false;
                default: //Read Only User or not authorized
                    if (this.readOnlyControllerAndActions.Any(r => r.Key == controllerRequested && r.Value == actionRequested))
                    {
                        filterContext.HttpContext.Response.AppendHeader("ViewControl", "ReadOnly");
                        filterContext.Controller.ViewBag.ViewControl = new UserAuthorization(filterContext.HttpContext.Response.Headers["ViewControl"]);
                        return true;
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.ViewControl = new UserAuthorization();
                        return false;
                    }
            }
        }

        // Loads user list from database.
        private void ReloadUsersList()
        {
            try
            {
                using (ProjConceptEntities db = new ProjConceptEntities())
                {
                    if (this.Users != null)
                    {
                        // Determine if the maximum amount of time has passed and recache the Users
                        if (DateTime.Now.Subtract(this.LastDateTime).TotalSeconds > this.MaximumWebUsersCacheTimeSecs)
                        {
                            this.Users = db.Users.ToList();
                            foreach (User user in this.Users)
                            {
                                db.Entry(user).Reload();
                            }
                            this.LastDateTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        this.Users = db.Users.ToList();
                    }
                }

                this.dbAccessAvailable = true;
            }
            catch
            {
                this.dbAccessAvailable = false;
            }
        }
    }
}