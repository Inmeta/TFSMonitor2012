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
	using System.Diagnostics;
	using System.Collections.Specialized;

	#endregion // Using

	public partial class SlideView : UserControl, ISlideView
	{
		#region Fields

		MouseDrag _mdSelect;
		Rectangle _selectRectangle;

		#endregion // Fields

		#region Properties

		public SlideVm Vm { get { return this.DataContext as SlideVm; } }

		//public UserControl Visual { get { return this; } }

		#endregion // Properties

		#region Constructors

		public SlideView()
		{
			InitializeComponent();
		}

		public SlideView(SlideVm vm)
		{
			InitializeComponent();
			this.DataContext = vm;
		}

		#endregion // Constructors

		#region Methods

		protected virtual void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.Vm == null)
			{
				return;
			}

			this.Vm.View = this;
			this.Vm.SlideElements.CollectionChanged += OnSlideElementsChanged;

			foreach (var elem in this.Vm.SlideElements)
			{
				canvas.Children.Add(CreateContainer(elem));
			}

			UpdateDesignMode();

		}

		protected virtual void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			canvas.Children.Clear();
			if (_mdSelect != null)
			{
				_mdSelect.UnRegisterEvents();
			}

			if (this.Vm != null)
			{
				this.Vm.View = null;
				this.Vm.SlideElements.CollectionChanged -= OnSlideElementsChanged;
			}
		}

		private void OnSelectDrag(MouseDrag md, double offsetX, double offsetY)
		{
			if (_selectRectangle == null)
			{
				_selectRectangle = new Rectangle();

				_selectRectangle.Stroke = new SolidColorBrush(Colors.Blue);
				_selectRectangle.Stroke.Opacity = 0.25;
				_selectRectangle.StrokeThickness = 2;
				_selectRectangle.Fill = new SolidColorBrush(Colors.Blue);
				_selectRectangle.Fill.Opacity = 0.1;
				canvas.Children.Add(_selectRectangle);
			}

			var pos = Mouse.GetPosition(this);
			var x = Math.Max(0, (offsetX < 0) ? pos.X : (pos.X - offsetX));
			var y = Math.Max(0, (offsetY < 0) ? pos.Y : (pos.Y - offsetY));

			Canvas.SetLeft(_selectRectangle, x);
			Canvas.SetTop(_selectRectangle, y);

			_selectRectangle.Width = (offsetX >= 0) ? (Math.Min(canvas.ActualWidth - x, Math.Abs(offsetX))) :
				(Math.Abs(offsetX) - (x - pos.X));
			_selectRectangle.Height = (offsetY >= 0) ? (Math.Min(canvas.ActualHeight - y, Math.Abs(offsetY))) :
				(Math.Abs(offsetY) - (y - pos.Y));
		}

		public void OnEndSelectDrag()
		{
			var selectedItems = new List<SlideElementContainerVm>();
			if (_selectRectangle != null)
			{
				Rect rcSelect = new Rect(Canvas.GetLeft(_selectRectangle), Canvas.GetTop(_selectRectangle),
					_selectRectangle.Width, _selectRectangle.Height);
				canvas.Children.Remove(_selectRectangle);
				_selectRectangle = null;

				foreach (FrameworkElement c in canvas.Children)
				{
					Rect rcContainer = new Rect(Canvas.GetLeft(c), Canvas.GetTop(c),
						c.Width, c.Height);

					if (rcSelect.Contains(rcContainer))
					{
						selectedItems.Add(((ISlideElementContainerView)c).ViewModel);
					}
				}
			}
			this.Vm.Select(selectedItems);
		}

		protected virtual void OnSlideElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				canvas.Children.Clear();
			}

			else if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
			{
				foreach (SlideElementContainerVm elem in e.NewItems)
				{
					canvas.Children.Add(CreateContainer(elem));
				}
			}

			else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
			{
				foreach (SlideElementContainerVm slide in e.OldItems)
				{
					 canvas.Children.Remove(slide.View.UIElement);
				}
			}
		}

		private UserControl CreateContainer(SlideElementContainerVm elem)
		{
			if (!this.Vm.Slide.DesignMode || this.Vm.ForceViewMode)
			{
				return new SlideElementContainerView(canvas, elem);
			}
			else
			{
				return new SlideElementEditContainerView(canvas, elem);
			}
		}

		private void UpdateDesignMode()
		{
			if (this.Vm.Slide.DesignMode)
			{
				if (_mdSelect == null)
				{
					_mdSelect = new MouseDrag(this, Cursors.Arrow, new MouseDragHandler(OnSelectDrag), OnEndSelectDrag);
				}
			}
			else
			{
				if (_mdSelect != null)
				{
					_mdSelect.UnRegisterEvents();
					_mdSelect = null;
				}
			}
		}

		#endregion // Methods
	}
}
