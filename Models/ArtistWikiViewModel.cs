using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proj.Models
{
    public class ArtistWikiViewModel
    {
        [Required, MinLength(2, ErrorMessage = "Title cannot be less than 2!"),
         MaxLength(50, ErrorMessage = "Title cannot be more than 200!")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z ]*$", ErrorMessage = "Country must be capitalized!")]
        public string Country { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot be more than 1000!")]
        public string Description { get; set; }

        [Required, RegularExpression(@"^[1-9](\d{3})$", ErrorMessage = "This is not a valid year!")]
        public int BirthYear { get; set; }

        [Required, RegularExpression(@"^(0[1-9])|(1[012])$", ErrorMessage = "This is not a valid month!")]
        public string BirthMonth { get; set; }

        [Required, RegularExpression(@"^((0[1-9])|([12]\d)|(3[01]))$", ErrorMessage = "This is not a valid day number!")]
        public string BirthDay { get; set; }

        [Required, RegularExpression(@"^[1-9](\d{3})$", ErrorMessage = "This is not a valid year!")]
        public int DebutYear { get; set; }
    }
}