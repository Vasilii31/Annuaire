using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnnuaireLib.DAO
{
	public class Salarie
	{
		[Required]
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Surname { get; set; }
		public string PhoneNumber { get; set; } = string.Empty;
		public string CellPhoneNumber { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;

		[ForeignKey(nameof(Service))]
		public int ServiceId { get; set; }
		public virtual Service Service { get; set; } = null!;

		[ForeignKey(nameof(Site))]
		public int SiteId { get; set; }
		public virtual Site Site { get; set; } = null!;
	}
}
