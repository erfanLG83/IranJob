using IranJob.Domain.Enums;
using System;
using System.Collections.Generic;

namespace IranJob.Domain.Entities
{
    public class Job :BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public bool ImmediateEmployment { get; set; }
        public string SkillsRequired { get; set; }
        public bool Finished { get; set; }
        public string EndTimeJobId { get; set; }

        public ContractType ContractType { get; set; }
        public Gender Gender { get; set; }
        public MinimumSalary MinimumSalary { get; set; }
        public WorkExperience WorkExperience { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int JobCategoryId { get; set; }
        public JobCategory JobCategory { get; set; }

        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        public List<JobRequest> JobRequests { get; set; }

    }
}
