using Annuaire.Command;
using Annuaire.Services;
using Annuaire.ViewModel.ItemsViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using AnnuaireLib.DAO;

namespace Annuaire.ViewModel
{
    public class GestionServicesViewModel : BaseViewModel
    {

		public ObservableCollection<ServiceItemViewModel> ListeItems { get; set; } = new();

		private string _newServiceName = string.Empty;
		public string NewServiceName
		{
			get { return _newServiceName; }
			set
			{
				_newServiceName = value;
				OnPropertyChanged(nameof(NewServiceName));
			}
		}

		public ICommand CreateNewServiceCommand { get; set; }

		public GestionServicesViewModel()
		{
			GetServices();
			CreateNewServiceCommand = new RelayCommand(CreateService);
		}

		private void CreateService(object obj)
		{
			if (NewServiceName == "")
			{
				MessageBox.Show("Vous ne pouvez pas enregistrer un nom de site vide.", "Erreur");
				return;
			}

			var newService = new Service { Name = NewServiceName };

			Task.Run(async () =>
			{

				return await HttpClientService.CreateNewService(newService);

			}).ContinueWith(t =>
			{
				if (t.Result == null)
				{
					MessageBox.Show("Création impossible.");
				}
				else
				{
					MessageBox.Show("Service créé");
					NewServiceName = "";
					GetServices();
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());

		}

		private void GetServices()
		{
			ListeItems.Clear();

			Task.Run(async () =>
			{
				return await HttpClientService.GetServices();

			})
			.ContinueWith(t =>
			{
				foreach (var item in t.Result)
				{
					var serviceVM = new ServiceItemViewModel(item);
					serviceVM.EH_Deleted += Reload;
					ListeItems.Add(serviceVM);
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void Reload(object? sender, EventArgs e)
		{
			GetServices();
		}
	}
}
