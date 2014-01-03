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

	public partial class ApplicationInfoView : DialogWindow
	{
		ApplicationInfoVm _vm;

		#region Constructors

		public ApplicationInfoView(Window owner)
		{
			InitializeComponent();
			this.Owner = owner;
			_vm = new ApplicationInfoVm();
			this.DataContext = _vm;
		}

		#endregion // Constructors

		#region Methods

		private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_vm.CloseHandler = delegate() { Close(); };
		}

		#endregion // Methods
	}
}
