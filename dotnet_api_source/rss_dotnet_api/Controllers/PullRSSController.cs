using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rss_dotnet_api.Models;
using Microsoft.EntityFrameworkCore;

namespace rss_dotnet_api.Controllers
{
    [Produces("application/json")]
    [Route("PullRSS")]
    public class PullRSSController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public PullRSSController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_dbContext.Channels.Select(n => n));
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            if (_dbContext.Channels.Any(n=>n.Id==id))
            {
                var channel = _dbContext.Channels.Include(n=>n.ItemList).First(n => n.Id == id);
                return Json(channel.ItemList);
            }
            return Json("Nie ma takiego ID");
        }
    }
}