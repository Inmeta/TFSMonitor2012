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

	public partial class BuildMonitorPanelView : UserControl
	{
		#region Properties

		BuildVm ViewModel { get { return this.DataContext as BuildVm; } }

		public LinearGradientBrush BkBrush { get; set; }

		#endregion // Properties

		#region Constructors

		public BuildMonitorPanelView()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			_userImage.Width = _userImageGrid.ActualHeight * 0.7;
			if (this.ViewModel != null)
			{
				this.ViewModel.SizeChanged(e.NewSize.Width, e.NewSize.Height);
			}
			e.Handled = true;
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.BkBrush = null;
			//this._userImage.Source = null;
			//this.UserImage.Loaded -= OnUserImageLoaded;
			//this.UserImage.Source = null;

		}

		private void OnUserImageSourceUpdated(object sender, DataTransferEventArgs e)
		{
			_userImage.Width = _userImageGrid.ActualHeight*0.7;
		}

		#endregion // Methods
	}

	public class UserImageScaleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var height = (double) value;
			if (double.IsNaN(height))
			{
				return 0;
			}
			return height * 7;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}


}
