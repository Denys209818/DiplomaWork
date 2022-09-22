using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.Linq;
using Volonterio.Constants;
using Volonterio.Data;
using Volonterio.Data.Entities;
using Volonterio.Data.Entities.CustomEntities;
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
        private EFContext _context;
        public AccountController(IMapper mapper, UserManager<AppUser> userManager,
            IJwtBearerService jwtBearer, EFContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtBearer = jwtBearer;
            _context = context;
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


                try
                {

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
                }
                catch (Exception ex)
                {

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
                    if (!string.IsNullOrEmpty(changeImage.ImageBase64))
                    {
                        if (changeImage.ImageBase64.Equals("default.jpg"))
                        {
                            string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", user.Image);
                            if (System.IO.File.Exists(oldPath) && !oldPath.Contains("default.jpg"))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                            user.Image = "default.jpg";
                            var upd = _userManager.UpdateAsync(user).Result;
                            if (upd.Succeeded)
                            {

                                return Ok(_jwtBearer.GenerateToken(user));
                            }
                            else
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
                    else if (changeImage.ImageBase64 == "")
                    {
                        return Ok();
                    }
                    else
                    {
                        res = BadRequest(new
                        {
                            Errors = new string[] { "Фотографії немає!" }
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

        [HttpGet("friends")]
        [Authorize]
        public async Task<IActionResult> GetAllFriends([FromQuery] string? page)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type
            == "id").First().Value);
            return await Task.Run(() =>
            {
                var friendsGroups = _context.UserFriends.Include(x => x.AppFriends).Where(x => x.AppFriends
                .Where(y => y.UserId == userId).Any()).Select(x => x.AppFriends).ToList();

                List<UserFriendReturned> appUsers = new List<UserFriendReturned>();

                foreach (var friendColl in friendsGroups)
                {
                    var friend = friendColl.Where(x => x.UserId != userId).First();
                    var userFriend = _context.UserFriends.Include(x=> x.AppFriends)
                    .Where(x => x.Id == friend.UserFriendId).First();

                    var iam = userFriend.AppFriends.Where(x => x.UserId == userId).First();


                    if (friend != null && friend.IsFriend && iam.IsFriend)
                    {
                        var user = _userManager.FindByIdAsync(friend.UserId.ToString()).Result;
                        UserFriendReturned ret = new UserFriendReturned();
                        ret.ChatId = "UserChatId" + friend.UserFriendId;
                        ret.Id = user.Id;
                        ret.Image = user.Image;
                        ret.FirstName = user.FirstName;
                        ret.SecondName = user.SecondName;
                        ret.Phone = user.PhoneNumber;
                        ret.Email = user.Email;
                        appUsers.Add(ret);
                    }
                }
                if(!string.IsNullOrEmpty(page))
                {
                    int skiped = 6;
                    return Ok(appUsers.Skip(skiped * int.Parse(page)).Take(skiped));
                }
                return Ok(appUsers);
            });
        }

        [HttpGet("friendsrequest")]
        [Authorize]
        public async Task<IActionResult> GetAllFriendsRequest([FromQuery] string? page)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type
            == "id").First().Value);
            return await Task.Run(() =>
            {
                var friendsGroups = _context.UserFriends.Include(x => x.AppFriends).Where(x => x.AppFriends
                .Where(y => y.UserId == userId).Any()).Select(x => x.AppFriends).ToList();

                List<UserFriendReturned> appUsers = new List<UserFriendReturned>();

                foreach (var friendColl in friendsGroups)
                {
                    var friend = friendColl.Where(x => x.UserId != userId).First();
                    var userFriend = _context.UserFriends.Include(x => x.AppFriends)
                    .Where(x => x.Id == friend.UserFriendId).First();

                    var iam = userFriend.AppFriends.Where(x => x.UserId == userId).First();


                    if (friend != null && (!friend.IsFriend || !iam.IsFriend))
                    {
                        var user = _userManager.FindByIdAsync(friend.UserId.ToString()).Result;
                        UserFriendReturned ret = new UserFriendReturned();
                        ret.ChatId = "UserChatId" + friend.UserFriendId;
                        ret.Id = user.Id;
                        ret.Image = user.Image;
                        ret.FirstName = user.FirstName;
                        ret.SecondName = user.SecondName;
                        ret.Phone = user.PhoneNumber;
                        ret.Email = user.Email;
                        ret.IsFriend = iam.IsFriend;
                        appUsers.Add(ret);
                    }
                }
                if (!string.IsNullOrEmpty(page))
                {
                    int skipped = 6;
                    return Ok(appUsers.Skip(skipped * int.Parse(page)).Take(skipped));
                }
                return Ok(appUsers);
            });
        }

        [HttpGet]
        [Route("claimrequest")]
        [Authorize]
        public async Task<IActionResult> ClaimRequestUser([FromQuery] string email)
        {
            return await Task.Run(() =>
            {
                var appUser = _userManager.FindByEmailAsync(email).Result;
                long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
                if(appUser != null)
                {

                    var userFriend = _context.UserFriends.Include(x => x.AppFriends).
                    Where(x => x.AppFriends.Where(x=> x.UserId == appUser.Id).Any() && 
                    x.AppFriends.Where(x => x.UserId == userId).Any()).FirstOrDefault();

                    userFriend.IsFriend = true;

                    foreach (var friend in userFriend.AppFriends)
                    {
                        friend.IsFriend = true;
                    }
                }

                _context.SaveChanges();


                return Ok();
            });
        }

        [HttpGet]
        [Authorize]
        [Route("getuserdata")]
        public async Task<IActionResult> GetUserData()
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            return await Task.Run(() =>
            {
                int groupsCount = _context.Groups.Where(x => x.UserId == userId).Count();
                int postsCount = _context.Post.Include(x => x.Group).Where(x => x.Group.UserId == userId)
                .Select(x => x).Count();
                //_context.Friends.Where(x => x.UserId == userId)
                int friendsCount = GetFriends(userId).Count();
                UserDataProfile profileData = new UserDataProfile();
                profileData.FriendsCount = friendsCount;
                profileData.PostsCount = postsCount;
                profileData.GroupsCount = groupsCount;
                return Ok(profileData);
            });
        }


        [HttpPost("googlelogin")]
        public async Task<IActionResult> GoogleExternalLoginAsync([FromBody] ExternalLoginRequest request)
        {
            var payload = await _jwtBearer.VerifyGoogleToken(request);
            if (payload == null)
            {
                return BadRequest(new[] {
                    "Щось пішло не так!"
                });
            }
            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        FirstName = payload.GivenName,
                        SecondName = payload.FamilyName,
                        PhoneNumber = "38(000)000-00-00",
                        Image = "default.jpg",
                        
                    };
                    string tempPassword = Guid.NewGuid().ToString();
                    var resultCreate = await _userManager.CreateAsync(user, tempPassword);
                    if (!resultCreate.Succeeded)
                    {
                        return BadRequest(new[] {
                            "Щось пішло не так!"
                        });
                    }

                    var resultAddLoginAny = await _userManager.AddLoginAsync(user, info);
                    if (!resultAddLoginAny.Succeeded)
                    {
                        return BadRequest(new[] {
                            "Щось пішло не так!"
                        });
                    }

                    return Ok(new { 
                        token = _jwtBearer.GenerateToken(user),
                        tempPassword = tempPassword
                    });
                }

                var resultAddLogin = await _userManager.AddLoginAsync(user, info);
                if (!resultAddLogin.Succeeded)
                {
                    return BadRequest(new[] {
                    "Щось пішло не так!"
                });
                }
            }

            return Ok(_jwtBearer.GenerateToken(user));
        }

        [HttpPost("getuserinfo")]
        public async Task<IActionResult> GetUserInfoByEmail([FromBody] string email)
        {
            IActionResult res = Ok();
            return await Task.Run(() =>
            {
                var user = _userManager.FindByEmailAsync(email).Result;

                if(user == null)
                {
                    res= BadRequest(new[] { 
                     "Користувача не існує!"
                    });
                    return res;
                }
                var groups = _context.Groups.Include(x => x.Posts).Where(x => x.UserId == user.Id);
                int postsCount = _context.Post.Include(x => x.Group).Where(x => x.Group.UserId == user.Id).Count();
                var friends = GetFriends(user.Id);
                return Ok(new
                {
                    Image = user.Image,
                    FullName = user.FirstName + " " + user.SecondName,
                    Email = user.Email,
                    GroupsCount = groups.Count(),
                    PostsCount = postsCount,
                    FriendsCount = friends.Count(),
                    Friends = friends,
                    Posts = GetPosts(user.Id),
                    Groups = GetGroups(user.Id)
                });
            });
        }

        [HttpPost]
        [Authorize]
        [Route("addfriend")]
        public async Task<IActionResult> AddFriendAsync([FromBody] string email)
        {
            IActionResult res = Ok();
            var userId = User.Claims.Where(x => x.Type == "id").First().Value;
            return await Task.Run(() =>
            {
                if(userId == null)
                {
                    return BadRequest(new[] { 
                        "Користувача не існує!"
                    });
                }
                AppUser friend = _userManager.FindByEmailAsync(email).Result;


                var isFriendCompleted = _context.UserFriends.Include(x => x.AppFriends).Where(x => x.AppFriends
                .Where(x => x.UserId == long.Parse(userId)).Any() &&
                x.AppFriends.Where(x => x.UserId == friend.Id).Any()
                ).FirstOrDefault();

                if (isFriendCompleted != null)
                {
                    return BadRequest(new [] { 
                        "Запит на дружбу вже існує!"
                    });
                }

                var isUserFriend = _context.UserFriends.Include(x => x.AppFriends)
                .Where(x => x.AppFriends.Where(y => y.UserId == long.Parse(userId)).Any() &&
                x.AppFriends.Where(y => y.UserId == friend.Id).Any()
                ).FirstOrDefault();

                if (isUserFriend == null)
                {

                    AppUserFriend userFriend = new AppUserFriend();
                    userFriend.IsFriend = false;

                    _context.UserFriends.Add(userFriend);

                    AppFriend myFriend = new AppFriend();
                    myFriend.UserFriend = userFriend;
                    myFriend.UserId = long.Parse(userId);
                    myFriend.IsFriend = true;

                    _context.Friends.Add(myFriend);

                    AppFriend youFriend = new AppFriend();
                    youFriend.UserId = friend.Id;
                    youFriend.UserFriend = userFriend;
                    youFriend.IsFriend = false;

                    _context.Friends.Add(youFriend);



                    _context.SaveChanges();

                }

                return res;
            });
        }

        [HttpGet]
        [Route("searchfriend")]
        [Authorize]
        public async Task<IActionResult> GetFriendsByParam([FromQuery] string param, string page)
        {
            long userId =long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            return await Task.Run(() =>
            {
                var users = _context.Users.Include(x => x.Friends).Where(x => x.Email.ToLower().Contains(param.ToLower()) ||
                (x.FirstName + " " + x.SecondName).ToLower().Contains(param.ToLower()))
                .Where(x => x.Id != userId).ToList();

                List<AppUser> newUsers = new List<AppUser>();
                foreach (var user in users)
                {
                    var listFriend = _context.UserFriends.Include(x => x.AppFriends).Where(x => x.AppFriends
                    .Where(y => y.UserId == user.Id).Any() && x.AppFriends
                    .Where(y => y.UserId == userId).Any()).FirstOrDefault();

                    if(listFriend == null)
                    {
                        newUsers.Add(user);
                    }
                }
                var list = newUsers.Select(x => _mapper.Map<ReturnedSearchUser>(x)).ToList();

                int skip = 6;
                return Ok(list.Skip(int.Parse(page) * skip).Take(skip));
            });
        }
        [HttpGet]
        [Route("deletefriend")]
        [Authorize]
        public async Task<IActionResult> DeleteFriendByEmail([FromQuery] string email)
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            return await Task.Run(() =>
            {
                var user = _userManager.FindByEmailAsync(email).Result;
                var userFriend = _context.UserFriends.Include(x => x.AppFriends)
                .Where(x => x.AppFriends.Where(y => y.UserId == userId).Any()
                && x.AppFriends.Where(y => y.UserId == user.Id).Any()).FirstOrDefault();


                foreach (var friend in userFriend.AppFriends)
                {
                    _context.Friends.Remove(friend);
                }

                _context.UserFriends.Remove(userFriend);

                _context.SaveChanges();
                return Ok();
            });
        }

        [HttpGet]
        [Route("getfriendscount")]
        [Authorize]
        public async Task<IActionResult> GetUserCount()
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            return await Task.Run(() => {
                return Ok(GetFriends(userId).Count());
            });
        }

        [HttpGet]
        [Route("getfriendsrequestcount")]
        [Authorize]
        public async Task<IActionResult> GetUserRequest()
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            return await Task.Run(() =>
            {
                return Ok(GetFriendsRequest(userId));
            });
        }

        [HttpGet]
        [Route("searchfriendcount")]
        [Authorize]
        public async Task<IActionResult> GetFriendsByParamCount([FromQuery] string param)
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            return await Task.Run(() =>
            {
                var users = _context.Users.Include(x => x.Friends).Where(x => x.Email.ToLower().Contains(param.ToLower()) ||
                (x.FirstName + " " + x.SecondName).ToLower().Contains(param.ToLower()))
                .Where(x => x.Id != userId).ToList();

                List<AppUser> newUsers = new List<AppUser>();
                foreach (var user in users)
                {
                    var listFriend = _context.UserFriends.Include(x => x.AppFriends).Where(x => x.AppFriends
                    .Where(y => y.UserId == user.Id).Any() && x.AppFriends
                    .Where(y => y.UserId == userId).Any()).FirstOrDefault();

                    if (listFriend == null)
                    {
                        newUsers.Add(user);
                    }
                }
                var list = newUsers.Select(x => _mapper.Map<ReturnedSearchUser>(x)).ToList();

                return Ok(list.Count());
            });
        }

        private List<UserFriendReturned> GetFriends(long userId)
        {
            var friendsGroups = _context.UserFriends.Include(x => x.AppFriends).Where(x => x.AppFriends
                .Where(y => y.UserId == userId).Any()).Select(x => x.AppFriends).ToList();

            List<UserFriendReturned> appUsers = new List<UserFriendReturned>();

            foreach (var friendColl in friendsGroups)
            {
                var friend = friendColl.Where(x => x.UserId != userId).First();
                if (friend != null)
                {
                    var userFriend = _context.UserFriends.Include(x => x.AppFriends)
                .Where(x => x.AppFriends.Where(y => y.UserId == userId).Any()
                && x.AppFriends.Where(y => y.UserId == friend.UserId).Any()).FirstOrDefault();
                    if (userFriend.IsFriend)
                    {
                        var user = _userManager.FindByIdAsync(friend.UserId.ToString()).Result;
                        UserFriendReturned ret = new UserFriendReturned();
                        ret.ChatId = "UserChatId" + friend.UserFriendId;
                        ret.Id = user.Id;
                        ret.Image = user.Image;
                        ret.FirstName = user.FirstName;
                        ret.SecondName = user.SecondName;
                        ret.Phone = user.PhoneNumber;
                        ret.Email = user.Email;
                        appUsers.Add(ret);
                    }
                }
            }
            return appUsers;
        }

        private int GetFriendsRequest(long userId)
        {
            var friendsGroups = _context.UserFriends.Include(x => x.AppFriends).Where(x => x.AppFriends
                .Where(y => y.UserId == userId).Any()).Select(x => x.AppFriends).ToList();

            List<UserFriendReturned> appUsers = new List<UserFriendReturned>();

            foreach (var friendColl in friendsGroups)
            {
                var friend = friendColl.Where(x => x.UserId != userId).First();
                if (friend != null)
                {
                    var userFriend = _context.UserFriends.Include(x => x.AppFriends)
                .Where(x => x.AppFriends.Where(y => y.UserId == userId).Any()
                && x.AppFriends.Where(y => y.UserId == friend.UserId).Any()).FirstOrDefault();
                    if (!userFriend.IsFriend)
                    {
                        var user = _userManager.FindByIdAsync(friend.UserId.ToString()).Result;
                        UserFriendReturned ret = new UserFriendReturned();
                        ret.ChatId = "UserChatId" + friend.UserFriendId;
                        ret.Id = user.Id;
                        ret.Image = user.Image;
                        ret.FirstName = user.FirstName;
                        ret.SecondName = user.SecondName;
                        ret.Phone = user.PhoneNumber;
                        ret.Email = user.Email;
                        appUsers.Add(ret);
                    }
                }
            }
            return appUsers.Count();
        }

        private object GetGroups(long userId)
        {
            var groups = _context.Groups.Include(x => x.AppGroupTags).Where(x => _context.UserGroups
                    .Where(y => y.GroupId == x.Id &&
                    y.UserId == userId).Any() || x.UserId == userId).ToList().Select(x => new
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Meta = x.Meta,
                        Description = x.Description,
                        UserId = x.UserId,
                        Image = x.Image,
                        Tags = SetStringTag(x.AppGroupTags)
                    }).ToList();
            return groups;
        }

        private string SetStringTag(IEnumerable<AppGroupTag> groupTags)
        {
            List<string> tags = new List<string>();
            foreach (var groupTag in groupTags)
            {
                tags.Add("#" + _context.Tags.Where(x => x.Id == groupTag.TagId).First().Tag + " ");
            }

            string tag = String.Concat(tags);
            return tag.Trim();
        }

        private List<GetPostByGroupIdSorted> GetPosts(long userId)
        {

            List<AppPost> appPosts = _context.Post.Include(x => x.Group).Include(x => x.PostTagEntities)
                .Where(x => x.Group.UserId == userId).Take(20).ToList();

            appPosts.Sort(new ComparerForPosts());
            appPosts.Reverse();
            var list = appPosts.Select(x => _mapper.Map<GetPostByGroupIdSorted>(x)).ToList();

            int index = 0;
            foreach (var appPost in appPosts)
            {
                List<string> postTags = new List<string>();
                foreach (var postTag in appPost.PostTagEntities)
                {
                    var item = _context.PostTags.Where(x => x.Id == postTag.PostTagId).FirstOrDefault();

                    string tag = item.Tag;
                    postTags.Add(tag);
                }


                string readyTags = string.Concat(postTags.Select(x => "#" + x + " ")).Trim();
                list[index].Tags = readyTags;
                var userObj = _userManager.FindByIdAsync(appPost.Group.UserId.ToString()).Result;
                list[index].UserName = userObj.FirstName + " " + userObj.SecondName;
                list[index].UserEmail = userObj.Email;
                list[index].UserImage = userObj.Image;

                list[index].CountLikes = _context.Likes.Where(x => x.PostId == appPost.Id).Count();

                list[index].GroupImage = appPost.Group.Image;
                list[index].GroupName = appPost.Group.Title;

                index++;
            }

            return list;
        }
    }
}
