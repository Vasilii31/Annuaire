using Annuaire.Command;
using Annuaire.Services;
using Annuaire.ViewModel.ItemsViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Annuaire.ViewModel
{
	public class AccueilViewModel : BaseViewModel
	{
		public ObservableCollection<SalarieItemViewModel> Salaries { get; set; } = new();

		public ObservableCollection<SalarieItemViewModel> ListeGauche {  get; set; } = new();
		public ObservableCollection<SalarieItemViewModel> ListeDroite {  get; set; } = new();

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

		private int _totalItems = 0;

        public int TotalItems
        {
            get { return _totalItems; }
            set {
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

		public ICommand NextPageCommand { get; set; }
		public ICommand PreviousPageCommand { get; set; }

		public AccueilViewModel()
        {
			GetSalaries();
			NextPageCommand = new RelayCommand(NextPage);
			PreviousPageCommand = new RelayCommand(PreviousPage);
        }

		private void GetSalaries()
		{
			Salaries.Clear();
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

					Salaries.Add(new SalarieItemViewModel(sal));

				}
				TotalItems = Salaries.Count;
				CalculateNumberOfPages();
				LoadPage(CurrentPage);
				SalarieListVisibility = Visibility.Visible;
				SalarieLoadingVisibility = Visibility.Hidden;

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void CalculateNumberOfPages()
		{
			NumberOfPages = TotalItems / NumberOfItemsPerPage;
			if( TotalItems % NumberOfItemsPerPage != 0)
			{
				NumberOfPages++;
			}
			
		}

		private void LoadPage(int currentPage)
		{
			ListeGauche.Clear();
			ListeDroite.Clear();

			int currentIndex = (currentPage - 1) * NumberOfItemsPerPage;

			for(int i = currentIndex; i < (currentIndex + (NumberOfItemsPerPage / 2));  i++)
			{
				if(i < Salaries.Count)
				{
					ListeGauche.Add(Salaries[i]);
				}

				if ((i + (NumberOfItemsPerPage / 2)) < Salaries.Count)
				{
					ListeDroite.Add(Salaries[i + (NumberOfItemsPerPage / 2)]);
				}
				
			}
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
	}
}
