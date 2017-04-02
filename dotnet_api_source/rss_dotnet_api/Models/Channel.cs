using System.Collections.Generic;

namespace rss_dotnet_api.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        public List<Item> ItemList { get; set; }
        public Channel()
        {
            ItemList = new List<Item>();
        }
    }
}