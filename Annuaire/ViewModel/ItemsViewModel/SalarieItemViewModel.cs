using Annuaire.Command;
using Annuaire.Services;
using AnnuaireLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Annuaire.ViewModel.ItemsViewModel
{
	public class SalarieItemViewModel : BaseViewModel
	{
		public SalarieLight SalarieLight { get; set; }

		public event EventHandler EH_ConsultSalarie;

		public ICommand ConsultSalarieCommand {  get; set; }
		public ICommand DeleteSalarieCommand { get; set; }
		public string FullName { get; set; }

		private Visibility _adminContentVisibility = Visibility.Hidden;

		public Visibility AdminContentVisibility
		{
			get { return _adminContentVisibility; }
			set
			{
				_adminContentVisibility = value;
				OnPropertyChanged(nameof(AdminContentVisibility));
			}
		}

		public event EventHandler EH_Deleted;
		public SalarieItemViewModel(SalarieLight sal) 
		{
			if (App.Current.Properties["status"] == "admin")
			{
				AdminContentVisibility = Visibility.Visible;
			}
			SalarieLight = sal;
			FullName = SalarieLight.Name + " " + SalarieLight.Surname;
			ConsultSalarieCommand = new RelayCommand(ConsultSalarie);
			DeleteSalarieCommand = new RelayCommand(Delete);
		}

		private void Delete(object obj)
		{
			MessageBoxResult answer = MessageBox.Show("Etes vous sûr de vouloir supprimer ce salarié ? Cette suppression sera définitive.", "Suppression d'un produit", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (answer == MessageBoxResult.Yes)
			{

				Task.Run(async () =>
				{
					return await HttpClientService.DeleteSalarie(this.SalarieLight.Id);

				}).ContinueWith(t =>
				{
					if (t.Result == true)
					{
						MessageBox.Show("Le salarie a été supprimé avec succès.");
						invoke_DeleteFromListe(this);

					}
					else
						MessageBox.Show("Suppression impossible.");


				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
			else
				return;
		}
		protected void invoke_DeleteFromListe(object sender)
		{
			EH_Deleted?.Invoke(sender, EventArgs.Empty);
		}

		private void ConsultSalarie(object sender)
		{
			EH_ConsultSalarie?.Invoke(this, EventArgs.Empty);
		}
	}
}
