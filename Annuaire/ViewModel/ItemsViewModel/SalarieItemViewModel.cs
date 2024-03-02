using AnnuaireLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annuaire.ViewModel.ItemsViewModel
{
	public class SalarieItemViewModel : BaseViewModel
	{
		public SalarieLight SalarieLight { get; set; }

		public string FullName { get; set; }
		public SalarieItemViewModel(SalarieLight sal) 
		{ 
			SalarieLight = sal;
			FullName = SalarieLight.Name + " " + SalarieLight.Surname;
		}
	}
}
