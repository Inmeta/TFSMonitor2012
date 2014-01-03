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
	using Osiris.Tfs.Monitor.Models;

	#endregion // Using

	public interface ISlidePropView : IDisposable
	{
		void ShowSlide(BurndownSlidePropVM vm);
	}

	public partial class SlidePropView : Window, ISlidePropView
	{
		#region Fields

		SlidePropVM _viewModel;
		IDisposable _slide;

		#endregion // Fields
	
		#region Constructors

		/// <summary>
		/// Edit new slide
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="slideType"></param>
		public SlidePropView(ConsoleVM viewModel, SlideType slideType)
		{
			InitializeComponent();

			_viewModel = SlidePropVM.EditSlide(viewModel, slideType, this);

			this.DataContext = _viewModel;
		}

		/// <summary>
		/// Edit slide
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="slide"></param>
		public SlidePropView(ConsoleVM viewModel, Slide slide)
		{
			InitializeComponent();

			_viewModel = SlidePropVM.EditSlide(viewModel, slide, this);

			this.DataContext = _viewModel;
		}

		#endregion // Constructors

		#region Methods

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_name.Focus();
			_name.SelectAll();
		}

		public void ShowSlide(BurndownSlidePropVM vm)
		{
			_slideTab.Header = "Burndown settings";

			// Add slide to container
			var slide = new BurndownSlideProp(vm);
			_slide = slide;
			_dockPanel.Children.Add(slide);
		}

		/// <summary>
		/// Add new slide
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Ok_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.Save();
			this.DialogResult = true;
		}

		/// <summary>
		/// Cancel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_slide != null)
			{
				_slide.Dispose();
			}
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			_viewModel.Dispose();
		}

		#endregion // IDisposable Members
	}
}
