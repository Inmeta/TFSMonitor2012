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
	using Osiris.Tfs.Monitor.Models;

	#endregion // Using

	public partial class SlideElementEditContainerView : UserControl, ISlideElementContainerView
	{
		#region Fields

		Canvas _canvas;
		SlideElementContainerVm _vm;
		bool _ignoreCanvasChange = false;
		List<MouseDrag> _mouseDrags;

		#endregion // Fields

		public SlideElementContainerVm ViewModel { get { return _vm; } }

		public new UIElement UIElement { get { return this; } }

		#region Constructors

		public SlideElementEditContainerView(Canvas canvas, SlideElementContainerVm vm)
		{
			InitializeComponent();

			_canvas = canvas;
			_vm = vm;
			this.DataContext = vm;

			// Add drag-events: Hmm we should free eventhandler in unload-event?? check out...
			_mouseDrags = new List<MouseDrag>();
			_mouseDrags.Add(new MouseDrag(this, this, Cursors.SizeAll, new MouseDragHandler(OnMoveDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeE, Cursors.SizeWE, new MouseDragHandler(OnSizeEDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeW, Cursors.SizeWE, new MouseDragHandler(OnSizeWDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeS, Cursors.SizeNS, new MouseDragHandler(OnSizeSDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeN, Cursors.SizeNS, new MouseDragHandler(OnSizeNDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeNW, Cursors.SizeNWSE, new MouseDragHandler(OnSizeNWDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeNE, Cursors.SizeNESW, new MouseDragHandler(OnSizeNEDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeSW, Cursors.SizeNESW, new MouseDragHandler(OnSizeSWDrag)));
			_mouseDrags.Add(new MouseDrag(this, sizeSE, Cursors.SizeNWSE, new MouseDragHandler(OnSizeSEDrag)));
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

			if (_mouseDrags != null)
			{
				foreach (var md in _mouseDrags)
				{
					md.UnRegisterEvents();
				}
				_mouseDrags.Clear();
			}
		}

		private void OnMoveDrag(MouseDrag md, double offsetX, double offsetY)
		{
			Canvas.SetLeft(this, Math.Max(0, Math.Min(_canvas.ActualWidth - this.ActualWidth, md.CanvasLeft + offsetX)));
			Canvas.SetTop(this, Math.Max(0, Math.Min(_canvas.ActualHeight - this.ActualHeight, md.CanvasTop + offsetY)));
			ReportCanvasChange();
		}

		private void SizeEDrag(MouseDrag md, double offsetX, double offsetY)
		{
			this.Width = Math.Max(18, Math.Min(_canvas.ActualWidth - Canvas.GetLeft(this), md.Width + offsetX));
		}

		private void OnSizeEDrag(MouseDrag md, double offsetX, double offsetY)
		{
			SizeEDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void SizeWDrag(MouseDrag md, double offsetX, double offsetY)
		{
			Canvas.SetLeft(this, Math.Min(md.CanvasLeft + md.Width - 18, Math.Max(0, md.CanvasLeft + offsetX)));
			this.Width = Math.Max(18, md.Width - Math.Max(-md.CanvasLeft, offsetX));
		}

		private void OnSizeWDrag(MouseDrag md, double offsetX, double offsetY)
		{
			SizeWDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void SizeNDrag(MouseDrag md, double offsetX, double offsetY)
		{
			Canvas.SetTop(this, Math.Min(md.CanvasTop + md.Height - 18, Math.Max(0, md.CanvasTop + offsetY)));
			this.Height = Math.Max(18, md.Height - Math.Max(-md.CanvasTop, offsetY));
		}

		private void OnSizeNDrag(MouseDrag md, double offsetX, double offsetY)
		{
			SizeNDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void SizeSDrag(MouseDrag md, double offsetX, double offsetY)
		{
			this.Height = Math.Max(18, Math.Min(_canvas.ActualHeight - Canvas.GetTop(this), md.Height + offsetY));
		}

		private void OnSizeSDrag(MouseDrag md, double offsetX, double offsetY)
		{
			SizeSDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void OnSizeNWDrag(MouseDrag md, double offsetX, double offsetY)
		{
			OnSizeNDrag(md, offsetX, offsetY);
			OnSizeWDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void OnSizeNEDrag(MouseDrag md, double offsetX, double offsetY)
		{
			OnSizeNDrag(md, offsetX, offsetY);
			OnSizeEDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void OnSizeSEDrag(MouseDrag md, double offsetX, double offsetY)
		{
			OnSizeSDrag(md, offsetX, offsetY);
			OnSizeEDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void OnSizeSWDrag(MouseDrag md, double offsetX, double offsetY)
		{
			OnSizeSDrag(md, offsetX, offsetY);
			OnSizeWDrag(md, offsetX, offsetY);
			ReportCanvasChange();
		}

		private void ReportCanvasChange()
		{
			_ignoreCanvasChange = true;
			_vm.Position = new Position(
				(_canvas.ActualWidth == 0) ? 0 : (Canvas.GetLeft(this) / _canvas.ActualWidth), 
				(_canvas.ActualHeight == 0) ? 0 : (Canvas.GetTop(this) / _canvas.ActualHeight),
				(_canvas.ActualWidth == 0) ? 0 : (this.Width / _canvas.ActualWidth), 
				(_canvas.ActualHeight == 0) ? 0 : (this.Height / _canvas.ActualHeight));
		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Position")
			{
				if (_ignoreCanvasChange)
				{
					_ignoreCanvasChange = false;
					return;
				}
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

		private void UserControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				_vm.Delete();
			}
		}


		#endregion // Methods

        #region ISlideElementContainerView Members

        public void InsertSlideElement(object viewModel)
        {
            // Make factory....

            if (viewModel is WebPageVm)
            {
                _cp.Content = new WebPageView(viewModel as WebPageVm);
            }
			else if (viewModel is WebPageDesignVm)
			{
				_cp.Content = new WebPageDesignView();
			}
			else if (viewModel is BurndownChartVm)
            {
                _cp.Content = new BurndownChartView(viewModel as BurndownChartVm);
            }
            else if (viewModel is BuildMonitorVm)
            {
                _cp.Content = new BuildMonitorView(viewModel as BuildMonitorVm);
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
