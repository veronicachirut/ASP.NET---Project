using Proj.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proj.Models
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }

        [Required, MinLength(2, ErrorMessage = "Title cannot be less than 2!"),
         MaxLength(100, ErrorMessage = "Title cannot be more than 100!")]
        public string Title { get; set; }

        [TimeValidator]
        public string Time { get; set; }

        // one-to-many relationship
        [ForeignKey("Artist")]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        // many-to-many relationship
        public virtual ICollection<Album> Albums { get; set; }

        // dropdown list
        public IEnumerable<SelectListItem> ArtistList { get; set; }

        // checkboxes list
        [NotMapped]
        public List<CheckBoxViewModel> AlbumsList { get; set; }
    }
}