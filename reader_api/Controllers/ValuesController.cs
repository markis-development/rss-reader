using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFGetStarted.AspNetCore.NewDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly BloggingContext _db;
        public ValuesController(BloggingContext db)
        {
            _db = db;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _db.Blogs.Select(n=>n.Url).AsEnumerable();
        }

        // GET api/values/COS
        [HttpGet("{value}")]
        public string Get(string value)
        {
            _db.Add(new Blog(){Url = value});
            _db.SaveChanges();
            return "Pomyslnie dodano: {"+value+"} do bazy :)";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
