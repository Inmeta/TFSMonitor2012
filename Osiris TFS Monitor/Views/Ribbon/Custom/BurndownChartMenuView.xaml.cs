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
using Microsoft.Windows.Controls.Ribbon;
    using System.Diagnostics;

	#endregion // Using

	public interface ICustomTab
	{
		bool IsCommon { get; }
	}


	public partial class BurndownChartMenuView : RibbonTab, IBurndownChartMenuView, ICustomTab
	{
		#region Fields

		BurndownChartMenuVm _vm;

		#endregion // Fields

		public bool IsCommon { get { return false; } } 

		#region Constructors

		public BurndownChartMenuView(BurndownChartMenuVm vm)
		{
			Debug.WriteLine("Creating BurndownChartMenuView..");

			_vm = vm;
			InitializeComponent();
		}

		#endregion // Constructors

		private void BurndownChart_Loaded(object sender, RoutedEventArgs e)
		{
			if (_vm != null)
			{
				_vm.View = this;
			}
		}

		private void BurndownChart_Unloaded(object sender, RoutedEventArgs e)
		{
			if (_vm != null)
			{
				_vm.View = null;
			}
		}
	}
}
