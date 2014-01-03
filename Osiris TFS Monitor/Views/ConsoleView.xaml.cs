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
	using System.ComponentModel;
	using Osiris.Tfs.Monitor;
	using System.Configuration;
	using System.Diagnostics;
	using Osiris.Tfs.Monitor.Properties;
	using Osiris.Tfs.Monitor.Models;

	#endregion // Using

	public partial class ConsoleView : Window
	{
		#region Fields

		private ConsoleVM _viewModel = new ConsoleVM();

		#endregion // Fields

		#region Constructor

		public ConsoleView() : this(null)
		{
		}

		public ConsoleView(Window owner)
		{
			InitializeComponent();
			this.Owner = owner;
			this.DataContext = _viewModel;
			ShowDialog();
		}


		#endregion // Constructor

		#region Methods

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.Save();
			this.DialogResult = true;
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		#endregion // Methods
	}

}
