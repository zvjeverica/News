using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Data;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repositories
{
    public class ApiRepository
    {
        private NewsletterDBContext _context;
        private PersonRepository _personRepository;
        private TopicRepository _topicRepository;

        public ApiRepository(NewsletterDBContext context)
        {
            _context = context;
            _personRepository = new PersonRepository(context);
            _topicRepository = new TopicRepository(context);
        }

        public IQueryable<Person> GetSubscribers(string[] topics)
        {
            var people = (from p in _context.People
                          join s in _context.Subscriptions on p.Id equals s.PersonId
                          join t in _context.Topics on s.TopicId equals t.Id
                          where topics.Any(x => x.Equals(t.Name, StringComparison.OrdinalIgnoreCase))
                          select p).Include(x => x.Subscriptions).ThenInclude(x => x.Topic).Distinct();
            return people;
        }

        public async Task<IList<Person>> GetAllPeople()
        {
            return await _personRepository.GetAllPeople();
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await _personRepository.GetPersonById(id);
        }
    }
}
