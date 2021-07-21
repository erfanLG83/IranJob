using System;
using IranJob.Domain.Entities;
using IranJob.Services.Api;

namespace IranJob.WebApi.Models
{
    public class SingleJobModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PastTime { get; set; }
        public bool ImmediateEmployment { get; set; }
        public string SkillsRequired { get; set; }

        public EnumObject ContractType { get; set; }
        public EnumObject Gender { get; set; }
        public EnumObject MinimumSalary { get; set; }
        public EnumObject WorkExperience { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImage { get; set; }

        public int JobCategoryId { get; set; }
        public string JobCategoryTitle { get; set; }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        public SingleJobModel(Job job)
        {
            Id = job.Id;
            Title = job.Title;
            ProvinceId = job.ProvinceId;
            ProvinceName = job.Province.Name;
            JobCategoryId = job.JobCategoryId;
            JobCategoryTitle = job.JobCategory.Title;
            CompanyId = job.CompanyId;
            CompanyName = job.Company.Name;
            CompanyImage = job.Company.ImageName;
            Description = job.Description;
            PastTime = (DateTime.Now.Day * DateTime.Now.Month) - (job.PublishDate.Month * job.PublishDate.Day);
            SkillsRequired = job.SkillsRequired;
            ImmediateEmployment = job.ImmediateEmployment;
            ContractType = job.ContractType;
            Gender = job.Gender;
            MinimumSalary = job.MinimumSalary;
            WorkExperience = job.WorkExperience;
        }
    }
}
