using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proj.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required, MinLength(2, ErrorMessage = "Title cannot be less than 2!"),
         MaxLength(50, ErrorMessage = "Title cannot be more than 50!")]
        public string Title { get; set; }

        [RegularExpression(@"^[1-9](\d{3})$", ErrorMessage = "This is not a valid year!")]
        public int Year { get; set; }

        // many-to-many relationship
        public virtual ICollection<Track> Tracks { get; set; }

        // many-to-many
        public virtual ICollection<ApplicationUser> Users { get; set; }

        // checkboxes list
        [NotMapped]
        public List<CheckBoxViewModel> TracksList { get; set; }
    }
}