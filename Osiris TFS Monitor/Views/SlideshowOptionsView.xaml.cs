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
	using Microsoft.TeamFoundation.Proxy;

	#endregion // Using

	public partial class SlideshowOptionsView : UserControl
	{
		#region Constructors

		public SlideshowOptionsView()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Bug in WPF 3.5 prevents us from using binding of IsChecked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			var dc = this.DataContext as SlideshowOptionsVm;
			//dc.MonitorSame = _rbSame.IsChecked.HasValue ? _rbSame.IsChecked.Value : false;
			//dc.MonitorSingle = _rbSingle.IsChecked.HasValue ? _rbSingle.IsChecked.Value : false;
			//dc.MonitorDifferent = _rbDifferent.IsChecked.HasValue ? _rbDifferent.IsChecked.Value : false;
		}

		#endregion // Methods
	}
}
