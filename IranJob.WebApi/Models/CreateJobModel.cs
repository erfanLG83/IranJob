using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IranJob.Domain.Enums;

namespace IranJob.WebApi.Models
{
    public class CreateJobModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool ImmediateEmployment { get; set; }
        public string SkillsRequired { get; set; }
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int ProvinceId { get; set; }

        public ContractType ContractType { get; set; }
        public Gender Gender { get; set; }
        public MinimumSalary MinimumSalary { get; set; }
        public WorkExperience WorkExperience { get; set; }
    }
}
