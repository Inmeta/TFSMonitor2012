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
	using Osiris.Tfs.Report;
	using System.Diagnostics;
	using Osiris.Tfs.Monitor.Models;
	using System.Windows.Media.Effects;
	using System.Windows.Media.Animation;
	using System.ComponentModel;
	using Microsoft.TeamFoundation.Server;

	#endregion // Using

	public partial class BuildMonitorView : UserControl, IBuildMonitorView, INotifyPropertyChanged
	{
		private ScrollViewer _buildScroller;

		#region Properties

		BuildMonitorVm ViewModel { get { return this.DataContext as BuildMonitorVm; } }

		public double PanelHeight { get { return (_itemsControl == null) ? 0 : _itemsControl.ActualHeight / this.ViewModel.Rows; } }

		#endregion // Properties

		#region Constructors

		public BuildMonitorView(BuildMonitorVm vm)
		{
			InitializeComponent();

			this.DataContext = vm;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Set viewmodel's view using property injection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.ViewModel.View = this;

			// Display buildlist
			_itemsControl.Visibility = Visibility.Visible;
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			if (this.ViewModel != null)
			{
				this.ViewModel.View = null;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;


		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{

			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs("PanelHeight"));
			}

			// Calculate buildlist in code instead of binding for optimization
			// ...
			//Debug.WriteLine("Size: " + e.NewSize.Width.ToString() + "x" + e.NewSize.Height.ToString());
			e.Handled = true;
		}
		
		#endregion // Methods

		private void Scroll(double startY, double endY, DateTime startTime, DateTime endTime)
		{
			double timeScrolled = endTime.Subtract(startTime).TotalSeconds;

			//if scrolling slowly, don't scroll with force
			if (timeScrolled < TIME_THRESHOLD)
			{
				double distanceScrolled = Math.Max(Math.Abs(endY - startY), MIN_DISTANCE);

				double velocity = distanceScrolled / timeScrolled;
				velocity = Math.Min(MAX_VELOCITY, velocity);
				int direction = 1;

				if (endY > startY)
				{
					direction = -1;
				}

				double timeToScroll = (velocity / DECELERATION) * SPEED_RATIO;

				double distanceToScroll = ((velocity * velocity) / (2 * DECELERATION)) * SPEED_RATIO;

				DoubleAnimation scrollAnimation = new DoubleAnimation();
				scrollAnimation.From = _buildScroller.VerticalOffset;
				scrollAnimation.To = _buildScroller.VerticalOffset + distanceToScroll * direction;
				scrollAnimation.DecelerationRatio = .9;
				scrollAnimation.SpeedRatio = SPEED_RATIO;
				scrollAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, Convert.ToInt32(timeToScroll), 0));
				this.BeginAnimation(BuildMonitorView.ScrollOffsetProperty, scrollAnimation);
			}
		}

		public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.Register("ScrollOffset", typeof(double), typeof(BuildMonitorView), new UIPropertyMetadata(BuildMonitorView.ScrollOffsetValueChanged));
		public double ScrollOffset
		{
			get { return (double)GetValue(ScrollOffsetProperty); }

			set { SetValue(ScrollOffsetProperty, value); }
		}

		private static void ScrollOffsetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			BuildMonitorView myClass = (BuildMonitorView)d;
			myClass._buildScroller.ScrollToVerticalOffset((double)e.NewValue);
		}

		
		#region Mouse Overrides
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{
			if (_buildScroller == null)
			{
				return;
			}

			mouseDragStartPoint = e.GetPosition(this);
			mouseDownTime = DateTime.Now;
			scrollStartOffset.X = _buildScroller.HorizontalOffset;
			scrollStartOffset.Y = _buildScroller.VerticalOffset;

			// Update the cursor if scrolling is possible 
			this.Cursor = (_buildScroller.ExtentWidth > _buildScroller.ViewportWidth) ||
				(_buildScroller.ExtentHeight > _buildScroller.ViewportHeight) ?
				Cursors.ScrollAll : Cursors.Arrow;

			this.CaptureMouse();
			base.OnPreviewMouseDown(e);
		}
		
		private Point mouseDragStartPoint;
		private DateTime mouseDownTime;
		private Point scrollStartOffset;
		private const double DECELERATION = 980;
		private const double SPEED_RATIO = .5;
		private const double MAX_VELOCITY = 2500;
		private const double MIN_DISTANCE = 0;
		private const double TIME_THRESHOLD = .4;

		
		protected override void OnPreviewMouseMove(MouseEventArgs e)
		{
			if (this.IsMouseCaptured)
			{
				// Get the new mouse position. 
				Point mouseDragCurrentPoint = e.GetPosition(this);

				// Determine the new amount to scroll. 
				Point delta = new Point(
					(mouseDragCurrentPoint.X > this.mouseDragStartPoint.X) ?
					-(mouseDragCurrentPoint.X - this.mouseDragStartPoint.X) :
					(this.mouseDragStartPoint.X - mouseDragCurrentPoint.X),
					(mouseDragCurrentPoint.Y > this.mouseDragStartPoint.Y) ?
					-(mouseDragCurrentPoint.Y - this.mouseDragStartPoint.Y) :
					(this.mouseDragStartPoint.Y - mouseDragCurrentPoint.Y));

				// Scroll to the new position. 
				_buildScroller.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
				_buildScroller.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
			}
			base.OnPreviewMouseMove(e);
		}

		protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
		{
			if (this.IsMouseCaptured)
			{
				this.Cursor = Cursors.Arrow;
				this.ReleaseMouseCapture();
			}

			Scroll(mouseDragStartPoint.Y, e.GetPosition(this).Y, mouseDownTime, DateTime.Now);

			base.OnPreviewMouseUp(e);
		}
		

		#endregion

		private void _buildScroller_Loaded(object sender, RoutedEventArgs e)
		{
			_buildScroller = sender as ScrollViewer;
		}
		

	}

	public class ScaleHeightConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			double doubleValue = (double)value;
			double doubleParameter = (double)parameter;
			return Math.Min(1, doubleValue * doubleParameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return this.Convert(value, targetType, parameter, culture);
		}

		#endregion // IValueConverter Members
	}
}
