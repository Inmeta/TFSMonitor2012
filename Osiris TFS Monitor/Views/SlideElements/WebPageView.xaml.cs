using System.Windows;
using System.Windows.Controls;

namespace Osiris.Tfs.Monitor
{
	public partial class WebPageView : UserControl
	{
		private readonly WebPageVm _vm;

		public WebPageView(WebPageVm vm)
		{
			_vm = vm;
			this.DataContext = vm;
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_vm.Continue(new System.Drawing.Size((int)ActualWidth, (int)ActualHeight));
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			_vm.Pause();
		}
	}
}
