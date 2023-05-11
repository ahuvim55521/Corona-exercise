using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ahuvi.Model;

namespace ahuvi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImmunizationsController : ControllerBase
    {
        private readonly AhuviContext _context;

        public ImmunizationsController(AhuviContext context)
        {
            _context = context;
        }

        // GET: api/Immunizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Immunization>>> GetImmunization()
        {
            return await _context.Immunization.ToListAsync();
        }

        // GET: api/Immunizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Immunization>> GetImmunization(int id)
        {
            var immunization = await _context.Immunization.FindAsync(id);

            if (immunization == null)
            {
                return NotFound();
            }

            return immunization;
        }

     

        // POST: api/Immunizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Immunization>> PostImmunization(Immunization immunization)
        {
            Insured insured = await _context.Insured
                .Include(i=>i.Immunizations)
                .Where(i => i.InsuredId == immunization.InsuredId)
                .FirstOrDefaultAsync();

            if (insured == null)
            {
                return StatusCode(500, "המבוטח אינו קיים");
            }
            if (insured.Immunizations.Count == 4)
            {
                return StatusCode(500, "אין אפשרות להתחסן יותר מ4 חיסונים");
            }

            if (immunization.ImmunizationDate < insured.DateBirthDay)
            {
                return StatusCode(500, "תאריך חיסון קטן מתאריך הלידה.");
            }

            //כדי שלא יוכלו לערוך את הנתונים בטבלה של היצרנים
            immunization.Manufacturer = null;
            _context.Immunization.Add(immunization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImmunization", new { id = immunization.ImmunizationId }, immunization);
        }

        // DELETE: api/Immunizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImmunization(int id)
        {
            var immunization = await _context.Immunization.FindAsync(id);
            if (immunization == null)
            {
                return NotFound();
            }

            _context.Immunization.Remove(immunization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImmunizationExists(int id)
        {
            return _context.Immunization.Any(e => e.ImmunizationId == id);
        }
    }
}
