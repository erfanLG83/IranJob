using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Entities
{
    public class Province:BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public List<Job> Jobs { get; set; }
    }
}
