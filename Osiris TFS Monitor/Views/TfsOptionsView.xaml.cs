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

	public partial class TfsOptionsView : UserControl
	{
		public TfsOptionsView()
		{
			InitializeComponent();
		}

		#region Methods

		private void SelectTfsServer_Click(object sender, RoutedEventArgs e)
		{
			var vm = (TfsOptionsVm)this.DataContext;

			DomainProjectPicker dpp = new DomainProjectPicker(DomainProjectPickerMode.None);
			if (dpp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				vm.Model.TfsUri = dpp.SelectedServer.Uri.ToString();
			}
			e.Handled = true;
		}

		#endregion // Methods
	}
}
