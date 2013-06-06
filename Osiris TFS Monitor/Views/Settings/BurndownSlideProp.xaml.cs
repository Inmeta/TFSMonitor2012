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
	using Osiris.Tfs.Monitor.Models;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using System.Diagnostics;

	#endregion // Using

	//public interface ISlidePropView : IDisposable { }

	public partial class BurndownSlideProp : UserControl, IDisposable
	{
		#region Fields

		BurndownSlidePropVM _vm;

		#endregion // Fields

		#region Constructors

		public BurndownSlideProp(BurndownSlidePropVM vm)
		{
			_vm = vm;

			InitializeComponent();

			this.DataContext = vm;
		}

		#endregion // Constructors

		#region Methods

		private void Change_Click(object sender, RoutedEventArgs e)
		{
			_vm.Change();
			_change.IsEnabled = false;
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			_vm.Dispose();
		}

		#endregion // IDisposable Members
	}
}
