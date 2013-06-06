namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using Microsoft.TeamFoundation.Proxy;
	using System.ComponentModel;
	using System.Windows.Controls;
	using System.Windows;

	#endregion // Using

	public partial class GeneralSettings : UserControl
	{
		private ConsoleVM ViewModel { get { return this.DataContext as ConsoleVM; } }

		public GeneralSettings()
		{
			InitializeComponent();
		}

		private void SelectTfsServer_Click(object sender, RoutedEventArgs e)
		{
			DomainProjectPicker dpp = new DomainProjectPicker(DomainProjectPickerMode.None);
			if (dpp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.ViewModel.Model.TfsUri = dpp.SelectedServer.Uri.ToString();
			}
			e.Handled = true;
		}

		private void UseTfsLocalAccount_Change(object sender, RoutedEventArgs e)
		{
			_tfsUser.IsEnabled = !this.ViewModel.Model.TfsUseLocalAccount;
			_tfsDomain.IsEnabled = !this.ViewModel.Model.TfsUseLocalAccount;
			_tfsPassword.IsEnabled = !this.ViewModel.Model.TfsUseLocalAccount;
		}
	}
}
