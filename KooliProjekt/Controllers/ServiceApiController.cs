using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Service
        [HttpGet]
        public IActionResult Get()
        {
            var services = _context.Service.ToList();
            return Ok(services);
        }

        // GET: api/Service/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var service = _context.Service.Find(id);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }

        // POST: api/Service
        [HttpPost]
        public IActionResult Post([FromBody] Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Проверяем, существует ли здание с таким BuildingId
            var buildingExists = _context.Building.Any(b => b.Id == service.BuildingId);
            if (!buildingExists)
            {
                return BadRequest($"Building with Id={service.BuildingId} not found.");
            }

            _context.Service.Add(service);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = service.Id }, service);
        }

        // PUT: api/Service/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            var existingService = _context.Service.Find(id);
            if (existingService == null)
            {
                return NotFound();
            }

            existingService.Title = service.Title;
            existingService.Description = service.Description;
            existingService.Price = service.Price;
            existingService.BuildingId = service.BuildingId;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Service/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var service = _context.Service.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Service.Remove(service);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
