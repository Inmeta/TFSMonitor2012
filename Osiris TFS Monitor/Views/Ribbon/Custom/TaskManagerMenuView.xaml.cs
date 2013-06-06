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

	public partial class TaskManagerMenuView : RibbonTab, ITaskManagerMenuView, ICustomTab
	{
		#region Fields

		TaskManagerMenuVm _vm;

		#endregion // Fields

		public bool IsCommon { get { return false; } } 

		#region Constructors

		public TaskManagerMenuView(TaskManagerMenuVm vm)
		{
			_vm = vm;
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private void RibbonTab_Unloaded(object sender, RoutedEventArgs e)
		{
			if (_vm != null)
			{
				//_vm.Dispose();
				_vm = null;
			}
		}

		private void RibbonTab_Loaded(object sender, RoutedEventArgs e)
		{
			if (_vm != null)
			{
				_vm.View = this;
			}
		}

		#endregion // Methods
	}
}
