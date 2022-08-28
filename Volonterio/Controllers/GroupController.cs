using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Volonterio.Data;
using Volonterio.Data.Entities;
using Volonterio.Data.Entities.CustomEntities;
using Volonterio.Models;
using Volonterio.Services;

namespace Volonterio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private EFContext _context { get; set; }
        private UserManager<AppUser> _userManager { get; set; }
        private IMapper _mapper { get; set; }

        public GroupController(EFContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            this._context = context; 
            this._userManager = userManager;
            this._mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroup createGroup)
        {
            IActionResult res = Ok("Групу створено!");
            return await Task.Run(() =>
            {
                if(_context.Groups.Where(x => x.Title.ToLower() == createGroup.Title.ToLower()).FirstOrDefault() != null) 
                {
                    res = BadRequest(new
                    {
                        Message = "Дана група вже існує!"
                    });
                    return res;
                }


                var group = _mapper.Map<AppGroup>(createGroup);


                if(!string.IsNullOrEmpty(createGroup.Image) && createGroup.Image != "default.jpg")
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Group");
                    string fileName = Path.GetRandomFileName() + ".jpg";

                    string fullPath = Path.Combine(filePath, fileName);

                    Bitmap bmp = ImageWorker.ConvertToBitmap(createGroup.Image);

                    group.Image = fileName;

                    bmp.Save(fullPath);
                }
                else
                {
                    group.Image = "default.jpg";
                }

                var user = _userManager.FindByIdAsync(createGroup.UserId.ToString()).Result;
                if (user == null) 
                {
                    res=  BadRequest(new
                    {
                        Message = "Користувача не існує!"
                    });
                    return res;
                } 
                group.User = user;
                
                _context.Groups.Add(group);
                _context.SaveChanges();

                SetTags(createGroup.Tags, group);

                return res;
            });

        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteGroup([FromBody] DeleteGroup delete)
        {
            IActionResult res = Ok("Групу видалено!");
            return await Task.Run(() =>
            {
                var group = _context.Groups.Where(x => x.Id == delete.GroupId).FirstOrDefault();
                if(group == null) 
                {
                    res = BadRequest(new
                    {
                        Message = "Групи не існує!"
                    });
                    return res;
                }

                if(!string.IsNullOrEmpty(group.Image))
                {
                    string dir = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Group", group.Image);
                    if(System.IO.File.Exists(dir))
                    {
                        System.IO.File.Delete(dir);
                    }
                }



                DeleteTags(delete.GroupId);

                _context.Groups.Remove(group);
                _context.SaveChanges();

                return res;
            });
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditGroup([FromBody] EditGroup edit) 
        {
            IActionResult res = Ok("Відредаговано!");
            return await Task.Run(()=>
            {
                var group = _context.Groups.Where(x => x.Id == edit.GroupId).FirstOrDefault();
                if (group != null)
                {
                    group.Title = edit.Title;
                    group.Meta = edit.Meta;
                    group.Description = edit.Description;

                    if(!string.IsNullOrEmpty(edit.ImageBase64))
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Group");
                        if(System.IO.File.Exists(Path.Combine(filePath, group.Image)))
                        {
                            System.IO.File.Delete(Path.Combine(filePath, group.Image));
                        }
                        string fileName = Path.GetRandomFileName() + ".jpg";
                        Bitmap bmp = ImageWorker.ConvertToBitmap(edit.ImageBase64);

                        bmp.Save(Path.Combine(filePath, fileName));
                        group.Image = fileName;
                    }

                    DeleteTags(edit.GroupId);
                    ///////////////////
                    SetTags(edit.Tags, group);

                }
                else
                {
                    res = BadRequest(new
                    {
                        Message = "Групи не знайдено!"
                    });
                    return res;
                }
                
                return res;

            });
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchGroup([FromBody] SearchGroup search)
        {
            return await Task.Run(() =>
            {
                int count = 5;
                var searchData = _context.Groups.Include(x => x.AppGroupTags)
                .Where(x => x.Title.ToLower().Contains(search.Param.ToLower()))
                .Select(x => _mapper.Map<GroupReturn>(x)).ToList();

                List<GroupReturn> returned = new List<GroupReturn>();


                returned.AddRange(searchData);


                var searchForTag = _context.Tags.Include(x => x.AppGroupTags).Where(x => x.Tag.Contains( search.Param))
                .Select(x => x.AppGroupTags.Select(x => _context.Groups.Where(y => y.Id == x.GroupId)
                .Select(z => _mapper.Map<GroupReturn>(z)).ToList())).ToList();

                foreach (var item in searchForTag)
                {
                    foreach (var listGroup in item)
                    {
                        foreach (var listItem in listGroup)
                        {
                            if(!returned.Where(x => x.Id == listItem.Id).Any())
                            {
                                returned.Add(listItem);
                            }
                        }
                    }
                }

                foreach (var search in returned)
                {

                    var tags = new List<string>();
                    tags.Add("");
                    
                    
                    tags.AddRange(_context.Tags.Include(x => x.AppGroupTags)
                    .Select(x => x.AppGroupTags.Where(x => x.GroupId == search.Id)
                    .First().Tag.Tag).ToList());

                    string tag = string.Join("#",tags);
                    search.Tags = tag;
                }

                var result = returned.Where(x => !_context.UserGroups.Where(y => y.GroupId == x.Id &&
                y.UserId == search.UserId).Any() && !_context.Groups.Where(y  => y.Id == x.Id && 
                y.UserId == search.UserId).Any()).Select(x => x);

                return Ok(result.Skip(count*search.Page).Take(count));
            });
        }

        [HttpPost]
        [Route("getbyid")]
        public async Task<IActionResult> GetGroupById([FromBody] GetById getModel)
        {
            return await Task.Run(() =>
            {
                IActionResult res = null;
                try
                {

                    //var list = _context.Groups.Where(x => x.UserId == getModel.Id)
                    //.Select(x => _mapper.Map<GetByIdResult>(x)).ToList();


                    var groups = _context.Groups.Where(x => _context.UserGroups.Where(y => (y.GroupId == x.Id &&
                    y.UserId == getModel.Id) || x.UserId == getModel.Id).Any()).Select(x => x).ToList();
                    //|| x.UserId == getModel.Id).Any()
                    res = Ok(groups);
                    return res;

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            });
        }

        [HttpPost]
        [Route("getbyname")]
        public async Task<IActionResult> GetGroupByName([FromBody] GetByName getModel)
        {
            return await Task.Run(() =>
            {
                IActionResult res = null;
                try
                {

                    var list = _context.Groups.Where(x => x.Title.ToLower() == 
                    getModel.Name.ToLower())
                    .Select(x => _mapper.Map<GetByIdResult>(x)).ToList();
                    var group = list.FirstOrDefault();
                    if(group != null)
                    {
                        res = Ok(group.Id);
                        return res;
                    }else
                    {
                        res = BadRequest("Не існує групи!");
                        return res;
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            });
        }

        [HttpPost]
        [Route("getgroupsbyid")]
        public async Task<IActionResult> GetGroupsByUserId([FromBody] int id)
        {
            return await Task.Run(() =>
            {
                var groups = _context.Groups.Where(x => x.UserId == id)
                .Select(x => _mapper.Map<GetByIdResult>(x)).ToList();

                
                return Ok(groups);
            });
        }

        [HttpPost]
        [Route("getgroup")]
        public async Task<IActionResult> GetGroup([FromBody] int id)
        {
            return await Task.Run(() =>
            {
                return Ok(_context.Groups.Where(x => x.Id == id).Select(x => new
                {
                    Id = x.Id,
                    Title= x.Title,
                    Meta =x.Meta,
                    Descrption = x.Description,
                    Image= x.Image

                }).FirstOrDefault());
            });
        }

        [HttpPost]
        [Route("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeModal subscribe)
        {
            return await Task.Run(() =>
            {
                if(!_context.UserGroups.Where(x => x.GroupId == subscribe.GroupId && 
                x.UserId == subscribe.UserId).Any())
                {
                    _context.UserGroups.Add(new AppUserGroup
                    {
                        GroupId = subscribe.GroupId,   
                        UserId = subscribe.UserId
                    });

                    _context.SaveChanges();
                }
                return Ok();
            });
        }


        //Custom Methods
        private void DeleteTags(int id)
        {
            var groupTags = _context.GroupTags.Where(x => x.GroupId == id).Select(x => x.TagId).ToList();

            _context.GroupTags.RemoveRange(_context.GroupTags.Where(x => x.GroupId == id));
            _context.SaveChanges();

            foreach (var groupTag in groupTags)
            {
                var tag = _context.Tags.Where(x => x.Id == groupTag).FirstOrDefault();
                int groupCount = _context.GroupTags.Where(x => x.TagId == tag.Id).Count();
                if (groupCount == 0)
                {
                    _context.Tags.Remove(tag);
                    _context.SaveChanges();
                }
            }
        }

        private void SetTags(string Tags, AppGroup group)
        {
            List<string> tags = Tags.Split("#").Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (tags != null && tags.Count() > 0)
            {
                foreach (var tag in tags)
                {
                    var tagItem = _context.Tags.Where(x => x.Tag.ToLower() == tag.ToLower()).FirstOrDefault();
                    if (tagItem == null)
                    {
                        AppTag appTagItem = new AppTag();
                        appTagItem.Tag = tag;

                        _context.Tags.Add(appTagItem);
                        _context.SaveChanges();


                        AppGroupTag groupTag = new AppGroupTag();
                        groupTag.Tag = appTagItem;
                        groupTag.Group = group;

                        _context.GroupTags.Add(groupTag);
                        _context.SaveChanges();
                    }
                    else
                    {
                        AppGroupTag groupTag = new AppGroupTag();
                        groupTag.Tag = tagItem;
                        groupTag.Group = group;

                        _context.GroupTags.Add(groupTag);
                        _context.SaveChanges();
                    }
                }
            }

            _context.SaveChanges();


        }
    }
}
