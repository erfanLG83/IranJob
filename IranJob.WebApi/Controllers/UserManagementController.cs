using IranJob.Services.Api;
using IranJob.Services.Contract;
using IranJob.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IranJob.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly IFileWorker _fileWorker;
        private readonly IAppUserManager _userManager;
        public UserManagementController(IFileWorker fileWorker, IAppUserManager userManager)
        {
            _userManager = userManager;
            _fileWorker = fileWorker;
        }
        [HttpPost]
        public async Task<ApiResult<object>> SetResumeFile(IFormFile resumeFile)
        {
            var user = await _userManager.FindByIdAsync(
                User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
            );
            if (user.ResumeFileName != null)
            {
                _fileWorker.RemoveFileInPath("resumes/" + user.ResumeFileName);
            }

            var resumeName = await _fileWorker.AddFileToPathAsync(resumeFile, "resumes/");
            user.ResumeFileName = resumeName;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _userManager.UpdateSecurityStampAsync(user);
            return Ok();
        }
        [HttpPost]
        public async Task<ApiResult<object>> Update(UserUpdateModel userUpdateModel)
        {
            var user = await _userManager.FindByIdAsync(
                User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
            );
            user.FullName = userUpdateModel.FullName;
            user.UserName = userUpdateModel.UserName;
            user.PhoneNumber = userUpdateModel.PhoneNumber;
            if (!user.PhoneNumberConfirmed)
                user.PhoneNumberConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _userManager.UpdateSecurityStampAsync(user);
            return Ok();
        }
    }
}