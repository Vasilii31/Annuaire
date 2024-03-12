using Annuaire.Command;
using Annuaire.Services;
using System.Windows;
using System.Windows.Input;


namespace Annuaire.ViewModel
{
	public class MainViewModel : BaseViewModel
	{

		private static MainViewModel instance = new MainViewModel();
		public static MainViewModel Instance => instance;

		private object _currentViewModel;
		public object CurrentViewModel
		{
			get { return _currentViewModel; }
			set
			{
				if (_currentViewModel != value)
				{
					_currentViewModel = value;
					OnPropertyChanged(nameof(CurrentViewModel));
				}
			}
		}

		private object _adminViewModel;
		public object AdminViewModel
		{
			get { return _adminViewModel; }
			set
			{
				if (_adminViewModel != value)
				{
					_adminViewModel = value;
					OnPropertyChanged(nameof(AdminViewModel));
				}
			}
		}

		private Visibility _adminViewVisibility = Visibility.Hidden;

		public Visibility AdminViewVisibility
		{
			get { return _adminViewVisibility; }
			set
			{
				_adminViewVisibility = value;
				OnPropertyChanged(nameof(AdminViewVisibility));
			}
		}

		private string _password = string.Empty;

		public string Password
		{
			get { return _password; }
			set
			{
				if (_password != value)
				{
					_password = value;
					OnPropertyChanged(nameof(Password));
				}
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

		public ICommand OpenAdminViewCommand { get; set; }
		public ICommand ConnexionCommand { get; set; }
		public ICommand RetourCommand { get; set; }
		public ICommand NavigationCommand { get; set; }
		public MainViewModel()
        {
			App.Current.Properties.Add("status", "visitor");

			CurrentViewModel = new AccueilViewModel();
			OpenAdminViewCommand = new RelayCommand(OpenAdminView);
			ConnexionCommand = new RelayCommand(Connexion);
			RetourCommand = new RelayCommand(CloseAdminWindow);
			NavigationCommand = new RelayCommand(NavigateTo);
		}

		private void NavigateTo(object obj)
		{
			if (obj is string)
			{
				switch(obj)
				{
					case "Sites":
						CurrentViewModel = new GestionSitesViewModel();
						break;
					case "Salaries":
						CurrentViewModel = new AccueilViewModel();
						break;
					case "Services":
						CurrentViewModel = new GestionServicesViewModel();
						break;
				}
			}
		}

		private void Connexion(object obj)
		{

			Task.Run(async () =>
			{
				return await HttpClientService.Login(Password);

			})
			.ContinueWith(t =>
			{
				if (t.Result == true)
				{
					
					App.Current.Properties.Remove("status");
					App.Current.Properties.Add("status", "admin");

					MessageBox.Show("Connexion administrateur confirmée.");
					CurrentViewModel = new AccueilViewModel();
					AdminViewVisibility = Visibility.Hidden;
					AdminContentVisibility = Visibility.Visible;
					
				}
				else
				{
					MessageBox.Show("Mot de passe admin incorrect.", "Accès refusé");
				}
				

				
				

			}, TaskScheduler.FromCurrentSynchronizationContext());

		}

		private void OpenAdminView(object obj)
		{
			AdminViewVisibility = Visibility.Visible;
		}

		private void CloseAdminWindow(object obj)
		{
			AdminViewVisibility = Visibility.Hidden;
			
		}


	}
}
