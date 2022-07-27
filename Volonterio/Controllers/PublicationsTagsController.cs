using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volonterio.Data;
using Volonterio.Models;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsTagsController : ControllerBase
    {
        private readonly EFContext _context;
        PublicationsTagsModel publicationsTagsModel = new PublicationsTagsModel();

        public PublicationsTagsController (EFContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("view")]
        public IActionResult ViewTags()
        {
            var list = _context.publicationsTagsModels.ToList();
            return Ok(list);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddPublicationsTags(int pulic, int tags)
        {
            publicationsTagsModel.PublicationsId = pulic;
            publicationsTagsModel.TegsId = tags;

            _context.publicationsTagsModels.Add(publicationsTagsModel);
            _context.SaveChanges();
            return Ok(new { massage = "Додано" });
        }
    }
}
