using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Volonterio.Data;
using Volonterio.Data.Entities.CustomEntities;
using Volonterio.Models;
using Volonterio.Services;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        public IMapper _mapper { get; set; }
        public EFContext _context { get; set; }
        public PublicationController(IMapper mapper, EFContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePublication([FromBody] CreatePublicationModel create)
        {
            IActionResult res = Ok("Створено публікацію!");
            return await Task.Run(() =>
            {
                var group = _context.Groups.Where(x => x.Id == create.GroupId).FirstOrDefault();

                if (group == null)
                {
                    res = BadRequest(new
                    {
                        Message = "Групи не існує!"
                    });
                    return res;
                }

                AppPost post = _mapper.Map<AppPost>(create);

                post.Group = group;

                _context.Post.Add(post);
                _context.SaveChanges();

                SetTags(create.Tags, post);

                if (create.Images != null && create.Images.Count() > 0)
                {
                    foreach (var image in create.Images)
                    {
                        AppPostImage img = new AppPostImage
                        {
                            Image = image.Image,
                            Post = post
                        };

                        _context.PostImages.Add(img);
                        _context.SaveChanges();
                    }
                }

                return res;
            });
        }

        [HttpPost]
        [Route("publicationimage")]
        public async Task<IActionResult> AddImageForPublication([FromBody] CreatePublicationImageModel image)
        {
            return await Task.Run(() =>
            {
                string fileName = Path.GetRandomFileName() + ".jpg";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Post", fileName);

                Bitmap bmp = ImageWorker.ConvertToBitmap(image.Image);
                bmp.Save(filePath);
                return Ok(new
                {
                    filename = fileName
                });
            });
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeletePublication([FromBody] DeletePublicationModel delete)
        {
            IActionResult res = Ok("Публікацію видалено!");
            return await Task.Run(() =>
            {
                var publication = _context.Post.Include(x=> x.PostTagEntities).Where(x => x.Id == delete.PostId).FirstOrDefault();
                if (publication == null)
                {
                    res = BadRequest(new
                    {
                        Message = "Публікації не існує!"
                    });
                    return res;
                }

                var postTagIdList = publication.PostTagEntities.Select(x => x.PostTagId).ToList();

                foreach (var postTagId in postTagIdList)
                {
                    var postTag = _context.PostTags.Include(x => x.PostTagEntities).Where(x => x.Id == postTagId).FirstOrDefault();
                    var postTagsEntities = _context.PostTagEntities
                    .Where(x => x.PostTagId == postTag.Id && x.PostId == publication.Id).ToList();

                    
                    _context.PostTagEntities.RemoveRange(postTagsEntities);
                    _context.SaveChanges();

                    if(_context.PostTagEntities.Where(x => x.PostTagId == postTagId).Count() == 0)
                    {
                        _context.PostTags.Remove(_context.PostTags.Where(x => x.Id == postTagId).First());
                        _context.SaveChanges();
                    }
                    
                }

                

                _context.Post.Remove(publication);
                _context.SaveChanges();
                return res;
            });
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditPublication([FromBody] EditPublicationModel edit)
        {
            IActionResult res = Ok("Успішно відредаговано!");
            return await Task.Run(() =>
            {
                var publication = _context.Post.Include(x => x.Images).Include(x => x.PostTagEntities)
                .Where(x => x.Id == edit.PublicationId).FirstOrDefault();
                if (publication == null)
                {
                    res = BadRequest(new
                    {
                        Message = "Публікації не існує!"
                    });
                    return res;
                }

                publication.Title = edit.Title;
                publication.Text = edit.Text;




                List<string> tags = edit.Tags.Split("#").Where(x => !string.IsNullOrEmpty(x)).ToList();
                if (tags != null && tags.Count() > 0)
                {

                    var tagsInItem = publication.PostTagEntities.Where(x => x.PostId == publication.Id);
                    foreach (var tagItem in tagsInItem)
                    {
                        var tagId = tagItem.PostTagId;

                        publication.PostTagEntities.Remove(tagItem);
                        _context.SaveChanges();
                        int countEntities = _context.PostTagEntities.Where(x => x.PostTagId == tagId).Count();
                        if (countEntities == 0)
                        {
                            _context.PostTags.Remove(_context.PostTags.Where(x => x.Id == tagId).First());
                            _context.SaveChanges();
                        }
                    }
                    _context.SaveChanges();

                    foreach (var tag in tags)
                    {
                        var postTagItem = _context.PostTags.Include(x => x.PostTagEntities)
                        .Where(x => x.Tag.ToLower() == tag.ToLower()).FirstOrDefault();
                        if (postTagItem == null)
                        {
                            AppPostTag postTag = new AppPostTag
                            {
                                Tag = tag
                            };
                            _context.PostTags.Add(postTag);

                            _context.PostTagEntities.Add(new AppPostTagEntity
                            {
                                PostTag = postTag,
                                Post = publication
                            });
                            _context.SaveChanges();
                        }
                        else
                        {
                            var item = new AppPostTagEntity
                            {
                                PostTag = postTagItem,
                                Post = publication
                            };
                            if (_context.PostTagEntities
                            .Where(x => x.PostTagId == item.PostTag.Id && x.PostId == item.Post.Id).FirstOrDefault()
                            == null)
                            {
                                _context.PostTagEntities.Add(item);
                                _context.SaveChanges();
                            }
                        }
                    }
                }

                if (edit.Images != null && edit.Images.Count() > 0)
                {
                    foreach (var image in edit.Images)
                    {
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Post", image.Image);
                        var postImg = _context.PostImages.Where(x => x.PostId == publication.Id
                        && x.Image.ToLower() == image.Image.ToLower()).FirstOrDefault();
                        if (postImg == null)
                        {

                            AppPostImage img = new AppPostImage
                            {
                                Image = image.Image,
                                Post = publication
                            };

                            _context.PostImages.Add(img);
                            _context.SaveChanges();

                        }
                    }
                }

                _context.SaveChanges();
                return res;
            });
        }

        [HttpPost]
        [Route("deletepublicationimage")]
        public async Task<IActionResult> DeleteImageForPublication([FromBody] DeleteImagePublicationModel image)
        {
            IActionResult res = Ok("Фотографію видалено!");
            return await Task.Run(() =>
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Post", image.Image);
                if (System.IO.File.Exists(filePath))
                {
                    _context.PostImages.Remove(_context.PostImages.Where(x => x.Image.ToLower()
                    == image.Image.ToLower()).FirstOrDefault());
                    _context.SaveChanges();
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    res = BadRequest(new
                    {
                        Message = "Файла не існує!"
                    });
                    return res;
                }

                return res;
            });
        }



        //Custom Methods
        private void SetTags(string Tags, AppPost post)
        {
            List<string> tags = Tags.Split("#").Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (tags != null && tags.Count() > 0)
            {
                foreach (var tag in tags)
                {
                    var postTagItem = _context.PostTags.Where(x => x.Tag.ToLower() == tag.ToLower()).FirstOrDefault();
                    if (postTagItem == null)
                    {
                        AppPostTag postTag = new AppPostTag
                        {
                            Tag = tag,
                        };
                        _context.PostTags.Add(postTag);

                        _context.PostTagEntities.Add(new AppPostTagEntity
                        {
                            PostTag = postTag,
                            Post = post
                        });
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.PostTagEntities.Add(new AppPostTagEntity
                        {
                            PostTag = postTagItem,
                            Post = post
                        });
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
