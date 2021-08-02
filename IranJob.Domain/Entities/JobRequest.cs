using IranJob.Domain.Auth;
using IranJob.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Entities
{
    public class JobRequest:BaseEntity
    {
        public string ApplicantMessage { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public DateTime Date { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public JobRequestStatus JobRequestStatus { get; set; }
    }
}
