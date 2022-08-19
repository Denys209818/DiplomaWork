using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
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
        public async Task<IActionResult> Register([FromBody] RegisterUserModel register)
        {
            IActionResult res = Ok();
            AppUser user = _mapper.Map<AppUser>(register);


            return await Task.Run(() =>
            {

                var userFind = _userManager.FindByEmailAsync(user.Email).Result;
                if (userFind == null)
                {
                    var r = _userManager.CreateAsync(user, register.Password).Result;
                    if (r.Succeeded)
                    {

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
                    }
                    else
                    {

                        res = BadRequest(new
                        {
                            Errors = r.Errors.Select(x => "Code: " + x.Code + "; Description: " + x.Description).ToList()
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

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] EditUserModel edit)
        {
            IActionResult res = Ok("Дані змінено!");
            return await Task.Run(() =>
            {
                var user = _userManager.FindByEmailAsync(edit.Email).Result;
                if (user == null)
                {

                    res = BadRequest(new
                    {
                        Errors = new string[] { "Користувача не існує!" }
                    });
                    return res;
                }
                else
                {
                    user.FirstName = edit.FirstName;
                    user.SecondName = edit.SecondName;
                    user.PhoneNumber = edit.PhoneNumber;

                    var upd = _userManager.UpdateAsync(user).Result;
                    if (!upd.Succeeded)
                    {
                        res = BadRequest(new
                        {
                            Errors = upd.Errors.Select(x => "Code: " + x.Code + "; Description: " + x.Description)
                        });
                        return res;
                    }

                    if (!string.IsNullOrEmpty(edit.OldPassword) &&
                    !string.IsNullOrEmpty(edit.Password) &&
                    !string.IsNullOrEmpty(edit.ConfirmPassword)
                    )
                    {

                        if (!edit.OldPassword.ToLower().Equals(edit.Password.ToLower()))
                        {
                            if (edit.Password.ToLower().Equals(edit.ConfirmPassword.ToLower()))
                            {

                                var changed = _userManager.ChangePasswordAsync(user, edit.OldPassword, edit.Password).Result;
                                if (!changed.Succeeded)
                                {
                                    res = BadRequest(new
                                    {
                                        Errors = changed.Errors.Select(x => "Code: " + x.Code + "; Description: " + x.Description)
                                    });
                                    return res;
                                }
                            }
                            else
                            {
                                res = BadRequest(new
                                {
                                    Errors = new string[] { "Паролі не співпадають!" }

                                });
                                return res;
                            }
                        }
                        else
                        {
                            res = BadRequest(new
                            {
                                Errors = new string[] { "Паролі однакові!" }
                            });
                            return res;
                        }
                    }

                }

                return Ok(_jwtBearer.GenerateToken(user));
            });
        }

        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> RemoveUser([FromBody] RemoveUserModel remove)
        {
            IActionResult res = Ok("Користувача видалено!");
            return await Task.Run(() =>
            {
                var user = _userManager.FindByEmailAsync(remove.Email).Result;
                if (user != null)
                {
                    var del = _userManager.DeleteAsync(user).Result;
                    if (!del.Succeeded)
                    {
                        res = BadRequest(new
                        {
                            Errors = del.Errors.Select(x => "Code: " + x.Code + "; Descrption: " + x.Description).ToList()
                        });

                        return res;
                    }
                }
                else
                {
                    res = BadRequest(new
                    {
                        Errors = "Користувача не існує!"
                    });

                    return res;
                }
                return res;
            });
        }

        [HttpPost]
        [Route("changeimage")]
        public async Task<IActionResult> ChangeImage([FromBody] ChangeImageUserModel changeImage)
        {
            IActionResult res = Ok("Фотографію змінено!");
            return await Task.Run(() =>
            {
                var user = _userManager.FindByEmailAsync(changeImage.Email).Result;

                if (user != null)
                {
                    if (changeImage.ImageBase64 != null)
                    {
                        if(changeImage.ImageBase64.Equals("default.jpg"))
                        {
                            string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", user.Image);
                            if (System.IO.File.Exists(oldPath) && !oldPath.Contains("default.jpg"))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                            user.Image = "default.jpg";
                            var upd = _userManager.UpdateAsync(user).Result;
                            if(upd.Succeeded)
                            {

                                return Ok(_jwtBearer.GenerateToken(user));
                            }else
                            {
                                res = BadRequest(new
                                {
                                    Errors = upd.Errors.Select(x => "Code: " + x.Code + "; Description: " + x.Description)
                                });
                                return res;
                            }

                        }
                        string fileName = Path.GetRandomFileName() + ".jpg";
                        string newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                        string filePath = Path.Combine(newFilePath, fileName);

                        try
                        {
                            string fileNameOld = Path.Combine(Directory.GetCurrentDirectory(), "Images", user.Image);

                            if (System.IO.File.Exists(fileNameOld) && !fileNameOld.Contains("default.jpg"))
                            {
                                System.IO.File.Delete(fileNameOld);
                            }

                            var arr = changeImage.ImageBase64.Split(",");
                            var code = arr[1];
                            Bitmap bmp = ImageWorker.ConvertToBitmap(code);
                            bmp.Save(filePath);

                            user.Image = fileName;
                            var upd = _userManager.UpdateAsync(user).Result;
                            if (!upd.Succeeded)
                            {
                                res = BadRequest(new
                                {
                                    Errors = upd.Errors.Select(x => "Code: " + x.Code + "; Description: " + x.Description)
                                });
                                return res;
                            }
                        }
                        catch (Exception ex)
                        {
                            res = BadRequest(new
                            {
                                Errors = new string[] { "Помилка збереження фотографії!", ex.Message }
                            });
                            return res;
                        }

                    }
                    else
                    {
                        res = BadRequest(new
                        {
                            Errors =new string[] { "Фотографії немає!" }
                        });
                        return res;
                    }

                }
                else
                {
                    res = BadRequest(new
                    {
                        Errors = new string[] { "Користувача не існує!" }
                    });
                    return res;
                }
                return Ok(_jwtBearer.GenerateToken(user));
            });
        }


    }
}
