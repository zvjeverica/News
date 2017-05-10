using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Models;


namespace BusinessLogic.Data
{
    public class NewsletterDBContext : DbContext
    {
        public NewsletterDBContext(DbContextOptions<NewsletterDBContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Person>().HasKey(q => q.Id);
            builder.Entity<Topic>().HasKey(q => q.Id);

            builder.Entity<Subscription>().HasKey(q =>
                new { q.PersonId, q.TopicId });

            builder.Entity<Subscription>()
                .HasOne(pt => pt.Person)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(pt => pt.PersonId);

            builder.Entity<Subscription>()
                .HasOne(pt => pt.Topic)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(pt => pt.TopicId);

            base.OnModelCreating(builder);
        } 
    }
}
