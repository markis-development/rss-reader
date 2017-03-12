using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace Cron.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        readonly ISingularity _singularity;
        static List<string> _messages = new List<string>();

        public HomeController(ISingularity singularity)
        {
            _singularity = singularity;
        }

        [HttpGet]
        public string Get()
        {
            if (_messages.Count == 0)
            {
                return "no messages yet";
            }
            return _messages.ToArray().Aggregate((s1, s2) => $"{s1}\r\n{s2}");
        }

        [HttpPost]
        public string Post(int seconds)
        {


            var job = new SimpleParameterizedJob<string>((parameter, scheduledTime) => 
                odpytajServerGET()
                );

            var schedule = new EveryXTimeSchedule(TimeSpan.FromSeconds(seconds));

            var scheduledJob = _singularity.ScheduleParameterizedJob(
                schedule, job, "Hello World", true); //starts immediately


            return "Started Cron";
        }

        private static async Task odpytajServerGET()
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://reader_api:4999");
                var response = await client.GetAsync("/api/values");
                response.EnsureSuccessStatusCode(); // Throw in not success

                var stringResponse = await response.Content.ReadAsStringAsync();
                var now = DateTime.Now.ToString("h:mm:ss tt");
                _messages.Add($"{now}->{stringResponse}");
                //Console.WriteLine($" {stringResponse}");
            }
        }
    }
}