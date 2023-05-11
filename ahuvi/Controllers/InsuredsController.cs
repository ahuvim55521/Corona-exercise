using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ahuvi.Model;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace ahuvi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuredsController : ControllerBase
    {
        private readonly AhuviContext _context;

        public InsuredsController(AhuviContext context)
        {
            _context = context;
        }

        // GET: api/Insureds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Insured>>> GetInsured()
        {
            return await _context.Insured
                .Include(i => i.Diseases)
                .Include(i => i.Immunizations)
                .ThenInclude(i => i.Manufacturer)
                .ToListAsync();
        }

        // GET: api/Insureds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Insured>> GetInsured(int id)
        {
            var insured = await _context.Insured
                .Include(i => i.Diseases)
                .Include(i => i.Immunizations)
                .ThenInclude(i => i.Manufacturer)
                .Where(i => i.InsuredId == id)
                .FirstOrDefaultAsync();

            if (insured == null)
            {
                return NotFound();
            }

            return insured;
        }

        // POST: api/Insureds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Insured>> PostInsured(Insured insured)
        {
            var exist = await _context.Insured.Where(i => i.IDnumber == insured.IDnumber).AnyAsync();
            if (exist)
            {
                return StatusCode(500, "מבוטח קיים");
            }
            if (insured.Immunizations.Count > 4)
            {
                return StatusCode(500, "אין אפשרות להתחסן יותר מ4 חיסונים");
            }
            //אם רוצים להגביל את כמות הפעמים שאפשר לחלות 
            if (insured.Diseases.Count > 1)
            {
                return StatusCode(500, "אין אפשרות להדבק יותר מפעם אחת");
            }
            if (insured.DateBirthDay > DateTime.Now)
            {
                return StatusCode(500, "תאריך לידה שגוי");
            }

            var err = "";
            insured.Immunizations.ForEach(i =>
            {
                if (i.ImmunizationDate < insured.DateBirthDay)
                {
                    err = "תאריך חיסון קטן מתאריך הלידה.";
                }
            });

            if (err != "")
            {
                return StatusCode(500, err);
            }

            ///כדי שלא ייצר בדטה בייס יצרנים חדשים
            insured.Immunizations.ForEach(i => i.Manufacturer = null);

            _context.Insured.Add(insured);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsured", new { id = insured.InsuredId }, insured);
        }

        // DELETE: api/Insureds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsured(int id)
        {
            var insured = await _context.Insured.FindAsync(id);
            if (insured == null)
            {
                return NotFound();
            }

            _context.Insured.Remove(insured);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFile([FromQuery] int insuredId,
    IFormFile file
    )

        {
            var insured = await _context.Insured.Where(i => i.InsuredId == insuredId).FirstOrDefaultAsync();
            if (insured != null)
            {

                var pathBuilt = "";
                pathBuilt = await WriteFile(file, insuredId);

                insured.PicturePath = pathBuilt;
                _context.SaveChanges();
                return Ok();
            }
            return StatusCode(500, "מבוטח לא קיים");
        }



        private async Task<string> WriteFile(IFormFile file, int userID)

        {
            string fileName = userID.ToString() + file.FileName;
            string pathBuilt = "";
            try
            {
                pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                try

                {
                    if (!Directory.Exists(pathBuilt))
                    {
                        Directory.CreateDirectory(pathBuilt);
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                  fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }
            }
            catch (Exception e)
            {
                throw;
            }
            return fileName;// isSaveSuccess;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFile([FromQuery] int insuredId)
        {
            //שליפה של המבוטח
            var insured = await _context.Insured.FindAsync(insuredId);
            var a = "Upload\\files\\" + insured.PicturePath;

            var fileFullPath = Path.Combine(Directory.GetCurrentDirectory(), a);

            if (!System.IO.File.Exists(fileFullPath))

                return NotFound();

            var memory = new MemoryStream();

            await using (var stream = new FileStream(fileFullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;


            return File(memory, GetContentType(fileFullPath));

        }





        private string GetContentType(string path)

        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;

        }


    }
}
