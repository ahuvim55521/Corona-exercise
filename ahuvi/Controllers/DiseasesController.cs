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
    public class DiseasesController : ControllerBase
    {
        private readonly AhuviContext _context;

        public DiseasesController(AhuviContext context)
        {
            _context = context;
        }

        // GET: api/Diseases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Disease>>> GetDisease()
        {
            return await _context.Disease.ToListAsync();
        }

        // GET: api/Diseases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Disease>> GetDisease(int id)
        {
            var disease = await _context.Disease.FindAsync(id);

            if (disease == null)
            {
                return NotFound();
            }

            return disease;
        }

      

        // POST: api/Diseases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Disease>> PostDisease(Disease disease)
        {
            var exist = await _context.Disease.Where(d => d.InsuredId == disease.InsuredId).AnyAsync();
            //אם רוצים להגביל את כמות הפעמים שאפשר לחלות 
            if (exist)
            {
                return StatusCode(500, "אין אפשרות להדבק יותר מפעם אחת");
            }

            if (disease.Recovery < disease.Start)
            {
                return StatusCode(500, "תאריך החלמה קטן מתאריך התחלה");
            }
            _context.Disease.Add(disease);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisease", new { id = disease.DiseaseId }, disease);
        }

        // DELETE: api/Diseases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var disease = await _context.Disease.FindAsync(id);
            if (disease == null)
            {
                return NotFound();
            }

            _context.Disease.Remove(disease);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiseaseExists(int id)
        {
            return _context.Disease.Any(e => e.DiseaseId == id);
        }
    }
}
