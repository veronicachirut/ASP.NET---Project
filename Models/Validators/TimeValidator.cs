using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proj.Models.Validators
{
    public class TimeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var track = (Track)validationContext.ObjectInstance;
            string time = track.Time;
            bool cond = false;
            if (time.Contains(":"))
            {
                string[] minsec = time.Split(':');

                int min, sec;
                bool minIsNumeric = int.TryParse(minsec[0], out min);
                bool secIsNumeric = int.TryParse(minsec[1], out sec);

                if (minIsNumeric && secIsNumeric)
                {
                    if (sec < 60)
                    {
                        cond = true;
                    }
                }
            }

            return cond ? ValidationResult.Success : new ValidationResult("Format of time mm:ss");
        }
    }
}