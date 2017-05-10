using BusinessLogic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repositories
{
    public class PersonRepository
    {
        private NewsletterDBContext _context;
        public PersonRepository(NewsletterDBContext context)
        {
            _context = context;
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

        public async Task DeletePerson(int id)
        {
            var person = await _context.People.SingleOrDefaultAsync(m => m.Id == id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
