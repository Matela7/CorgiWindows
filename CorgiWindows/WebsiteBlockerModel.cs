using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiWindows
{
    public class WebsiteBlockerModel
    {
        public List<BlockedWebsite> BlockedWebsites { get; set; } = new List<BlockedWebsite>();

        public void AddBlockedWebsite(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            else if (BlockedWebsites.Any(w => w.Url.Equals(url, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            BlockedWebsites.Add(new BlockedWebsite
            {
                Id = Guid.NewGuid(),
                Url = url
            });
        }

        public void RemoveWebsite(Guid Id)
        {
            var websiteToRemove = BlockedWebsites.FirstOrDefault(w => w.Id == Id);
            if (websiteToRemove != null)
            {
                BlockedWebsites.Remove(websiteToRemove);
            }
        }
    }
}
