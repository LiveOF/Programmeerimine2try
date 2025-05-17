using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/Building")]
    [ApiController]
    public class BuildingApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BuildingApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Building
        [HttpGet]
        public IActionResult Get()
        {
            var buildings = _context.Building.ToList();
            return Ok(buildings);
        }

        // GET: api/Building/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var building = _context.Building.Find(id);
            if (building == null)
            {
                return NotFound();
            }
            return Ok(building);
        }

        // POST: api/Building
        [HttpPost]
        public IActionResult Post([FromBody] Building building)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            building.Id = 0; // Обнуляем ID для автоинкремента

            // Всегда устанавливаем null для UserId
            building.UserId = null;

            _context.Building.Add(building);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = building.Id }, building);
        }

        // PUT: api/Building/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Building building)
        {
            if (id != building.Id)
            {
                return BadRequest();
            }

            var existingBuilding = _context.Building.Find(id);
            if (existingBuilding == null)
            {
                return NotFound();
            }

            existingBuilding.Location = building.Location;
            existingBuilding.Date = building.Date;
            existingBuilding.UserId = building.UserId;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Building/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var building = _context.Building.Find(id);
            if (building == null)
            {
                return NotFound();
            }

            // Удаляем связанные BuildingPanels перед удалением здания
            var relatedPanels = _context.BuildingPanels.Where(bp => bp.BuildingId == id).ToList();
            if (relatedPanels.Any())
            {
                _context.BuildingPanels.RemoveRange(relatedPanels);
            }

            _context.Building.Remove(building);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
