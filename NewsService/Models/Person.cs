using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsService.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "e-mail")]
        public string EMail { get; set; }
        public string Telephone { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public Person()
        {
            this.Subscriptions = new HashSet<Subscription>();
        }
    }
}
