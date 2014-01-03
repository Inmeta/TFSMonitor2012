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
	using Osiris.Tfs.Monitor.Models;
	using System.Diagnostics;

	#endregion // Using

	public partial class FullscreenSlideView : Window, IFullscreenSlideView
	{
		private FullscreenSlideVm _vm;

		public FullscreenSlideView(FullscreenSlideVm vm)
		{
			_vm = vm;
			_vm.View = this;
			InitializeComponent();
			this.DataContext = vm;
			SetContent(vm.Slide);
		}

		public void SetContent(SlideVm slide)
		{
			if (slide == null)
			{
				this.Content = null;				
			}
			else
			{
				this.Content = new SlideView(slide);
			}
		}

	}
}
