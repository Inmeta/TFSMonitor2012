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
	using System.Reflection;
	using System.Globalization;

	#endregion // Using

	public partial class TweetView : UserControl
	{
		TweetVm ViewModel { get { return this.DataContext as TweetVm; } }

		#region Constructors

		public TweetView()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.ViewModel != null)
			{
				this.ViewModel.SizeChanged(e.NewSize.Width, e.NewSize.Height);
			}
			e.Handled = true;
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
		}

		#endregion // Methods
	}


}
