using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volonterio.Data;
using Volonterio.Models;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EFContext _context;
        UsersModels usersModels = new UsersModels();

        public UsersController(EFContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("view")]
        public IActionResult ViewUsers()
        {
            var list = _context.usersModels.ToList();
            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddUsers(string email, string firstname, string secondname, string image, string telephone, string password, int group)
        {
            usersModels.Email = email;
            usersModels.FirstName = firstname;
            usersModels.SecondName = secondname;
            usersModels.Image = image;
            usersModels.Phone = telephone;
            usersModels.Password = password;
            usersModels.GroupsId = group;

            _context.usersModels.Add(usersModels);
            _context.SaveChanges();
            return Ok(new { massage = "Додано" });
        }

        [HttpPost]
        [Route("remove")]
        public IActionResult RemoveUsers(int id)
        {
            var users = _context.usersModels.FirstOrDefault(x => x.Id == id);

            _context.usersModels.Remove(users);
            _context.SaveChanges();
            return Ok(new { massage = "Видаленно" });

        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditUserss(int id, string email, string firstname, string secondname, string image, string telephone, string password, int group)
        {
            UsersModels users = _context.usersModels.SingleOrDefault(x => x.Id == id);
            users.Email = email;
            users.FirstName = firstname;
            users.SecondName = secondname;
            users.Image = image;
            users.Phone = telephone;
            users.Password = password;
            users.GroupsId = group;

            _context.SaveChanges();
            return Ok(new { massage = "Зміненно" });

        }


    }
}
