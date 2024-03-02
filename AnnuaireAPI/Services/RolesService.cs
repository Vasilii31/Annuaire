using AnnuaireAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AnnuaireAPI.Services
{
	public class RolesService : IRoleService
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RolesService(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public async Task CreateRolesAsync()
		{
			string[] roleNames = { "Admin", "Visiteur" };

			foreach (var roleName in roleNames)
			{
				var roleExist = await _roleManager.RoleExistsAsync(roleName);

				if (!roleExist)
				{
					// Créer le rôle s'il n'existe pas
					await _roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
		}
	}
}
