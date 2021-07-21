using IranJob.Persistence;
using IranJob.Services.Api;
using IranJob.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IranJob.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IranJobDbContext _context;
        public JobsController(IranJobDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ApiResult<IEnumerable<JobListModel>>> GetListJobs(int index = 1, int row = 5)
        {
            int count = _context.Jobs.Count();
            int pages = (count % row == 0) ? count / row : (count / row) + 1;
            return await _context.Jobs.OrderByDescending(x => x.PublishDate).Skip((index - 1) * row).Take(row)
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
                .OrderByDescending(x => x.PublishDate)
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
                .Skip((index - 1) * row).Take(row)
                .Include(x => x.Company)
                .Include(x => x.Province)
                .Select(x => new JobListModel(x)).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ApiResult<SingleJobModel>> GetJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();
            await _context.Entry(job).Reference(x => x.Company).LoadAsync();
            await _context.Entry(job).Reference(x => x.Province).LoadAsync();
            await _context.Entry(job).Reference(x => x.JobCategory).LoadAsync();
            return new SingleJobModel(job);
        }
    }
}