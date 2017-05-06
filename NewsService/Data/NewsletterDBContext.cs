using Microsoft.EntityFrameworkCore;
using NewsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsService.Data
{
    public class NewsletterDBContext : DbContext
    {
        public NewsletterDBContext(DbContextOptions<NewsletterDBContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
                .HasKey(x => new { x.PersonId, x.TopicId });
        }

        
    }
}
