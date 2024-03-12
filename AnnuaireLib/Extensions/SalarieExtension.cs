using AnnuaireLib.DAO;
using AnnuaireLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnuaireLib.Extensions
{
	public static class SalarieExtension
	{
		public static SalarieLight? ToLightDTO(this Salarie salarie)
		{
			return new SalarieLight { Id = salarie.Id, Name = salarie.Name, Surname = salarie.Surname, Site = salarie.Site.Town, Service = salarie.Service.Name };
		}
	}
}
