using Annuaire.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Annuaire.View
{
	/// <summary>
	/// Logique d'interaction pour AccueilView.xaml
	/// </summary>
	public partial class AccueilView : UserControl
	{
		public AccueilView()
		{
			InitializeComponent();
		}

		private void NametextChangedEventHandler(object sender, TextChangedEventArgs args)
		{

			var item = (TextBox)sender;

			AccueilViewModel dtC = (AccueilViewModel)item.DataContext;
			if (dtC != null)
			{
				dtC.NameSearchQuery = item.Text;
				dtC.ReloadList();
			}

		}
		private void SitetextChangedEventHandler(object sender, TextChangedEventArgs args)
		{

			var item = (TextBox)sender;

			AccueilViewModel dtC = (AccueilViewModel)item.DataContext;
			if (dtC != null)
			{
				dtC.SiteSearchQuery = item.Text;
				dtC.ReloadList();
			}

		}
		private void ServicetextChangedEventHandler(object sender, TextChangedEventArgs args)
		{

			var item = (TextBox)sender;

			AccueilViewModel dtC = (AccueilViewModel)item.DataContext;
			if (dtC != null)
			{
				dtC.ServiceSearchQuery = item.Text;
				dtC.ReloadList();
			}

		}
	}
}
