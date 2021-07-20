using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Entities
{
    public class JobCategory:BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public List<Job> Jobs { get; set; }
    }
}
