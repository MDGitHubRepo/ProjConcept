using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjConcept.Models
{
    [MetadataType(typeof(UserNoteMetadata))]
    public partial class UserNote
    { }

    public class UserNoteMetadata
    {
        public int NoteId { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        public string UserId { get; set; }
        [Required, StringLength(4000, MinimumLength = 1), Display(Name = "Note Entry"), DataType(DataType.MultilineText)]
        public string Note { get; set; }
        [Required, StringLength(50, MinimumLength = 1), Display(Name = "Note Title")]
        public string NoteTitle { get; set; }
        [Display(Name = "Last Updated:")]
        public System.DateTime NoteLastUpdate { get; set; }
    }
}