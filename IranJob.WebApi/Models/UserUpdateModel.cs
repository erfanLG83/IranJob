using System.ComponentModel.DataAnnotations;

namespace IranJob.WebApi.Models
{
    public class UserUpdateModel
    {
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
    }
}
