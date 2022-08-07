using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volonterio.Data;
using Volonterio.Models;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly EFContext _context;
        MessageModels messageModels = new MessageModels();

        public MessageController(EFContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("view")]
        public IActionResult ViewMessage()
        {
            var list = _context.messages.ToList();
            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddUsers(string message, int userId, int chadId)
        {
            messageModels.Message = message;
            messageModels.UserId = userId;
            messageModels.ChadId = chadId;
            messageModels.DateCreated = DateTime.UtcNow;
            messageModels.DateCreatedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

           _context.messages.Add(messageModels);
           _context.SaveChanges();
            return Ok(new { massage = "Додано" });
        }

        [HttpPost]
        [Route("remove")]
        public IActionResult RemoveUsers(int id)
        {
            var mess = _context.messages.FirstOrDefault(x => x.Id == id);

            _context.messages.Remove(mess);
            _context.SaveChanges();
            return Ok(new { massage = "Видаленно" });

        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditUserss(int id, string message, int userId, int chadId, DateTime dateTimecreated, long unix)
        {
           MessageModels mess = _context.messages.SingleOrDefault(x => x.Id == id);
            mess.Message = message;
            mess.UserId = userId;
            mess.ChadId = chadId;
            mess.DateCreated = dateTimecreated;
            mess.DateCreatedUnix = unix;

            _context.SaveChanges();
            return Ok(new { massage = "Зміненно" });

        }


    }
}
