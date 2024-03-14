using Annuaire.Command;
using Annuaire.Services;
using Annuaire.ViewModel.ItemsViewModel;
using AnnuaireLib.DAO;
using AnnuaireLib.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Annuaire.ViewModel
{
	public class AccueilViewModel : BaseViewModel
	{
		public ObservableCollection<SalarieItemViewModel> Salaries { get; set; } = new();
		public ObservableCollection<Site> ListeSite { get; set; } = new();
		public ObservableCollection<Service> ListeService { get; set; } = new();

		public ObservableCollection<SalarieItemViewModel> ListeSalarieFromSearch { get; set; } = new();

		//public ObservableCollection<string> ListeModeRecherche { get; set; } = new ObservableCollection<string>()
		//			{
		//				"Nom",
		//				"Site",
		//				"Service",

		//			};

		//private string _currentSearchMethod = "Nom";
		//public string CurrentSearchMethod
		//{
		//	get { return _currentSearchMethod; }
		//	set
		//	{
		//		_currentSearchMethod = value;
		//		OnPropertyChanged(nameof(CurrentSearchMethod));
		//		ReloadList();
		//	}
		//}

		public ObservableCollection<SalarieItemViewModel> ListeGauche { get; set; } = new();
		public ObservableCollection<SalarieItemViewModel> ListeDroite { get; set; } = new();

		private Salarie _currentSalarie;

		public Salarie CurrentSalarie
		{
			get { return _currentSalarie; }
			set
			{
				_currentSalarie = value;
				OnPropertyChanged(nameof(CurrentSalarie));
			}
		}

		private Visibility _detailsSalarieVisibility = Visibility.Hidden;

		public Visibility DetailsSalarieVisibility
		{
			get { return _detailsSalarieVisibility; }
			set
			{
				_detailsSalarieVisibility = value;
				OnPropertyChanged(nameof(DetailsSalarieVisibility));
			}
		}

		private Visibility _salarieListVisibility = Visibility.Hidden;

		public Visibility SalarieListVisibility
		{
			get { return _salarieListVisibility; }
			set
			{
				_salarieListVisibility = value;
				OnPropertyChanged(nameof(SalarieListVisibility));
			}
		}

		private Visibility _salarieLoadingVisibility = Visibility.Visible;

		public Visibility SalarieLoadingVisibility
		{
			get { return _salarieLoadingVisibility; }
			set
			{
				_salarieLoadingVisibility = value;
				OnPropertyChanged(nameof(SalarieLoadingVisibility));
			}
		}

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

		private Visibility _consultVisibility = Visibility.Visible;

		public Visibility ConsultVisibility
		{
			get { return _consultVisibility; }
			set
			{
				_consultVisibility = value;
				OnPropertyChanged(nameof(ConsultVisibility));
			}
		}

		private Visibility _modifyVisibily = Visibility.Hidden;

		public Visibility ModifyVisibility
		{
			get { return _modifyVisibily; }
			set
			{
				_modifyVisibily = value;
				OnPropertyChanged(nameof(ModifyVisibility));
			}
		}


		private int _totalItems = 0;

		public int TotalItems
		{
			get { return _totalItems; }
			set
			{
				_totalItems = value;
				OnPropertyChanged(nameof(TotalItems));
			}
		}

		private int _numberOfPages = 0;

		public int NumberOfPages
		{
			get { return _numberOfPages; }
			set
			{
				_numberOfPages = value;
				OnPropertyChanged(nameof(NumberOfPages));
			}
		}

		private int _numberOfItemsPerPage = 12;

		public int NumberOfItemsPerPage
		{
			get { return _numberOfItemsPerPage; }
			set
			{
				_numberOfItemsPerPage = value;
				OnPropertyChanged(nameof(NumberOfItemsPerPage));
			}
		}

		private int _currentPage = 1;

		public int CurrentPage
		{
			get { return _currentPage; }
			set
			{
				_currentPage = value;
				OnPropertyChanged(nameof(CurrentPage));
			}
		}

		private string _nameSearchQuery = "";

		public string NameSearchQuery
		{
			get { return _nameSearchQuery; }
			set
			{
				_nameSearchQuery = value;
				OnPropertyChanged(nameof(NameSearchQuery));
			}
		}

		private string _siteSearchQuery = "";

		public string SiteSearchQuery
		{
			get { return _siteSearchQuery; }
			set
			{
				_siteSearchQuery = value;
				OnPropertyChanged(nameof(SiteSearchQuery));
			}
		}

		private string _serviceSearchQuery = "";

		public string ServiceSearchQuery
		{
			get { return _serviceSearchQuery; }
			set
			{
				_serviceSearchQuery = value;
				OnPropertyChanged(nameof(ServiceSearchQuery));
			}
		}

		public bool Ajout { get; set; } = false;

		public ICommand NextPageCommand { get; set; }
		public ICommand PreviousPageCommand { get; set; }
		public ICommand FermerDetailsCommand { get; set; }
		public ICommand OuvrirAjoutSalarieCommad { get; set; }
		public ICommand ValidateSalarieCommand { get; set; }
		public ICommand ModifySalarieCommand { get; set; }

		public AccueilViewModel()
		{
			GetSalaries();
			GetSites();
			GetServices();
			if (App.Current.Properties["status"] == "admin")
			{
				AdminContentVisibility = Visibility.Visible;
			}

			NextPageCommand = new RelayCommand(NextPage);
			PreviousPageCommand = new RelayCommand(PreviousPage);
			FermerDetailsCommand = new RelayCommand(FermerDetails);
			OuvrirAjoutSalarieCommad = new RelayCommand(AjoutSalarie);
			ValidateSalarieCommand = new RelayCommand(Validate);
			ModifySalarieCommand = new RelayCommand(Modifier);
		}

		private void Modifier(object obj)
		{
			ConsultVisibility = Visibility.Hidden;
			ModifyVisibility = Visibility.Visible;
			Ajout = false;
		}

		private void Validate(object obj)
		{
			if (CurrentSalarie.Site != null)
				CurrentSalarie.SiteId = CurrentSalarie.Site.Id;
			if (CurrentSalarie.Service != null)
				CurrentSalarie.ServiceId = CurrentSalarie.Service.Id;
			if (Ajout)
			{
				CreateSalarie();
				
			}
			else
			{
				ModifySalarie();

			}

		}

		private void ModifySalarie()
		{
			if (CurrentSalarie.Name == "")
			{
				MessageBox.Show("Vous ne pouvez pas enregistrer un nom de salarié vide.", "Erreur");
				return;
			}

			if (CurrentSalarie.Surname == "")
			{
				MessageBox.Show("Vous ne pouvez pas enregistrer un nom de salarié vide.", "Erreur");
				return;
			}

			if (CurrentSalarie.Site != null)
			{
				CurrentSalarie.SiteId = CurrentSalarie.Site.Id;
				CurrentSalarie.Site = null;
			}

			if (CurrentSalarie.Service != null)
			{
				CurrentSalarie.ServiceId = CurrentSalarie.Service.Id;
				CurrentSalarie.Service = null;
			}

			Task.Run(async () =>
			{

				return await HttpClientService.ModifySalarie(CurrentSalarie, CurrentSalarie.Id);

			}).ContinueWith(t =>
			{
				if (t.Result == false)
				{
					MessageBox.Show("Modification impossible.");
				}
				else
				{
					MessageBox.Show("Salarié modifié");
					GetSalaries();
					ConsultVisibility = Visibility.Visible;
					ModifyVisibility = Visibility.Hidden;

				}

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void CreateSalarie()
		{
			if (CurrentSalarie.Name == "")
			{
				MessageBox.Show("Vous ne pouvez pas créer un salarié sans nom", "Erreur");
				return;
			}
			if (CurrentSalarie.Surname == "")
			{
				MessageBox.Show("Vous ne pouvez pas créer un salarié sans prénom", "Erreur");
				return;
			}
			
			if (CurrentSalarie.Site != null)
			{
				CurrentSalarie.SiteId = CurrentSalarie.Site.Id;
				CurrentSalarie.Site = null;
			}
			else
			{
				MessageBox.Show("Veuillez sélectionner un site", "Erreur");
				return;
			}
				
			if (CurrentSalarie.Service != null)
			{
				CurrentSalarie.ServiceId = CurrentSalarie.Service.Id;
				CurrentSalarie.Service = null;
			}
			else
			{
				MessageBox.Show("Veuillez sélectionner un service", "Erreur");
				return;
			}

			Task.Run(async () =>
			{

				return await HttpClientService.CreateNewSalarie(CurrentSalarie);

			}).ContinueWith(t =>
			{
				if (t.Result == null)
				{
					MessageBox.Show("Création impossible.");
				}
				else
				{
					MessageBox.Show("Salarié créé");
					CurrentSalarie = null;
					DetailsSalarieVisibility = Visibility.Hidden;
					ConsultVisibility = Visibility.Visible;
					ModifyVisibility = Visibility.Hidden;
					GetSalaries();
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void GetServices()
		{
			ListeService.Clear();

			Task.Run(async () =>
			{
				return await HttpClientService.GetServices();

			})
			.ContinueWith(t =>
			{
				foreach (var item in t.Result)
				{
					ListeService.Add(item);
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void GetSites()
		{
			ListeSite.Clear();

			Task.Run(async () =>
			{
				return await HttpClientService.GetSites();

			})
			.ContinueWith(t =>
			{
				foreach (var item in t.Result)
				{
					ListeSite.Add(item);
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void AjoutSalarie(object obj)
		{
			CurrentSalarie = new Salarie
			{
				Name = "",
				Surname = ""
			};
			Ajout = true;
			DetailsSalarieVisibility = Visibility.Visible;
			ConsultVisibility = Visibility.Hidden;
			ModifyVisibility = Visibility.Visible;
		}

		private void FermerDetails(object obj)
		{
			CurrentSalarie = null;
			DetailsSalarieVisibility = Visibility.Hidden;
		}

		private void GetSalaries()
		{
			Salaries.Clear();
			ListeSalarieFromSearch.Clear();
			SalarieListVisibility = Visibility.Hidden;
			SalarieLoadingVisibility = Visibility.Visible;

			Task.Run(async () =>
			{
				return await HttpClientService.GetSalaries();

			})
			.ContinueWith(t =>
			{
				foreach (var sal in t.Result)
				{
					var item = new SalarieItemViewModel(sal);
					item.EH_ConsultSalarie += Item_EH_ConsultSalarie;
					item.EH_Deleted += Reload;
					Salaries.Add(item);


				}
				TotalItems = Salaries.Count;
				CalculateNumberOfPages();
				LoadPage(CurrentPage);
				SalarieListVisibility = Visibility.Visible;
				SalarieLoadingVisibility = Visibility.Hidden;

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void Item_EH_ConsultSalarie(object? sender, EventArgs e)
		{
			SalarieItemViewModel item = (SalarieItemViewModel)sender;
			Task.Run(async () =>
			{
				return await HttpClientService.GetSalarieDetails(item.SalarieLight.Id);

			})
			.ContinueWith(t =>
			{
				if (t.Result != null)
				{
					CurrentSalarie = (Salarie)t.Result;
					DetailsSalarieVisibility = Visibility.Visible;
				}
				else
				{
					MessageBox.Show("Une erreur est survenue en chargeant les détails du salarié.", "Abandon");
					return;
				}

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void CalculateNumberOfPages()
		{
			NumberOfPages = TotalItems / NumberOfItemsPerPage;
			if (TotalItems % NumberOfItemsPerPage != 0)
			{
				NumberOfPages++;
			}

		}

		private void LoadPage(int currentPage)
		{
			ListeGauche.Clear();
			ListeDroite.Clear();

			if (NameSearchQuery == "" && SiteSearchQuery == "" && ServiceSearchQuery == "")
			{
				ListeSalarieFromSearch.Clear();
				//ListeSalarieFromSearch = Salaries;
				foreach (var item in Salaries)
				{
					ListeSalarieFromSearch.Add(item);
				}
			}

			int currentIndex = (currentPage - 1) * NumberOfItemsPerPage;

			for (int i = currentIndex; i < (currentIndex + (NumberOfItemsPerPage / 2)); i++)
			{
				if (i < ListeSalarieFromSearch.Count)
				{
					ListeGauche.Add(ListeSalarieFromSearch[i]);
				}

				if ((i + (NumberOfItemsPerPage / 2)) < ListeSalarieFromSearch.Count)
				{
					ListeDroite.Add(ListeSalarieFromSearch[i + (NumberOfItemsPerPage / 2)]);
				}

			}


		}

		private void Reload(object? sender, EventArgs e)
		{
			NameSearchQuery = "";
			SiteSearchQuery = "";
			ServiceSearchQuery = "";
			GetSalaries();
		}

		private void NextPage(object j)
		{
			if (CurrentPage == NumberOfPages)
				return;
			CurrentPage++;
			LoadPage(CurrentPage);
		}

		private void PreviousPage(object j)
		{
			if (CurrentPage == 1)
				return;
			CurrentPage--;
			LoadPage(CurrentPage);
		}

		internal void ReloadList()
		{
			ListeSalarieFromSearch.Clear();

			//ListeSalarieFromSearch = Salaries.Where(p => p.SalarieLight.Name.Contains(SearchQuery)).Select(p => p);
			foreach (var item in Salaries)
			{
				bool nameFilter = false;
				bool siteFilter = false;
				bool serviceFilter = false;

				if (NameSearchQuery != "")
				{
					if (item.FullName.ToLower().Contains(NameSearchQuery.ToLower()))
					{
						nameFilter = true;
					}
				}
				else
					nameFilter = true;

				if (SiteSearchQuery != "")
				{
					if (item.SalarieLight.Site.ToLower().Contains(SiteSearchQuery.ToLower()))
					{
						siteFilter = true;
					}
				}
				else
					siteFilter = true;

				if (ServiceSearchQuery != "")
				{
					if (item.SalarieLight.Service.ToLower().Contains(ServiceSearchQuery.ToLower()))
					{
						serviceFilter = true;
					}
				}
				else
					serviceFilter = true;

				if (siteFilter && nameFilter && serviceFilter)
				{
					ListeSalarieFromSearch.Add(item);
					siteFilter = false;
					nameFilter = false;
					serviceFilter = false;
				}

			}

			//switch (CurrentSearchMethod)
			//{
			//	case "Nom":
			//		foreach (var item in Salaries)
			//		{
			//			if (item.FullName.ToLower().Contains(SearchQuery.ToLower()))
			//			{
			//				ListeSalarieFromSearch.Add(item);
			//			}
			//		}
			//		break;
			//	case "Service":
			//		foreach (var item in Salaries)
			//		{
			//			if (item.SalarieLight.Service.ToLower().Contains(SearchQuery.ToLower()))
			//			{
			//				ListeSalarieFromSearch.Add(item);
			//			}
			//		}
			//		break;
			//	case "Site":
			//		foreach (var item in Salaries)
			//		{
			//			if (item.SalarieLight.Site.ToLower().Contains(SearchQuery.ToLower()))
			//			{
			//				ListeSalarieFromSearch.Add(item);
			//			}
			//		}
			//		break;
			//}
			TotalItems = ListeSalarieFromSearch.Count;
			CalculateNumberOfPages();
			CurrentPage = 1;
			LoadPage(CurrentPage);
		}
	}
}
