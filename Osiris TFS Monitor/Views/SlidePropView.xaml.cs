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

	public partial class SlidePropView : DialogWindow, ISlidePropView
	{
		#region Constructors

		public SlidePropView(Window owner, SlideTemplateVm slideType)
		{
			InitializeComponent();
			this.Owner = owner;
			this.DataContext = slideType.CreateSlidePropVM(this);
		}

		#endregion // Constructors

		#region Methods


		#endregion // Methods

		#region ISlidePropView2 Members

		/*void Close()
		{
			this.Close();
		}*/

		#endregion // ISlidePropView2 Members
	}
}
