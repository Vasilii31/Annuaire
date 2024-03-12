using AnnuaireLib.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnuaireLib.DTO
{
	public class SalarieCreate
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Surname { get; set; }
		public string PhoneNumber { get; set; } = string.Empty;
		public string CellPhoneNumber { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public Service? Service { get; set; }	
		public Site? Site { get; set; }

	}
}
