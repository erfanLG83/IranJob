using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Enums
{
    public enum ContractType
    {
        [Display(Name ="تمام وقت")]
        FullTime,
        [Display(Name = "نیمه وقت")]
        PartTime,
        [Display(Name = "کاراموزی")]
        Novitiate,
        [Display(Name = "دورکاری")]
        TeleWorking
    }
}
