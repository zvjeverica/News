using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsService.Models
{
    public class Subscription
    {
        [ForeignKey ("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [ForeignKey ("Topic")]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
