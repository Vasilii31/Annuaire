using Annuaire.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Annuaire.ViewModel
{
	public class AdminViewModel : BaseViewModel
	{
		public EventHandler RetourButtonClick;

		public ICommand ConnexionCommand { get; set; }
        public ICommand RetourCommand { get; set; }

		public AdminViewModel()
        {
            ConnexionCommand = new RelayCommand(Connexion);
			RetourCommand = new RelayCommand(Retour);
        }

		private void Connexion(object obj)
		{
			MessageBox.Show("Connexion");
		}

		private void Retour(object obj)
		{
			RetourButtonClick?.Invoke(this, EventArgs.Empty);
		}
	}
}
