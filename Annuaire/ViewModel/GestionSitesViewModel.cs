
using Annuaire.Command;
using Annuaire.Services;
using Annuaire.ViewModel.ItemsViewModel;
using AnnuaireLib.DAO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Annuaire.ViewModel
{
    public class GestionSitesViewModel : BaseViewModel
    {
        public ObservableCollection<SiteItemViewModel> ListeItems { get; set; } = new();
		
		private string _newSiteName = string.Empty;
		public string NewSiteName
		{
			get { return _newSiteName; }
			set { 
				_newSiteName = value; 
				OnPropertyChanged(nameof(NewSiteName));	
			}
		}
		
		public ICommand CreateNewSiteCommand { get; set; }

		public GestionSitesViewModel()
        {
            GetSites();
			CreateNewSiteCommand = new RelayCommand(CreateSite);
        }

		private void CreateSite(object obj)
		{
			if (NewSiteName == "")
			{
				MessageBox.Show("Vous ne pouvez pas enregistrer un nom de site vide.", "Erreur");
				return;
			}

			var newsite = new Site { Town = NewSiteName };

			Task.Run(async () =>
			{

				return await HttpClientService.CreateNewSite(newsite);

			}).ContinueWith(t =>
			{
				if (t.Result == null)
				{
					MessageBox.Show("Création impossible.");
				}
				else
				{
					MessageBox.Show("Site créé");
					NewSiteName = "";
					GetSites();
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());

		}

		private void GetSites()
		{
			ListeItems.Clear();
			
			Task.Run(async () =>
			{
				return await HttpClientService.GetSites();

			})
			.ContinueWith(t =>
			{
				foreach (var item in t.Result)
				{
					var siteVM = new SiteItemViewModel(item);
					siteVM.EH_Deleted += Reload;
					ListeItems.Add(siteVM);
					//ListeSalarieFromSearch.Add(item);
				}
				

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void Reload(object? sender, EventArgs e)
		{
			GetSites();
		}
	}
}
