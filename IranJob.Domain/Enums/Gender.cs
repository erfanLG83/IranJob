using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Enums
{
    public enum Gender
    {
        [Display(Name ="مرد")]
        Male,
        [Display(Name = "زن")]
        Female,
        [Display(Name = "مهم نیست")]
        NoMatter
    }
}
