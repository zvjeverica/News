using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace NewsService.Models
{
    [DataContract]
    public class Person : ISerializable
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [Display(Name = "First name")]
        [DataMember]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [DataMember]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "E-mail")]
        [DataMember]
        [Required]
        public string EMail { get; set; }

        [DataMember]
        [Required]
        public string Telephone { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }

        //[DataMember (EmitDefaultValue = false)]
        //private IList<string> Topics;
        public Person()
        {
            this.Subscriptions = new HashSet<Subscription>();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("FirstName", FirstName);
            info.AddValue("LastName", LastName);
            info.AddValue("EMail", EMail);
            info.AddValue("Telephone", Telephone);
            info.AddValue("Topics", Subscriptions.Select(o => o.Topic.Name).ToList());
        }
    }
}
