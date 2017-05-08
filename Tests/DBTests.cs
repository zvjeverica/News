using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsService.Models;
using NewsService.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class DBTests
    {
        public string ConnectionString = "Server=sql11.freemysqlhosting.net,3306;database=sql11172899;user=sql11172899;password=VI1D6mcIcI";
        public NewsletterDBContext context;
        [TestInitialize()]
        public void Initialize()
        {
            DbContextOptions<NewsletterDBContext> options = new DbContextOptionsBuilder<NewsletterDBContext>().UseMySql(ConnectionString).Options;
            context = new NewsletterDBContext(options);
        }


        [TestMethod]
        public void AddPerson()
        {
            var newPerson = new Person();
            newPerson.FirstName = "First";
            newPerson.LastName = "Last";
            newPerson.EMail = "Email";
            newPerson.Telephone = "Phone";

            DbContextOptions<NewsletterDBContext> options = new DbContextOptionsBuilder<NewsletterDBContext>().UseMySql(ConnectionString).Options;
            using (var dbCtx = new NewsletterDBContext(options))
            {
                dbCtx.Add(newPerson);
                dbCtx.SaveChanges();
            }
        }

        [TestMethod]
        public void AddPersonWithTopic()
        {
            var newPerson = new Person();
            newPerson.FirstName = "First1";
            newPerson.LastName = "Last";
            newPerson.EMail = "Email";
            newPerson.Telephone = "Phone";

            var newTopic = new Topic();
            newTopic.Name = "Topics1";

            var sub = new Subscription();
            sub.Person = newPerson;
            sub.Topic = newTopic;

            newPerson.Subscriptions.Add(sub);
            //newTopic.Subscriptions.Add(sub);


            DbContextOptions<NewsletterDBContext> options = new DbContextOptionsBuilder<NewsletterDBContext>().UseMySql(ConnectionString).Options;
            using (var dbCtx = new NewsletterDBContext(options))
            {
                dbCtx.Add(newPerson);
                dbCtx.Add(newTopic);

                dbCtx.SaveChanges();
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ReadPersonAsync()
        {
            //var person = context.People.Include(x => x.Subscription).SingleOrDefault(x => x.Id == 7);

            var person = await context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).SingleOrDefaultAsync(m => m.Id == 7);
            //var person = await context.People
            //    .SingleOrDefaultAsync(m => m.Id == 7);
            if (person == null)
            {
                Assert.Fail();
            }
            //context.Entry(person.Subscriptions.ToList()[0]).Reference(c => c.Topic).Load();

            Assert.AreEqual(2, person.Subscriptions.Count);
            Assert.AreEqual(5, person.Subscriptions.ToList()[0].TopicId);
            Assert.AreEqual("topics", person.Subscriptions.ToList()[0].Topic.Name);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdatePerson()
        {
            var person = await context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).SingleOrDefaultAsync(m => m.Id == 4);
            var topic = await context.Topics.SingleOrDefaultAsync(m => m.Id == 1);

            List<Topic> newTopics = new List<Topic>();
            newTopics.Add(topic);

            var deletedTopics = person.Subscriptions.Where(x => !newTopics.Contains(x.Topic));
            var addedTopics = newTopics.Except(person.Subscriptions.Select(o => o.Topic));

            foreach (Subscription subscription in deletedTopics)
            {
                person.Subscriptions.Remove(subscription);
            }
            foreach (Topic newTopic in addedTopics)
            {
                //if (context.Entry(newTopic).State == EntityState.Detached)
                //    context.Topics.Attach(newTopic);
                person.Subscriptions.Add(new Subscription() { Person = person, Topic = newTopic });
            }
            context.SaveChanges();

        }





        }
}
