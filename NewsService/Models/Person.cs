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
    public class Person
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

        [DataMember (EmitDefaultValue = false)]
        private IList<string> Topics;
        public Person()
        {
            this.Subscriptions = new HashSet<Subscription>();
        }

        public string ToJson()
        {
            Topics = Subscriptions.Select(o => o.Topic.Name).ToList();
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Person));

            ser.WriteObject(stream, this);

            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }            
    }
}
