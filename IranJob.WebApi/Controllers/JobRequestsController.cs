using System;
using IranJob.Domain.Entities;
using IranJob.Persistence;
using IranJob.Services.Api;
using IranJob.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IranJob.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class JobRequestsController : ControllerBase
    {
        private readonly IranJobDbContext _context;

        public JobRequestsController(IranJobDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ApiResult<IEnumerable<JobRequestModel>>> GetAllRequests()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return BadRequest();
            var requests = await _context.JobRequests
                .Where(x => x.UserId == userId)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Company)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Province)
                .ToListAsync();
            return requests.Select(x => new JobRequestModel(x)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<object>> GetRequestDetail(int id)
        {
            var request = await _context.JobRequests
                .Where(x => x.Id == id)
                .Include(x => x.Job)
                .ThenInclude(x => x.Company)
                .Include(x => x.Job)
                .ThenInclude(x => x.Province)
                .FirstAsync();
            if (request == null)
                return NotFound();
            return new JobRequestDetailsModel(request);
        }

        /// <summary>
        /// a action for apply a job
        /// </summary>
        /// <param name="id">jobId in path query</param>
        /// <param name="applyJob"></param>
        /// <returns> </returns>
        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<ApiResult> ApplyJob(int id, [FromBody] ApplyJobModel applyJob)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(x => !x.Finished && x.Id == id);
            if (job == null)
                return NotFound();
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var isDuplicateRequest = await _context.JobRequests
                .Where(x => x.JobId == id && x.UserId == userId)
                .AnyAsync();
            if (isDuplicateRequest)
                return BadRequest();
            var request = new JobRequest
            {
                JobId = id,
                UserId = userId,
                JobRequestStatus = 0,
                ApplicantMessage = applyJob.ApplicantMessage,
                PhoneNumber = applyJob.PhoneNumber,
                Date = DateTime.Now
            };
            await _context.JobRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            HttpContext.Response.StatusCode = 201;
            return Ok();
        }
    }
}