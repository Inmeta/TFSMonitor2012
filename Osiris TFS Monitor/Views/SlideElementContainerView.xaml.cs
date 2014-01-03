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
	using System.ComponentModel;
	using System.Diagnostics;

	#endregion // Using

	public partial class SlideElementContainerView : UserControl, ISlideElementContainerView
	{
		#region Fields

		Canvas _canvas;
		SlideElementContainerVm _vm;

		#endregion // Fields

		public SlideElementContainerVm ViewModel { get { return _vm; } }

		public new UIElement UIElement { get { return this; } }

		#region Constructors

		public SlideElementContainerView(Canvas canvas, SlideElementContainerVm vm)
		{
			InitializeComponent();

			_canvas = canvas;
			_vm = vm;
			this.DataContext = vm;
		}

		#endregion // Constructors

		#region Methods

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_vm.View = this;
			_vm.PropertyChanged += OnViewModelPropertyChanged;
			_canvas.SizeChanged += Canvas_SizeChanged;
			ConvertFromPosition();
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			_canvas.SizeChanged += Canvas_SizeChanged;
			_vm.PropertyChanged -= OnViewModelPropertyChanged;
			_vm.View = null;
			_canvas.Children.Clear();
            _cp.Content = null;
		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Position")
			{
				ConvertFromPosition();
			}
		}

		private void ConvertFromPosition()
		{
			Canvas.SetLeft(this, _vm.Position.X * _canvas.ActualWidth);
			Canvas.SetTop(this, _vm.Position.Y * _canvas.ActualHeight);
			this.Width = _vm.Position.Width * _canvas.ActualWidth;
			this.Height = _vm.Position.Height * _canvas.ActualHeight;
		}

		private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			ConvertFromPosition();
		}

		private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			_vm.Select();
		}

		#endregion // Methods

        #region ISlideElementContainerView Members


        public void InsertSlideElement(object viewModel)
        {
            if (viewModel is WebPageVm)
            {
                _cp.Content = new WebPageView(viewModel as WebPageVm);
            }
            else if (viewModel is BurndownChartVm)
            {
                _cp.Content = new BurndownChartView(viewModel as BurndownChartVm);
            }
            else if (viewModel is BuildMonitorVm)
            {
                _cp.Content = new BuildMonitorView(viewModel as BuildMonitorVm);
            }
			else if (viewModel is TaskManagerVm)
			{
				_cp.Content = new TaskManagerView(viewModel as TaskManagerVm);
			}
			else if (viewModel is TwitterVm)
			{
				_cp.Content = new TwitterView(viewModel as TwitterVm);
			}
			else
            {
                Debug.Assert(false);
            }
        }

        #endregion
    }
}
