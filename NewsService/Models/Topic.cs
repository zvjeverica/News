using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsService.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public Topic()
        {
            this.Subscriptions = new HashSet<Subscription>();
        }
    }
}
