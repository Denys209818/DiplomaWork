using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text.RegularExpressions;
using Volonterio.Data;
using Volonterio.Data.Entities;
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
        public UserManager<AppUser> _userManager { get; set; }
        public PublicationController(IMapper mapper, EFContext context, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
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
                post.DateCreated = DateTime.Now;
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

        [HttpPost]
        [Route("delete")]
        [Authorize]
        public async Task<IActionResult> DeletePublication([FromBody] DeletePublicationModel delete)
        {
            IActionResult res = Ok("Публікацію видалено!");
            return await Task.Run(() =>
            {
                var publication = _context.Post.Include(x=> x.PostTagEntities).Include(x => x.Group)
                .Where(x => x.Id == delete.PostId).FirstOrDefault();
                if (publication == null)
                {
                    res = BadRequest(new
                    {
                        Message = "Публікації не існує!"
                    });
                    return res;
                }


                var userId = User.Claims.Where(x => x.Type == "id").First().Value;

                if (userId == null || int.Parse(userId) != publication.Group.UserId)
                {
                    res = BadRequest(new
                    {
                        Message = "Користувач не має права на видалення публікації!"
                    });
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
        [Authorize]
        public async Task<IActionResult> EditPublication([FromBody] EditPublicationModel edit)
        {
            IActionResult res = Ok("Успішно відредаговано!");
            return await Task.Run(() =>
            {
                var publication = _context.Post.Include(x => x.Images).Include(x => x.Group
                ).Include(x => x.PostTagEntities)
                .Where(x => x.Id == edit.PublicationId).FirstOrDefault();


                var userId = User.Claims.Where(x => x.Type == "id").First().Value;

                if (userId == null || int.Parse(userId) != publication.Group.UserId)
                {
                    res = BadRequest(new
                    {
                        Message = "Користувач не має права на редагування публікації!"
                    });
                }

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
                    var item = _context.PostImages.Where(x => x.Image.ToLower()
                    == image.Image.ToLower()).FirstOrDefault();
                    if(item != null)
                    {
                        _context.PostImages.Remove(item);
                        _context.SaveChanges();
                    }
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


        [HttpPost]
        [Route("getpostbygroupid")]
        [Authorize]
        public async Task<IActionResult> GetPostByGroupId([FromBody] int id)
        {
            return await Task.Run(() =>
            {
                var userId = User.Claims.Where(x => x.Type == "id").First().Value;

                var posts = _context.Post.Include(x => x.Images).Where(x => x.GroupId == id).OrderByDescending(x => x.Id)
                .Select(x => _mapper.Map<GetPostByGroupId>(x)).ToList();

              
                if (posts != null)
                {
                    foreach (GetPostByGroupId post in posts)
                    {
                        if (userId != null)
                        {
                            var like = _context.Likes.FirstOrDefault(x => x.PostId == post.Id 
                            && x.UserId == int.Parse(userId));

                            if(like != null)
                            {
                                post.IsLiked = true;
                            }
                        }


                        foreach (var postImage in post.Images)
                        {
                            if (!string.IsNullOrEmpty(postImage))
                            {
                                string imgs = postImage;
                                  #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                AppPostImage img = _context.PostImages
                                .Where(x => x.Image.ToLower() == imgs.ToLower())
                                .FirstOrDefault();
                                   #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                                if (img != null)
                                {
                                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Post", imgs);
                                    if(!System.IO.File.Exists(fullPath))
                                    {
                                        _context.PostImages.Remove(img);
                                        _context.SaveChanges();

                                        post.Images.Remove(imgs);
                                    }
                                }


                            }
                        }
                    }
                }

                return Ok(posts);
            });
        }

        [HttpPost]
        [Route("getpostdatabyid")]
        public async Task<IActionResult> GetPostDataById([FromBody] int id)
        {
            return await Task.Run(() =>
            {
                IActionResult res = Ok();
                AppPost? post = _context.Post.Where(x => x.Id == id).Include(x => x.Group)
                .Include(x => x.Images).Include(x => x.PostTagEntities).Select(x => x).FirstOrDefault();

                var tags = post.PostTagEntities.SelectMany(x => _context.PostTags.Where(y => y.Id == x.PostTagId));

                string tagsString = string.Concat(tags.Select(x => "#" + x.Tag + " ")).Trim();

                if(post != null)
                {
                    var returnedData = _mapper.Map<IPublicationData>(post);
                    returnedData.Tags = tagsString;
                    res = Ok(returnedData);
                    return res;
                }

                res = BadRequest(new
                {
                    Errors = new string[]
                    {
                        "Не знайдено публікацію!"
                    }
                });
                return res;
            });
        }

        [HttpPost]
        [Route("imagedynamicupdate")]
        public async Task<IActionResult> AddImageDynamic([FromBody] IEditDynamicImage image)
        {
            return await Task.Run(() =>
            {
                AppPostImage appImage = new AppPostImage();
                appImage.PostId = image.PostId;
                appImage.Image = image.Image;

                _context.PostImages.Add(appImage);
                _context.SaveChanges();
                return Ok();
            });
        }

        [HttpPost]
        [Route("likepost")]
        [Authorize]
        public async Task<IActionResult> LikePost([FromBody] ILikePostModel like)
        {
            return await Task.Run(() =>
            {
                var userId = User.Claims.Where(x => x.Type == "id").First().Value;
                var liked = _context.Likes.Where(x => x.UserId == int.Parse(userId) && x.PostId == like.PostId)
                .FirstOrDefault();
                if (like.Liked)
                {
                    if(liked == null)
                    {
                        var newLike = new AppLike
                        {
                            PostId = like.PostId,
                            UserId = long.Parse(userId),
                        };

                        _context.Likes.Add(newLike);
                    }
                }
                else
                {
                    if (liked != null)
                    {

                        _context.Likes.Remove(liked);
                    }
                }
                _context.SaveChanges();
                return Ok();
            });
        }

        [HttpGet]
        [Route("getpopularfromgroupbyuserid")]
        [Authorize]
        public async Task<IActionResult> GetPopularPostByUserId()
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "id").First().Value);
            return await Task.Run(() =>
            {
                var groupsId =  _context.Groups.Include(x => x.AppGroupTags).Where(x => _context.UserGroups
                    .Where(y => y.GroupId == x.Id &&
                    y.UserId == userId).Any() || x.UserId == userId).ToList().Select(x => x.Id).ToList();
                List<AppPost> appPosts = new List<AppPost>();
                var date = DateTime.Now;
                foreach (var groupId in groupsId)
                {
                    var posts = _context.Post.Where(x => x.DateCreated.Year == date.Year &&
                    x.DateCreated.Month == date.Month && (x.DateCreated.Day - date.Day) < 4).Include(x=> x.PostTagEntities)
                    .Include(x => x.Images).Include(x => x.Group).Where(x => x.GroupId == groupId);
                    appPosts.AddRange(posts);

                }

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
                    var userObj = _userManager.FindByIdAsync(userId.ToString()).Result;
                    list[index].UserName = userObj.FirstName + " " + userObj.SecondName;
                    list[index].UserEmail = userObj.Email;
                    list[index].UserImage = userObj.Image;

                    list[index].CountLikes = _context.Likes.Where(x => x.PostId == appPost.Id).Count();

                    list[index].GroupImage = appPost.Group.Image;
                    list[index].GroupName = appPost.Group.Title;

                    index++;
                }
                
                return Ok(list);
            });
        }

        [HttpGet]
        [Route("getpopularposts")]
        public async Task<IActionResult> GetPopularPosts([FromQuery] int items)
        {
            return await Task.Run(() =>
            {
                var list = _context.Post.Include(x => x.Images).Include(x => x.Likes)
                .OrderByDescending(x => x.Likes.Count).Take(items).Select(x => 
                _mapper.Map<GetPostByGroupIdSortedPopular>(x)).ToList();



                foreach (var item in list)
                {
                    var group = _context.Groups.Include(x => x.Posts)
                    .Where(x => x.Posts.Where(x => x.Id == item.Id).Any()).First();

                    var likes = _context.Likes.Where(x => x.PostId == item.Id).Count();
                    item.GroupName = group.Title;
                    item.GroupImage = group.Image;
                    item.CountLikes = likes;
                    
                }
                return Ok(list);
            });
        }


        //getallpopularposts
        [HttpGet]
        [Route("getallpopularposts")]
        public async Task<IActionResult> GetAllPopularPosts([FromQuery] int page)
        {
            return await Task.Run(() =>
            {
                int count = 6;
                var list = _context.Post.Include(x => x.Images).Include(x => x.Likes)
                .OrderByDescending(x => x.Likes.Count).Skip(count * page).Take(count).Select(x =>
                _mapper.Map<GetPostByGroupIdSortedPopular>(x)).ToList();



                foreach (var item in list)
                {
                    var group = _context.Groups.Include(x => x.Posts)
                    .Where(x => x.Posts.Where(x => x.Id == item.Id).Any()).First();

                    var likes = _context.Likes.Where(x => x.PostId == item.Id).Count();
                    item.GroupName = group.Title;
                    item.GroupImage = group.Image;
                    item.CountLikes = likes;

                }
                return Ok(list);
            });
        }
        [HttpGet]
        [Route("getpostcount")]
        public async Task<IActionResult> GetCountPublication()
        {
            return await Task.Run(() =>
            {
                return Ok(_context.Post.Count());
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

    class ComparerForPosts : IComparer<AppPost>
    {
        public int Compare(AppPost? x, AppPost? y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}
