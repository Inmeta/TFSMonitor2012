namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;
	using Osiris.Tfs.Report;
	using System.Diagnostics;
	using Osiris.Tfs.Monitor.Models;
	using System.Windows.Media.Effects;
	using System.Windows.Media.Animation;
	using System.ComponentModel;
	using Microsoft.TeamFoundation.Server;

	#endregion // Using

	public partial class TwitterView : UserControl, ITwitterView, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		#region Properties

		TwitterVm ViewModel { get { return this.DataContext as TwitterVm; } }

		#endregion // Properties

		#region Constructors

		public TwitterView(TwitterVm vm)
		{
			InitializeComponent();

			this.DataContext = vm;
		}

		#endregion // Constructors

		/// <summary>
		/// Set viewmodel's view using property injection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.ViewModel.View = this;

			
			_itemsControl.Visibility = Visibility.Visible;
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			if (this.ViewModel != null)
			{
				this.ViewModel.View = null;
			}
		}

		

	}

}
