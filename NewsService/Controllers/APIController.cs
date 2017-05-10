using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Repositories;
using BusinessLogic.Data;

namespace NewsService.Controllers
{

    [Produces("application/json")]
    [Route("api")]
    public class APIController : Controller
    {
        private ApiRepository _repository;

        public APIController(NewsletterDBContext context)
        {
            _repository = new ApiRepository(context);
        }

        //GET: api/Subscribers?topic="meh"&topic="bla"
        [HttpGet("Subscribers")]
        public IActionResult GetSubscribers([FromQuery] string[] topic)
        {
            if (User.Identity.IsAuthenticated)
            {
                var people = _repository.GetSubscribers(topic);

                if (people == null)
                {
                    return NotFound();
                }
                return Ok(people);
            }
            else return StatusCode(418);
        }

        // GET: api/Persons
        [HttpGet("Persons")]
        public async Task<IActionResult> GetPeopleAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var people = await _repository.GetAllPeople();

                return Ok(people);
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

                var person = await _repository.GetPersonById(id);

                if (person == null)
                {
                    return NotFound();
                }
                return Ok(person);
            }
            else return StatusCode(418);
        }
    }
}