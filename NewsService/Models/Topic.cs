using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NewsService.Models
{
    [DataContract]
    public class Topic
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public Topic()
        {
            this.Subscriptions = new HashSet<Subscription>();
        }
    }
}
