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
	using System.Windows.Shapes;
	using System.Collections.ObjectModel;

	#endregion // Using

	public partial class BurndownSlidePropView : UserControl
	{
		#region Constructors

		public BurndownSlidePropView()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private void Change_Click(object sender, RoutedEventArgs e)
		{
			//_vm.Change();
			//_change.IsEnabled = false;
		}

		#endregion // Methods
	}
}
