using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjConcept.CustomAuth
{
    public class UserAuthorization
    {
        public bool Edit;
        public bool Create;
        public bool Delete;
        public bool Admin;
        public bool Dev;
        public bool ReadOnly;
        public bool User;

        public UserAuthorization()
        {
            this.Edit = this.Create = this.Delete = this.Admin = this.Dev = this.ReadOnly = this.User = false;
        }
        
        /// <summary>
        /// Sets the appropriate boolean values for permissions based on role specified.
        /// </summary>
        /// <param name="role">Supply the role string user is assigned.</param>
        public UserAuthorization(string role)
            : base()
        {
            this.EnableUserAuthorization(role);
        }

        private void EnableUserAuthorization(string role)
        {
            switch (role)
            {
                case "ReadOnly":
                    this.ReadOnly = true;
                    break;
                case "Basic":
                    this.EnableUserAuthorization("ReadOnly");
                    this.User = this.Create = this.Edit = this.Delete = true;
                    break;
                case "Administrator":
                    this.EnableUserAuthorization("User");
                    this.Admin = true;
                    break;
                case "Developer":
                    this.EnableUserAuthorization("Administrator");
                    this.Dev = true;
                    break;
            }
        }
    }
}