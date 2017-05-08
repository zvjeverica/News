using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Models;
using NewsService.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace NewsService.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly NewsletterDBContext _context;

        public PeopleController(NewsletterDBContext context)
        {
            _context = context;    
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            PersonViewModel personViewModel = new PersonViewModel();
            var allTopicsList = _context.Topics.ToList();
            personViewModel.AllTopics = allTopicsList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString(),
            });
            return View(personViewModel);
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonViewModel personViewModel)
        {
            if (ModelState.IsValid)
            {
                var newTopics = personViewModel.SelectedJobTags;
                foreach (int newTopic in newTopics)
                {
                    personViewModel.Person.Subscriptions.Add(new Subscription() { Person = personViewModel.Person, TopicId = newTopic });
                }
                _context.Add(personViewModel.Person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(personViewModel.Person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personViewModel = new PersonViewModel
            {
                Person = await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).SingleOrDefaultAsync(m => m.Id == id),
            };               
            if (personViewModel.Person == null)
            {
                return NotFound();
            }
            var allTopicsList = _context.Topics.ToList();
            personViewModel.AllTopics = allTopicsList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.Id.ToString(),
            });

            return View(personViewModel);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PersonViewModel personViewModel)
        {
            if (id != personViewModel.Person.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingPerson = await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).SingleOrDefaultAsync(m => m.Id == personViewModel.Person.Id);
                    var newTopics = personViewModel.SelectedJobTags;
                    var deletedTopics = existingPerson.Subscriptions.Where(x => !newTopics.Contains(x.Topic.Id)).ToList();
                    var addedTopics = newTopics.Except(existingPerson.Subscriptions.Select(o => o.TopicId));

                    foreach (Subscription subscription in deletedTopics)
                    {
                        existingPerson.Subscriptions.Remove(subscription);
                    }
                    foreach (int newTopic in addedTopics)
                    {
                        existingPerson.Subscriptions.Add(new Subscription() { Person = personViewModel.Person, TopicId = newTopic });
                    }
                    existingPerson.FirstName = personViewModel.Person.FirstName;
                    existingPerson.LastName = personViewModel.Person.LastName;
                    existingPerson.Telephone = personViewModel.Person.Telephone;
                    existingPerson.EMail = personViewModel.Person.EMail;
                    _context.Update(existingPerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(personViewModel.Person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(personViewModel.Person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.SingleOrDefaultAsync(m => m.Id == id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
