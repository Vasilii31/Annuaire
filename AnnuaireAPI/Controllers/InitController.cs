using AnnuaireAPI.Services;
using AnnuaireAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnnuaireAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InitController : ControllerBase
	{

		private readonly IRoleService _rolesService;
		private readonly ISeedService _seedService;

		public InitController(IRoleService rolesService, ISeedService seedService)
		{
			_rolesService = rolesService;
			_seedService = seedService;
		}

		[HttpGet]
		public async Task InitAll()
		{
			await _rolesService.CreateRolesAsync();
			//_seedService.SeedSites();
			//_seedService.SeedServices();
			_seedService.SeedSalaries();
		}

	}
}
