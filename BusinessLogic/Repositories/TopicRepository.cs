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
    public class TopicRepository
    {
        private NewsletterDBContext _context;
        public TopicRepository (NewsletterDBContext context)
        {
            _context = context;
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
    }
}
