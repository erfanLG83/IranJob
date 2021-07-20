using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IranJob.WebApi.Models
{
    public class LoginModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public string UserName { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }
    }
}
