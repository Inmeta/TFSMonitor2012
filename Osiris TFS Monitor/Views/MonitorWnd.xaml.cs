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
	using System.Diagnostics;

	#endregion // Using

	public partial class MonitorWnd : Window
	{

		public MonitorWnd()
		{
			InitializeComponent();

			this.Focus();
			
			_monitor.StartSlideshow();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			_monitor.StopSlideshow();
		}


		#region Methods


		#endregion // Methods


	}
}
