using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proj.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Required, MinLength(2, ErrorMessage = "Title cannot be less than 2!"),
         MaxLength(50, ErrorMessage = "Title cannot be more than 200!")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z ]*$", ErrorMessage = "Country must be capitalized!")]
        public string Country { get; set; }

        // many-to-one relationship
        public virtual ICollection<Track> Tracks { get; set; }

        // one-to one-relationship
        [Required]
        public virtual Wiki Wiki { get; set; }
    }
}