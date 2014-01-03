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

	public partial class ApplicationOptionsView : DialogWindow
	{
		#region Constructors

		public ApplicationOptionsView()
		{
			InitializeComponent();
		}

		public ApplicationOptionsView(Window owner)
		{
			InitializeComponent();
			this.Owner = owner;
		}

		#endregion // Constructors

		#region Methods

		private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var vm = this.DataContext as ApplicationOptionsVm;
			vm.CloseHandler = delegate() { Close(); };
		}

		#endregion // Methods
	}
}
