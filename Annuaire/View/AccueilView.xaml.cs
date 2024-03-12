﻿using Annuaire.ViewModel;
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

		private void textChangedEventHandler(object sender, TextChangedEventArgs args)
		{

			var item = (TextBox)sender;

			AccueilViewModel dtC = (AccueilViewModel)item.DataContext;

			dtC.SearchQuery = item.Text;
			dtC.ReloadList();
			
		}
	}
}
