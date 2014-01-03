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
	using System.Windows.Media.Effects;

	#endregion // Using

	public partial class TaskManagerView : UserControl, ITaskManagerView
	{
		#region Properties

		TaskManagerVm ViewModel { get { return this.DataContext as TaskManagerVm; } }

		#endregion // Properties

		#region Constructors

		public TaskManagerView(TaskManagerVm vm)
		{
			InitializeComponent();

			this.DataContext = vm;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Set viewmodel's view using property injection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.ViewModel.View = this;
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			if (this.ViewModel != null)
			{
				this.ViewModel.View = null;
			}
		}

		#endregion // Methods

		/*public void Unload()
		{
			if (this.ViewModel != null)
			{
				this.ViewModel.Unload();
			}
		}*/
	}
}
