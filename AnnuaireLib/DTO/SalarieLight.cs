using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnuaireLib.DTO
{
	public class SalarieLight
	{
		public int Id { get; set; }
		public required string Name { get; set; }

		public required string Surname { get; set; }
	}
}
