using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volonterio.Data;
using Volonterio.Models;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly EFContext _context;
        TagsModels tagsModels = new TagsModels();

        public TagsController(EFContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("view")]
        public IActionResult ViewTags()
        {
            var list = _context.tagsModels.ToList();
            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTags(string nametags)
        {
            tagsModels.NameTags = nametags;

            _context.tagsModels.Add(tagsModels);
            _context.SaveChanges();
            return Ok(new { massage = "Додано" });
        }

        [HttpPost]
        [Route("remove")]
        public IActionResult RemoveTags(int id)
        {
            var tags = _context.tagsModels.FirstOrDefault(x => x.Id == id);

            _context.tagsModels.Remove(tags);
            _context.SaveChanges();
            return Ok(new { massage = "Видаленно" });
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditTags(int id, string nametags)
        {
            TagsModels t = _context.tagsModels.SingleOrDefault(x => x.Id == id);
            t.NameTags = nametags;

            _context.SaveChanges();
            return Ok(new { massage = "Зміненно" });

        }
    }
}
