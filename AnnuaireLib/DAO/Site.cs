using System.ComponentModel.DataAnnotations;

namespace AnnuaireLib.DAO
{
	public class Site
	{
		[Required]
		public int Id { get; set; }
		public required string Town { get; set; }
	}
}
