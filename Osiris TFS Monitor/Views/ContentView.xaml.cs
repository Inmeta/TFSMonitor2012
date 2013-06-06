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
	using System.Diagnostics;

	#endregion // Using

	public partial class ContentView : UserControl, IContentView
	{
		#region Constructors

		public ContentView()
		{
			InitializeComponent();

			this.DataContext = new ContentVm(this);
		}

		#endregion // Constructors

		#region Methods

		ContentPresenter _cp;

		public void SetContent(SlideVm slide)
		{
			doc.Children.Clear();

			if (slide == null)
			{
				return;
			}

			var view = new SlideView();
			view.DataContext = slide;

			doc.Children.Add(view);
		}

		#endregion // Methods

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{

		}

		private void cp_Loaded(object sender, RoutedEventArgs e)
		{
			var x = _cp;
		}


		#region IContentView Members



		#endregion

		#region IContentView Members


		public new Visual Visual
		{
			get { return doc; }
		}

		#endregion

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			doc.Width = this.ActualWidth - 3 - 7;
			doc.Height = this.ActualHeight - 3 - 6;
		}
	}
}
