using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volonterio.Constants;
using Volonterio.Data;
using Volonterio.Data.Entities;
using Volonterio.Models;
using Volonterio.Services;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private IMapper _mapper;
        private UserManager<AppUser> _userManager;
        private IJwtBearerService _jwtBearer;
        public AccountController(IMapper mapper, UserManager<AppUser> userManager, IJwtBearerService jwtBearer)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtBearer = jwtBearer;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserModel register)
        {
            IActionResult res = Ok();
            AppUser user = _mapper.Map<AppUser>(register);


            return await Task.Run(() =>
            {

                var userFind = _userManager.FindByEmailAsync(user.Email).Result;
                if (userFind == null)
                {
                    var r = _userManager.CreateAsync(user, register.Password).Result;
                    var rolRes = _userManager.AddToRoleAsync(user, Roles.USER).Result;
                    if (!r.Succeeded)
                    {
                        res = BadRequest(new
                        {
                            Errors = new
                            {
                                Email = new string[] { "Помилка авторизації користувача!" }
                            }
                        });
                        return res;
                    }



                    res = Ok(new
                    {
                        token = _jwtBearer.GenerateToken(user)
                    });
                }
                else
                {
                    res = BadRequest(new
                    {
                        Errors = new
                        {
                            Email = new string[] { "Користувач вже існує!" }
                        }
                    });
                    return res;
                }

                return res;
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel login)
        {
            IActionResult res = null;
            return await Task.Run(async () =>
            {

                var user = await _userManager.FindByEmailAsync(login.Email);

                if (user == null)
                {
                    res = BadRequest(new
                    {
                        Errors = new
                        {
                            Email = new string[] { "Користувача не існує!" }
                        }
                    });
                    return res;
                }

                if (!(await _userManager.CheckPasswordAsync(user, login.Password)))
                {
                    res = BadRequest(new
                    {
                        Errors = new
                        {
                            Password = new string[] { "Пароль не правильний!" }
                        }
                    });
                    return res;
                }

                res = Ok(new
                {
                    token = _jwtBearer.GenerateToken(user)
                });
                return res;
            });
        }
    }
}
