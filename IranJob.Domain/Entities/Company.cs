using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IranJob.Domain.Entities
{
    public class Company:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Introduction { get; set; }
        [Required]
        public string NumberOfEmployees { get; set; }
        [Required]
        public string EstablishedYear { get; set; }
        public string Website { get; set; }
        [Required]
        public string ImageName { get; set; }

        public List<Job> Jobs { get; set; }
    }
}
