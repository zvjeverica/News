using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsService.Models
{
    public class Subscription
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public string TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
