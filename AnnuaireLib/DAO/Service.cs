using System.ComponentModel.DataAnnotations;

namespace AnnuaireLib.DAO
{
	public class Service
	{
		[Required]
		public int Id { get; set; }
		public required string Name { get; set; }


	}
}
