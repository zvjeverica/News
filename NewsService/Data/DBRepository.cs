using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsService.Models;
using Microsoft.EntityFrameworkCore;

namespace NewsService.Data
{
    public class DBRepository
    {
        private NewsletterDBContext _context;

        public DBRepository(NewsletterDBContext context)
        {
            this._context = context;
        }


        public async Task<IList<Person>> GetAllPeople()
        {
            return await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).ToListAsync();
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreatePerson(Person person)
        {
            _context.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePerson(Person person, List<int> Topics)
        {
            foreach (int newTopic in Topics)
            {
                person.Subscriptions.Add(new Subscription() { Person = person, TopicId = newTopic });
            }
            _context.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditPerson(Person person, IList<int> topics)
        {
            try
            {
                var existingPerson = await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).SingleOrDefaultAsync(m => m.Id == person.Id);
                var newTopics = topics;
                var deletedTopics = existingPerson.Subscriptions.Where(x => !newTopics.Contains(x.Topic.Id)).ToList();
                var addedTopics = newTopics.Except(existingPerson.Subscriptions.Select(o => o.TopicId));

                foreach (Subscription subscription in deletedTopics)
                {
                    existingPerson.Subscriptions.Remove(subscription);
                }
                foreach (int newTopic in addedTopics)
                {
                    existingPerson.Subscriptions.Add(new Subscription() { Person = person, TopicId = newTopic });
                }
                existingPerson.FirstName = person.FirstName;
                existingPerson.LastName = person.LastName;
                existingPerson.Telephone = person.Telephone;
                existingPerson.EMail = person.EMail;
                _context.Update(existingPerson);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(person.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeletePerson (int id)
        {
            var person = await _context.People.SingleOrDefaultAsync(m => m.Id == id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }

        public async Task<IList<Topic>> GetAllTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        public async Task<Topic> GetTopicById(int id)
        {
            return await _context.Topics.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateTopic(Topic topic)
        {
            _context.Add(topic);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditTopic(Topic topic)
        {
            try
            {
                _context.Update(topic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(topic.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteTopic(int id)
        {
            var topic = await _context.Topics.SingleOrDefaultAsync(m => m.Id == id);
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.Id == id);
        }


        public IQueryable<Person> GetSubscribers (string [] topics)
        {
            var people = (from p in _context.People
                          join s in _context.Subscriptions on p.Id equals s.PersonId
                          join t in _context.Topics on s.TopicId equals t.Id
                          where topics.Any(x => x.Equals(t.Name, StringComparison.OrdinalIgnoreCase))
                          select p).Include(x => x.Subscriptions).ThenInclude(x => x.Topic).Distinct();
            return people;
        }
    }
}
