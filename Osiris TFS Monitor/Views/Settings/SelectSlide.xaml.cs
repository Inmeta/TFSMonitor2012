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
	using Osiris.Tfs.Monitor.Models;

	#endregion // Using

	public partial class SelectSlide : Window
	{
		#region Properties

		private ConsoleVM ViewModel { get { return this.DataContext as ConsoleVM; } }

		#endregion // Properties

		#region Constructors

		public SelectSlide(ConsoleVM viewModel)
		{
			InitializeComponent();

			this.DataContext = viewModel;
		}

		#endregion // Constructors

		#region Methods

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (_slideTypes.Items.Count > 0)
			{
				_slideTypes.SelectedIndex = 0;
			}
			_slideTypes.Focus();
		}

		private void Ok_Click(object sender, RoutedEventArgs e)
		{
			if (_slideTypes.SelectedItem == null)
			{
				return;
			}

			this.DialogResult = true;
			var dlg = new SlidePropView(this.ViewModel, _slideTypes.SelectedItem as SlideType);
			dlg.Owner = Application.Current.MainWindow;
			dlg.ShowDialog();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void SlideTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_ok.IsEnabled = _slideTypes.SelectedItem != null;
		}

		#endregion // Methods
	}
}
