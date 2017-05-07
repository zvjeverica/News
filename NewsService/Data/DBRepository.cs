using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsService.Models;

namespace NewsService.Data
{
    public class DBRepository
    {
        private NewsletterDBContext _context;

        public DBRepository(NewsletterDBContext context)
        {
            this._context = context;
        }

        public void AddPerson(Person person)
        {
            _context.Add(person);
            _context.SaveChanges();
        }

        public void AddSubsription(Person person, Topic topic)
        {
            Subscription subscription = new Subscription();
            subscription.Person = person;
            subscription.Topic = topic;

            //_context.Update

            _context.Add(person);
            _context.SaveChanges();
        }
    }
}
