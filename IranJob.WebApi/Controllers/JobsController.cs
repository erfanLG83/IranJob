using IranJob.Persistence;
using IranJob.Services.Api;
using IranJob.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using IranJob.Domain.Entities;
using IranJob.Services.Contract;
using Microsoft.AspNetCore.Authorization;

namespace IranJob.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IranJobDbContext _context;
        private readonly IJobRepository _jobRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public JobsController(IranJobDbContext context , IBackgroundJobClient backgroundJobClient ,IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
            _backgroundJobClient = backgroundJobClient;
            _context = context;
        }
        [HttpGet]
        public async Task<ApiResult<IEnumerable<JobListModel>>> GetListJobs(int index = 1, int row = 5)
        {
            int count = _context.Jobs.Count();
            int pages = (count % row == 0) ? count / row : (count / row) + 1;
            return await _context.Jobs
                .Where(x=>!x.Finished)
                .OrderByDescending(x => x.PublishDate).Skip((index - 1) * row).Take(row)
                .Include(x => x.Company)
                .Include(x => x.Province)
                .Select(x => new JobListModel(x)).ToListAsync();
        }
        [HttpPost]
        public async Task<ApiResult<IEnumerable<JobListModel>>> GetListJobs([FromBody] JobsFilterModel filter, int index = 1, int row = 5)
        {
            int count = _context.Jobs.Count();
            int pages = (count % row == 0) ? count / row : (count / row) + 1;
            return await _context.Jobs
                .Where(x=>!x.Finished)
                .Where(x =>!filter.CategoryId.HasValue || x.JobCategoryId == filter.CategoryId)
                .Where(x=>!filter.ProvinceId.HasValue || x.ProvinceId==filter.ProvinceId)
                .Where(x => (
                    String.IsNullOrEmpty(filter.Search) ||
                    (x.Title.Contains(filter.Search) ||
                     x.Description.Contains(filter.Search)
                        /*|| x.SkillsRequired.Split(',').Any(c => filter.Search.Contains(c))*/
                        )
                    )
                )
                .OrderByDescending(x => x.PublishDate)
                .Skip((index - 1) * row).Take(row)
                .Include(x => x.Company)
                .Include(x => x.Province)
                .Select(x => new JobListModel(x)).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ApiResult<SingleJobModel>> GetJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null || job.Finished)
                return NotFound();
            await _context.Entry(job).Reference(x => x.Company).LoadAsync();
            await _context.Entry(job).Reference(x => x.Province).LoadAsync();
            await _context.Entry(job).Reference(x => x.JobCategory).LoadAsync();
            return new SingleJobModel(job);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("[action]/{id}")]
        public async Task<ApiResult> FinishJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();
            if (job.Finished) 
                return Ok();
            job.Finished = true;
            if (job.EndTimeJobId != null)
            {
                _backgroundJobClient.Delete(job.EndTimeJobId);
                job.EndTimeJobId = null;
            }
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            return Ok();

        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("[action]")]
        public async Task<ApiResult> AddJob(CreateJobModel model)
        {
            var job = new Job
            {
                Description = model.Description,
                SkillsRequired = model.SkillsRequired,
                MinimumSalary = model.MinimumSalary,
                WorkExperience = model.WorkExperience,
                ProvinceId = model.ProvinceId,
                JobCategoryId = model.CategoryId,
                PublishDate = DateTime.Now,
                Finished = false,
                Title = model.Title,
                ContractType = model.ContractType,
                ImmediateEmployment = model.ImmediateEmployment,
                CompanyId = model.CompanyId,
                Gender = model.Gender
            };
            job.EndTimeJobId = _backgroundJobClient.Schedule(
                () => _jobRepository.FinishTime(job)
                , TimeSpan.FromDays(60)
            );
            await _context.Jobs.AddAsync(job);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _backgroundJobClient.Delete(job.EndTimeJobId);
                throw e;
            }
            return Ok();
        }
    }
}