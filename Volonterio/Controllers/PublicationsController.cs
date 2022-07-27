using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volonterio.Data;
using Volonterio.Models;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly EFContext _context;
        PublicationsModels publicationsModels = new PublicationsModels();

        public PublicationsController (EFContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("view")]
        public IActionResult ViewTags()
        {
            var list = _context.publicationsModels.ToList();
            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddPublications(string name, string tegssearch, int idgroup, string image)
        {
            publicationsModels.Name = name;
            publicationsModels.TegsSearch = tegssearch;
            publicationsModels.GroupsId = idgroup;
            publicationsModels.Image = image;

            _context.publicationsModels.Add(publicationsModels);
            _context.SaveChanges();
            return Ok(new { massage = "Додано" });
        }

        [HttpPost]
        [Route("remove")]
        public IActionResult RemovePublication(int id)
        {
            var publications = _context.publicationsModels.FirstOrDefault(x => x.Id == id);

            _context.publicationsModels.Remove(publications);
            _context.SaveChanges();
            return Ok(new { massage = "Видаленно" });

        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditPublications(int id, string name, string tegssearch, int idgroup, string image)
        {
            PublicationsModels p = _context.publicationsModels.SingleOrDefault(x => x.Id == id);
            p.Name = name;
            p.TegsSearch = tegssearch;
            p.GroupsId = idgroup;
            p.Image = image;

            _context.SaveChanges();
            return Ok(new { massage = "Зміненно" });

        }
    }
}
