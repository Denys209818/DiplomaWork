using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volonterio.Data;
using Volonterio.Models;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly EFContext _context;
        GroupsModels groupsModels = new GroupsModels();

        public GroupsController(EFContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("view")]
        public IActionResult ViewGroup()
        {
            var list = _context.groupsModels.ToList();
            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddGroup(string name, string desc, string target)
        {
            groupsModels.Name = name;
            groupsModels.Description = desc;
            groupsModels.Target = target;

            _context.groupsModels.Add(groupsModels);
            _context.SaveChanges();
            return Ok(new {massage = "Додано"});
        }

        [HttpPost]
        [Route("remove")]
        public IActionResult RemoveGroup(int id)
        {
            var group = _context.groupsModels.FirstOrDefault(x => x.Id == id);
         
            _context.groupsModels.Remove(group);
            _context.SaveChanges();
            return Ok(new {massage = "Видаленно"});
           
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditGroup(int id, string name, string desc, string target)
        {
            GroupsModels g = _context.groupsModels.SingleOrDefault(x => x.Id == id);
            g.Name = name;
            g.Description = desc;
            g.Target = target;

            _context.SaveChanges();
            return Ok(new { massage = "Зміненно" });

        }
    }
}
