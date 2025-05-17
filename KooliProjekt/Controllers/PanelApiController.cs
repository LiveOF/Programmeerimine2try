using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PanelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Panel
        [HttpGet]
        public IActionResult Get()
        {
            var panels = _context.Panel.ToList();
            return Ok(panels);
        }

        // GET: api/Panel/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var panel = _context.Panel.Find(id);
            if (panel == null)
            {
                return NotFound();
            }
            return Ok(panel);
        }

        // POST: api/Panel
        [HttpPost]
        public IActionResult Post([FromBody] Panel panel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Panel.Add(panel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = panel.Id }, panel);
        }

        // PUT: api/Panel/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Panel panel)
        {
            if (id != panel.Id)
            {
                return BadRequest();
            }

            var existingPanel = _context.Panel.Find(id);
            if (existingPanel == null)
            {
                return NotFound();
            }

            existingPanel.Title = panel.Title;
            existingPanel.Description = panel.Description;
            existingPanel.Cost = panel.Cost;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Panel/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var panel = _context.Panel.Find(id);
            if (panel == null)
            {
                return NotFound();
            }

            // Проверяем наличие связанных записей в BuildingPanels
            var hasReferences = _context.BuildingPanels.Any(bp => bp.PanelId == id);
            if (hasReferences)
            {
                return Conflict("The panel cannot be deleted because there are matching entries in BuildingPanels.");
            }

            _context.Panel.Remove(panel);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
