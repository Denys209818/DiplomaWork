using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volonterio.Data;
using Volonterio.Data.Entities;
using Volonterio.Data.Entities.CustomEntities;
using Volonterio.Models;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private UserManager<AppUser> _userManager { get; set; }
        private EFContext _context { get; set; }
        private IMapper _mapper { get; set; }

        public ChatController(UserManager<AppUser> userManager, EFContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context; 
            _mapper = mapper;
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<IActionResult> AddNewMessage([FromBody] IAddMessage addMessage)
        {
            return await Task.Run(()=>
            {
                IActionResult res = Ok(new
                {
                    IsSended = false
                });
                try
                {
                    int userId = int.Parse(User.Claims.Where(x => x.Type == "id").First().Value);

                    AppGroupMessage message = _mapper.Map<AppGroupMessage>(addMessage);
                    message.UserId = userId;
                    int count = _context.GroupMessages.Where(x => x.UserId == userId 
                    && x.GroupId == message.GroupId
                    && x.DateCreated.Year == message.DateCreated.Year
                    && message.DateCreated.DayOfYear == x.DateCreated.DayOfYear).Count();

                    var group = _context.Groups.Include(x => x.User).Where(x => x.Id == message.GroupId).First();

                    if(count < 3 || group.UserId == userId)
                    {
                        _context.GroupMessages.Add(message);
                        _context.SaveChanges();
                        res = Ok(new
                        {
                            IsSended = true,
                        });
                        return res;
                    }
                    return res;
                }
                catch(Exception ex)
                {
                    return BadRequest(new
                    {
                        Message = ex.Message,  
                    });
                }

                
                
            });
        }

        [HttpGet]
        [Route("getbygroupid")]
        public async Task<IActionResult> GetMessagesByGroupId([FromQuery] int groupId)
        {
            return await Task.Run(() =>
            {

                return Ok(_context.GroupMessages.Where(x => x.GroupId == groupId)
                    .Select(x => new GetMessage
                    {
                        Date = x.DateCreated,
                        Text = x.Message,
                        Image = _userManager.Users
                        .Where(y => y.Id == x.UserId).First().Image
                    }));
            });
        }

        [HttpGet]
        [Route("getbyfriendid")]
        [Authorize]
        public async Task<IActionResult> GetMessagesByFriendId([FromQuery] int friendUserId)
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            IActionResult res = BadRequest(new
            {
                Message = "Повідомлень не існує!"
            }); 
            return await Task.Run(() =>
            {
                var userFriend = _context.UserFriends.Include(x => x.FriendMessages)
                .Where(x => x.Id == friendUserId).FirstOrDefault();
                if(userFriend== null)
                {
                    return res;
                }

                List<FriendMessageInfo> messageInfos = new List<FriendMessageInfo>();
                if(userFriend != null)
                {
                    var messages = userFriend.FriendMessages;
                    foreach (var message in messages)
                    {
                        var item = _mapper.Map<FriendMessageInfo>(message);
                        var user = _userManager.FindByIdAsync(item.UserId.ToString()).Result;

                        item.FullName = user.FirstName + " " + user.SecondName;
                        item.Image = user.Image;

                        messageInfos.Add(item);
                    }
                    
                }

                return Ok(messageInfos);
            });
        }

        [HttpPost]
        [Route("addfriendmessage")]
        [Authorize]
        public async Task<IActionResult> AddFriendMessage(ChatFriend messageChat)
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);

            return await Task.Run(() =>
            {
                _context.FriendMessages.Add(new AppFriendMessage
                {
                    UserId = userId,
                    DateCreated = messageChat.Date,
                    Message = messageChat.Message,
                    UserFriendId = messageChat.ChatId
                });
                _context.SaveChanges();
                return Ok();
            });
        }
    }
}
