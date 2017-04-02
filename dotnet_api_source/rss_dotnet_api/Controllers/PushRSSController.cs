using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rss_dotnet_api.Models;
using System.Xml;
using System.Net.Http;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace rss_dotnet_api.Controllers
{
    //[Produces("application/json")]
    [Route("PushRSS")]
    public class PushRSSController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public PushRSSController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_dbContext.Channels.Include(n=>n.ItemList).Select(n=>n));
        }

        [HttpPost]
        public void Post(string link,string name,string description)
        {
            if(!_dbContext.Channels.Any(n => n.Link == link))
            {
                _dbContext.Channels.Add(new Channel() { Link = link, Title = name, Description = description });
                _dbContext.SaveChanges();
            }
        }

        [HttpPut]
        public void Put(string channelLink, string link, string name, string description,string pubDate)
        {
            var channel = _dbContext.Channels.FirstOrDefault(n => n.Link == channelLink);
            if (channel!=null)
            {
                if (!_dbContext.Items.Any(n => n.Link == link))
                {
                    channel.ItemList.Add(new Item() { Link = link, Title = name, Description = description, PubDate = pubDate });
                    _dbContext.Update(channel);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}