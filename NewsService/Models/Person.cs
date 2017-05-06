using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsService.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string Telephone { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public Person()
        {
            this.Subscriptions = new HashSet<Subscription>();
        }
    }
}
