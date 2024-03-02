using AnnuaireAPI.Services.Interfaces;
using AnnuaireLib.Context;
using AnnuaireLib.DAO;

namespace AnnuaireAPI.Services
{
	public class SeedService : ISeedService
	{
		private readonly AnnuaireDbContext _context;

		public SeedService(AnnuaireDbContext context)
		{
			_context = context;
		}

		public void SeedSites()
		{
			//sites
			_context.Site.Add(new Site { Town = "Paris" });
			_context.Site.Add(new Site { Town = "Nantes" });
			_context.Site.Add(new Site { Town = "Toulouse" });
			_context.Site.Add(new Site { Town = "Lille" });
			_context.Site.Add(new Site { Town = "Nice" });
			_context.SaveChanges();
		}

		public void SeedServices()
		{
			//services
			_context.Service.Add(new Service { Name = "Comptabilité" });
			_context.Service.Add(new Service { Name = "Marketing" });
			_context.Service.Add(new Service { Name = "Production" });
			_context.Service.Add(new Service { Name = "Ressources Humaines" });
			_context.Service.Add(new Service { Name = "Informatique" });
			_context.Service.Add(new Service { Name = "Commercial" });
			_context.Service.Add(new Service { Name = "Accueil" });
			_context.SaveChanges();
		}

		public void SeedSalaries()
		{
			// Paris, Administratif
			_context.Salarie.Add(new Salarie { Name = "Ballourdu", Surname = "Edward", CellPhoneNumber = "0785656789", Email = "truc@truc.fr", PhoneNumber = "0529102910", SiteId = 1, ServiceId = 6 });
			_context.Salarie.Add(new Salarie { Name = "Francisco", Surname = "Thomas", CellPhoneNumber = "0785657779", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 1, ServiceId = 6 });
			_context.Salarie.Add(new Salarie { Name = "Vaugirard", Surname = "Virginie", CellPhoneNumber = "0765453422", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 1, ServiceId = 4 });
			_context.Salarie.Add(new Salarie { Name = "Populos", Surname = "Sébastian", CellPhoneNumber = "0654346789", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 1, ServiceId = 2 });

			// Nantes, Production

			_context.Salarie.Add(new Salarie { Name = "Des", Surname = "Henri", CellPhoneNumber = "0765453433", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 2, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Colon", Surname = "Luc", CellPhoneNumber = "0657890909", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 2, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Balar", Surname = "Nicolaj", CellPhoneNumber = "0743322112", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 2, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Dupres", Surname = "Emile", CellPhoneNumber = "0743322112", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 2, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Polifa", Surname = "Ludivine", CellPhoneNumber = "0976432334", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 2, ServiceId = 3 });

			//Toulouse , Production

			_context.Salarie.Add(new Salarie { Name = "Mol", Surname = "Tiffany", CellPhoneNumber = "0766665555", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 3, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Parten", Surname = "John", CellPhoneNumber = "0654324976", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 3, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Bulir", Surname = "Eliza", CellPhoneNumber = "0678987656", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 3, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Convenant", Surname = "Ulfric", CellPhoneNumber = "0564334442", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 3, ServiceId = 3 });

			//Lille, Production

			_context.Salarie.Add(new Salarie { Name = "Iknea", Surname = "Maria", CellPhoneNumber = "0976432334", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 4, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Poaze", Surname = "Jules", CellPhoneNumber = "0976432334", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 4, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Huji", Surname = "Tanaka", CellPhoneNumber = "0976432334", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 4, ServiceId = 3 });
			_context.Salarie.Add(new Salarie { Name = "Huji", Surname = "Sashi", CellPhoneNumber = "0976432334", Email = "truc@truc.fr", PhoneNumber = "0529109432", SiteId = 4, ServiceId = 3 });

			_context.SaveChanges();
		}

	}
}
