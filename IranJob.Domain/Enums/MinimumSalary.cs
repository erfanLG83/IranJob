using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Enums
{
    public enum MinimumSalary
    {
        [Display(Name = "توافقی")]
        Agreement,
        [Display(Name = "حقوق پایه (وزارت کار)")]
        BasicSalary,
        [Display(Name = "از سه میلیون")]
        OfThreeMillion,
        [Display(Name = "از پنج میلیون")]
        OfFiveMillion,
        [Display(Name = "از هفت میلیون")]
        OfSevenMillion,
        [Display(Name = "از ده میلیون")]
        OfTenMillion,
        [Display(Name = "از پانزده میلیون")]
        OfFifteenMillion,
    }
}
