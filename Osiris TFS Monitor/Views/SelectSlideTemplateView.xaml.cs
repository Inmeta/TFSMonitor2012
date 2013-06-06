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

	public partial class SelectSlideTemplateView : DialogWindow
	{
		#region Fields

		SelectSlideTemplateVm _vm;

		#endregion // Fields

		#region Properties

		public SlideTemplateVm Selected 
		{ 
			get { return _vm.SelectedResult; }
		}

		#endregion // Properties

		#region Constructors

		public SelectSlideTemplateView(Window owner)
		{
			InitializeComponent();
			_vm = new SelectSlideTemplateVm();
			this.DataContext = _vm;
			this.Owner = owner;
		}

		#endregion // Constructors

		#region Methods

		private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_vm.CloseHandler = delegate() { Close(); };
			_listView.Focus();
		}

		#endregion // Methods
	}
}
