using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnnuaireLib.DAO;
using AnnuaireLib.Context;
using AnnuaireLib.Extensions;
using AnnuaireLib.DTO;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authorization;

namespace AnnuaireAPI.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class SalariesController : ControllerBase
	{
		private readonly AnnuaireDbContext _context;

		public SalariesController(AnnuaireDbContext context)
		{
			_context = context;
		}

		// GET: api/Salaries
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SalarieLight>>> GetSalarie()
		{
			var test = await _context.Salarie
				.Include(x => x.Service)
				.Include(x => x.Site)
				.OrderBy(x => x.Name)
				.Select(p => p.ToLightDTO())
				.ToListAsync();
			return test;
		}

		[HttpGet("PerSite/{id}")]
		public async Task<ActionResult<IEnumerable<SalarieLight>>> GetSalariePerSite(int id)
		{
			var test = await _context.Salarie
				.Where(x => x.SiteId == id)
				.Include(x => x.Service)
				.Include(x => x.Site)
				.OrderBy(x => x.Name)
				.Select(p => p.ToLightDTO())
				.ToListAsync();
			return test;
		}

		[HttpGet("PerServices/{id}")]
		public async Task<ActionResult<IEnumerable<SalarieLight>>> GetSalariePerService(int id)
		{
			var test = await _context.Salarie
				.Where(x => x.ServiceId == id)
				.Include(x => x.Service)
				.Include(x => x.Site)
				.OrderBy(x => x.Name)
				.Select(p => p.ToLightDTO())
				.ToListAsync();
			return test;
		}

		// GET: api/Salaries/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Salarie>> GetSalarie(int id)
		{
			var salarie = await _context.Salarie
				.Include(x => x.Service)
				.Include(x => x.Site)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();


			if (salarie == null)
			{
				return NotFound();
			}

			return salarie;
		}

		// PUT: api/Salaries/5
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutSalarie(int id, Salarie salarie)
		{
			if (id != salarie.Id)
			{
				return BadRequest();
			}

			_context.Entry(salarie).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SalarieExists(id))
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

		// POST: api/Salaries

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult<Salarie>> PostSalarie(Salarie salarie)
		{

			_context.Salarie.Add(salarie);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetSalarie", new { id = salarie.Id }, salarie);
		}

		// DELETE: api/Salaries/5
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSalarie(int id)
		{
			var salarie = await _context.Salarie.FindAsync(id);
			if (salarie == null)
			{
				return NotFound();
			}

			_context.Salarie.Remove(salarie);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool SalarieExists(int id)
		{
			return _context.Salarie.Any(e => e.Id == id);
		}
	}
}
