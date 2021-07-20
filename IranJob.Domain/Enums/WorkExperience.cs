using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Enums
{
    public enum WorkExperience
    {
        [Display(Name = "بدون محدودیت سابقه کار")]
        WithoutRestrictions,
        [Display(Name = "کمتر از 3 سال")]
        LessThanThreeYear,
        [Display(Name = "سه تا شش سال")]
        ThreeToSixYear,
        [Display(Name = "بیش از شش سال")]
        MoreThanSixYear
    }
}
