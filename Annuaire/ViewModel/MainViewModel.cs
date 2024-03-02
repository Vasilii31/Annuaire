using Annuaire.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public MainViewModel()
        {
			//var uc = new AccueilView();
			//uc.DataContext = new AccueilViewModel();
			CurrentViewModel = new AccueilViewModel();
			
		}




    }
}
