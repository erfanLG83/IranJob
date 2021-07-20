using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IranJob.Domain.Entities;
using IranJob.Domain.Enums;
using IranJob.Persistence;
using IranJob.Services.Api;
using IranJob.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ApiResult<IEnumerable<JobListModel>>> Index(int index = 1, int row = 5)
        {
            int count = _context.Jobs.Count();
            int pages = (count % row == 0) ? count / row : (count / row) + 1;
            return await _context.Jobs.Skip((index - 1) * row).Take(row)
                .Include(x=>x.Company)
                .Include(x=>x.Province)
                .Select(x=>new JobListModel(x)).ToListAsync();
        }

        [HttpGet("Test")]
        public ApiResult<string> TestApiResult(Gender gender)
        {
            return gender.ToDisplay().First();
        }
    }
}