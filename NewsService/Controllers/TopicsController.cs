using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Models;
using Microsoft.AspNetCore.Authorization;

namespace NewsService.Controllers
{
    [Authorize]
    public class TopicsController : Controller
    {
        //private readonly NewsletterDBContext _context;
        private DBRepository _repository;

        public TopicsController(NewsletterDBContext context)
        {
            _repository = new DBRepository(context);    
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllTopics());
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _repository.GetTopicById(id.Value);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => _repository.CreateTopic(topic));
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _repository.GetTopicById(id.Value);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Topic topic)
        {
            if (id != topic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await _repository.EditTopic(topic))
                    return RedirectToAction("Index");
                else return NotFound();
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _repository.GetTopicById(id.Value);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Task.Run(() => _repository.DeleteTopic(id));
            return RedirectToAction("Index");
        }
    }
}
