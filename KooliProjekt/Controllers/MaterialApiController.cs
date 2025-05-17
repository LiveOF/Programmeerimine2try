using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaterialController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Material
        [HttpGet]
        public IActionResult Get()
        {
            var materials = _context.Material.ToList();
            return Ok(materials);
        }

        // GET: api/Material/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var material = _context.Material.Find(id);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }

        // POST: api/Material
        [HttpPost]
        public IActionResult Post([FromBody] Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Material.Add(material);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = material.Id }, material);
        }

        // PUT: api/Material/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Material material)
        {
            if (id != material.Id)
            {
                return BadRequest();
            }

            var existingMaterial = _context.Material.Find(id);
            if (existingMaterial == null)
            {
                return NotFound();
            }

            existingMaterial.Title = material.Title;
            existingMaterial.Description = material.Description;
            existingMaterial.UnitPrice = material.UnitPrice;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Material/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var material = _context.Material.Find(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Material.Remove(material);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
