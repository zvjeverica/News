using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsService.Data;
using NewsService.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace NewsService.Controllers
{
    
    [Produces("application/json")]
    [Route("api")]
    public class APIController : Controller
    {
        private readonly NewsletterDBContext _context;

        public APIController(NewsletterDBContext context)
        {
            _context = context;
        }

        //GET: api/Subscribers?topic="meh"&topic="bla"
        [HttpGet("Subscribers")]
        public IActionResult GetSubscribers([FromQuery] string[] topic)
        {
            if (User.Identity.IsAuthenticated)
            {
                var people = (from p in _context.People
                              join s in _context.Subscriptions on p.Id equals s.PersonId
                              join t in _context.Topics on s.TopicId equals t.Id
                              where topic.Any(x => x.Equals(t.Name, StringComparison.OrdinalIgnoreCase))
                              select p).Include(x => x.Subscriptions).ThenInclude(x => x.Topic).Distinct();

                if (people == null)
                {
                    return NotFound();
                }
                return Ok(PeopleToJson(people));
            }
            else return StatusCode(418);

        }

        // GET: api/Persons
        [HttpGet("Persons")]
        public IActionResult GetPeople()
        {
            if (User.Identity.IsAuthenticated)
            {
                var people = _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic);
                
                return Ok(PeopleToJson(people));
            }
                
            else return StatusCode(418);
        }

        // GET: api/Person/5
        [HttpGet("Person/{id}")]
        public async Task<IActionResult> GetPerson([FromRoute] int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var person = await _context.People.Include(x => x.Subscriptions).ThenInclude(x => x.Topic).SingleOrDefaultAsync(m => m.Id == id);

                if (person == null)
                {
                    return NotFound();
                }

                return Ok(person.ToJson());
            }
            else return StatusCode(418);
        }

        private string PeopleToJson (IEnumerable<Person> people)
        {
            //If serilization doesn't want to be your friend you have to do it yourself
            string json = "[";
            bool any = false;
            foreach (Person person in people)
            {
                json += person.ToJson() + ", ";
                any = true;
            }
            if (any)
                json = json.Substring(0, json.Length - 2) + "]";
            else json = "[]";
            return json;
        }
    }
}