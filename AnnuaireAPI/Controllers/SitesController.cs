using AnnuaireLib;
using AnnuaireLib.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnuaireAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SitesController : ControllerBase
	{
		private readonly AnnuaireDbContext _context;

		public SitesController(AnnuaireDbContext context)
		{
			_context = context;
		}

		// GET: api/Sites
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Site>>> GetSite()
		{
			return await _context.Site.ToListAsync();
		}

		// GET: api/Sites/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Site>> GetSite(int id)
		{
			var site = await _context.Site.FindAsync(id);

			if (site == null)
			{
				return NotFound();
			}

			return site;
		}

		// PUT: api/Sites/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutSite(int id, Site site)
		{
			if (id != site.Id)
			{
				return BadRequest();
			}

			_context.Entry(site).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SiteExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Sites
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Site>> PostSite(Site site)
		{
			_context.Site.Add(site);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetSite", new { id = site.Id }, site);
		}

		// DELETE: api/Sites/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSite(int id)
		{
			var site = await _context.Site.FindAsync(id);
			if (site == null)
			{
				return NotFound();
			}

			_context.Site.Remove(site);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool SiteExists(int id)
		{
			return _context.Site.Any(e => e.Id == id);
		}
	}
}
