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

	public partial class SlideshowView_old : Window
	{
	}

	/*
	public interface ISlideshowView
	{
		void Show(Slide slide);
		int ScreenOffset { get; }
	}

	public partial class SlideshowView : Window, ISlideshowView, IDisposable, IFullscreenWindow
	{
		#region Fields

		SlideshowVm _vm;
		Dictionary<Slide, SlideView> _slides = new Dictionary<Slide, SlideView>();

		#endregion // Fields

		public SlideshowVm ViewModel { get { return _vm; } }

		public int ScreenOffset { get; set; }

		#region Constructors

		public SlideshowView()
		{
			InitializeComponent();

			_vm = new SlideshowVm(this);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_vm.StartTimer();
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Databind instead...
		/// </summary>
		/// <param name="view"></param>
		public void Show(Slide slide)
		{
			if (!_slides.ContainsKey(slide))
			{
				_slides[slide] = new SlideView(new SlideVm(slide, true, true));
				_grid.Children.Add(_slides[slide]);
			}
			else
			{
				foreach (SlideView view in _grid.Children)
				{
					if (view == _slides[slide])
					{
						view.Visibility = Visibility.Visible;
					}
					else
					{
						view.Visibility = Visibility.Collapsed;
					}
				}
			}
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			_vm.StopTimer();

			// Dispose children
			foreach (var s in _slides)
			{
				s.Value.Dispose();
			}
			_slides.Clear();
			_grid.Children.Clear();

			this.Close();
		}

		#endregion // IDisposable Members

	}
	 */
}
