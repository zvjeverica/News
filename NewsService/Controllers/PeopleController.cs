using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Data;
using BusinessLogic.Repositories;

namespace NewsService.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private PersonRepository _repository;
        private TopicRepository _repositoryTopics;

        public PeopleController(NewsletterDBContext context)
        {
            _repository = new PersonRepository(context);
            _repositoryTopics = new TopicRepository(context);
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllPeople());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _repository.GetPersonById(id.Value);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public async Task<IActionResult> Create()
        {
            PersonViewModel personViewModel = new PersonViewModel();
            var allTopicsList = await _repositoryTopics.GetAllTopics();
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
                await Task.Run(() => _repository.CreatePerson(personViewModel.Person, personViewModel.SelectedTopics));
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
                Person = await _repository.GetPersonById(id.Value),
            };               
            if (personViewModel.Person == null)
            {
                return NotFound();
            }
            var allTopicsList = await _repositoryTopics.GetAllTopics();
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
                if (await _repository.EditPerson(personViewModel.Person, personViewModel.SelectedTopics))
                {
                    return RedirectToAction("Index");
                }
                else
                    return NotFound();
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
            var person = await _repository.GetPersonById(id.Value);
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
            await Task.Run(() => _repository.DeletePerson(id));
            return RedirectToAction("Index");
        }

    }
}
