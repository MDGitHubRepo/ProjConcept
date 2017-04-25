using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjConcept.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public bool UserAlreadyExists(ProjConceptEntities db)
        {
            return db.Users.Any(u => u.UserLoginId.ToLower() == this.UserLoginId.ToLower());
        }
    }

    public class UserMetadata
    {
        [Required, StringLength(50, MinimumLength = 2), Display(Name = "User Login ID")]
        public string UserLoginId { get; set; }
        [Required, StringLength(30, MinimumLength = 2), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, StringLength(30, MinimumLength = 2), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, EmailAddress, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Authorization Level")]
        public byte AuthorizationLevel { get; set; }
    }
}