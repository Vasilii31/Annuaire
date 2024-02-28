using AnnuaireLib.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnnuaireLib
{
	public class AnnuaireDbContext(DbContextOptions<AnnuaireDbContext> options) : DbContext
	{
		public DbSet<Salarie> Salarie { get; set; }
		public DbSet<Service> Service { get; set; }
		public DbSet<Site> Site { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connexionString = "server=localhost;port=3306;userid=root;password=;database=AnnuaireDb;";
			optionsBuilder.UseMySql(connexionString, ServerVersion.AutoDetect(connexionString));
		}
	}




	public class AnnuaireDbContextFactory : IDesignTimeDbContextFactory<AnnuaireDbContext>
	{
		public AnnuaireDbContext CreateDbContext(string[] args)
		{
			var connexionString = "server=localhost;port=3306;userid=root;password=;database=AnnuaireDB;";
			var optionsBuilder = new DbContextOptionsBuilder<AnnuaireDbContext>();
			optionsBuilder.UseMySql(connexionString, ServerVersion.AutoDetect(connexionString));

			return new AnnuaireDbContext(optionsBuilder.Options);
		}
	}

}
