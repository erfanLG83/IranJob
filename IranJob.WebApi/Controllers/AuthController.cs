using AutoMapper;
using IranJob.Domain.Auth;
using IranJob.Services.Api;
using IranJob.Services.AutoMapper;
using IranJob.Services.Contract;
using IranJob.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IranJob.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAppUserManager _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(IAppUserManager userManager, IJwtService jwtService, IMapper mapper)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _userManager = userManager;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ApiResult<object>> Login(LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
                return NotFound();
            var result = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!result)
                return new BadRequestResult();
            return new
            {
                token = await _jwtService.GenerateTokenAsync(user)
            };
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ApiResult<object>> SignUp(SignUpModel signUp)
        {
            var user = new AppUser()
            {
                FullName = signUp.FullName,
                Email = signUp.Email,
                EmailConfirmed = true,
                Id = Guid.NewGuid().ToString(),
                UserName = signUp.UserName
            };
            var result = await _userManager.CreateAsync(user, signUp.Password);
            if (!result.Succeeded)
            {
                //HttpContext.Response.StatusCode = 400;
                return BadRequest(result.Errors);
            }
            //return new ApiResult<object>(false,ApiResultStatusCode.BadRequest,
            //    null,
            //    result.Errors.Select(x=>x.Description).ToList()
            //    );
            HttpContext.Response.StatusCode = 201;
            return Created("~/api/auth/signup", signUp);
        }

        //[HttpPost]
        //[Route("[action]")]
        //public ApiResult<ChildViewModel> Test(Child child)
        //{
        //    return Ok(_mapper.Map<ChildViewModel>(child));
        //}
    }
}