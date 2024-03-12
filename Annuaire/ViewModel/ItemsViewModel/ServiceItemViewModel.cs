using Annuaire.Command;
using Annuaire.Services;
using AnnuaireLib.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Annuaire.ViewModel.ItemsViewModel
{
    public class ServiceItemViewModel : BaseViewModel
    {
        public Service Service { get; set; }

		private Visibility _modifVisibility = Visibility.Hidden;

		public Visibility ModifVisibility
		{
			get { return _modifVisibility; }
			set
			{
				_modifVisibility = value;
				OnPropertyChanged(nameof(ModifVisibility));
			}
		}

		private Visibility _displayVisibility = Visibility.Visible;

		public Visibility DisplayVisibility
		{
			get { return _displayVisibility; }
			set
			{
				_displayVisibility = value;
				OnPropertyChanged(nameof(DisplayVisibility));
			}
		}

		public event EventHandler EH_Deleted;

		public ICommand ModifCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand ValidModifCommand { get; set; }

		public ServiceItemViewModel(Service _service)
		{
			Service = _service;
			ModifCommand = new RelayCommand(OuvrirModifier);
			ValidModifCommand = new RelayCommand(ValiderModification);
			DeleteCommand = new RelayCommand(Supprimer);

		}

		private void Supprimer(object obj)
		{
			Task.Run(async () =>
			{

				return await HttpClientService.GetSalariesFromService(this.Service.Id);

			}).ContinueWith(t =>
			{
				if (t.Result.Any())
				{
					MessageBox.Show("Suppression impossible : Des salariés sont reliés à ce services.", "Erreur suppression");
					return;
				}
				else
				{
					MessageBoxResult answer = MessageBox.Show("Etes vous sûr de vouloir supprimer ce service ? Cette suppression sera définitive.", "Suppression d'un produit", MessageBoxButton.YesNo, MessageBoxImage.Question);

					if (answer == MessageBoxResult.Yes)
					{

						Task.Run(async () =>
						{
							return await HttpClientService.DeleteService(this.Service.Id);

						}).ContinueWith(t =>
						{
							if (t.Result == true)
							{
								MessageBox.Show("Le Service a été supprimé avec succès.");
								invoke_DeleteFromListe(this);

							}
							else
								MessageBox.Show("Suppression impossible.");


						}, TaskScheduler.FromCurrentSynchronizationContext());
					}
					else
						return;

				}

			}, TaskScheduler.FromCurrentSynchronizationContext());

		}

		private void ValiderModification(object obj)
		{
			if (this.Service.Name == "")
			{
				MessageBox.Show("Vous ne pouvez pas enregistrer un nom de Service vide.", "Erreur");
				return;
			}

			Task.Run(async () =>
			{

				return await HttpClientService.ModifyService(this.Service, this.Service.Id);

			}).ContinueWith(t =>
			{
				if (t.Result == false)
				{
					MessageBox.Show("Modification impossible.");
				}
				else
				{
					MessageBox.Show("Service modifié");
					DisplayVisibility = Visibility.Visible;
					ModifVisibility = Visibility.Hidden;

				}

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		protected void invoke_DeleteFromListe(object sender)
		{
			EH_Deleted?.Invoke(sender, EventArgs.Empty);
		}

		private void OuvrirModifier(object obj)
		{
			DisplayVisibility = Visibility.Collapsed;
			ModifVisibility = Visibility.Visible;
		}
	}
}

