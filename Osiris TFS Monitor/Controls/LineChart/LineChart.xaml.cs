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
	using System.Collections;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Globalization;
	using System.Windows.Media.Effects;

	#endregion // Using
	
	/// <summary>
	/// Line Chart Control
	/// </summary>
	public partial class LineChart : UserControl
	{
		#region Properties

		#endregion // Properties

		#region Fields

		LineChartOptions _options; 
		Point _pointFactor;
		Point _origo;
		ObservableCollection<LineChartLegend> _legends = new ObservableCollection<LineChartLegend>();
		List<LineChartLine> _lines = new List<LineChartLine>();
		List<string> _xValues;
		bool _loaded = false;
		int _xValueIncr;

		double[] _roundedTick = 
		{
			0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.7, 0.75, 0.8, 0.9, 1.0
		};

		#endregion // Fields

		#region Constructors

		/// <summary>
		/// Construct and initialize
		/// </summary>
		public LineChart()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_options = new LineChartOptions();

			// Bind Legend-list to observable collection
			this.Legends.ItemsSource = _legends;

			_loaded = true;

			RefreshGraph();
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			// Clean up
			this.Legends.ItemsSource = null;
			_lines.Clear();
			_canvas.Children.Clear();
			_options = null;
			_legends = null;
		}

		public void Clear()
		{
			_legends.Clear();
			_lines.Clear();
			if (_xValues != null)
			{
				_xValues.Clear();
			}
		}

		/// <summary>
		/// Set description of x-values
		/// </summary>
		/// <param name="xValues"></param>
		public void SetXValues(List<string> xValues)
		{
			_xValues = xValues;
		}

		/// <summary>
		/// Add a line to the graph.
		/// </summary>
		/// <param name="chartData"></param>
		/// <param name="color"></param>
		public void AddLine(LineChartLine chartData)
		{
			// Add line
			_lines.Add(chartData);

			// Add legend
			if (!string.IsNullOrEmpty(chartData.Legend))
			{
				_legends.Add(new LineChartLegend(chartData.Legend, chartData.Color));
			}
		}

		/// <summary>
		/// Update chart and draw chartlines based on canvas size.
		/// </summary>
		public void RefreshGraph()
		{
			if (!_loaded)
			{
				return;
			}

			// Any lines?
			if (_lines.Count <= 0)
			{
				return;
			}

			// Clear previous lines from canvas
			_canvas.Children.Clear();

			// Calculate maxvalues
			Point maxValue = new Point(_lines.Max(l => (l.Count == 0) ? 0 : l.Max(p => p.X)), _lines.Max(l => (l.Count == 0) ? 0 : l.Max(p => p.Y)));

			// Calculate nescessary factors
			int numYTicks = 1;
			double tickSize = ScaleTick.GetTick(maxValue.Y, out numYTicks) / numYTicks;

			double fontHeight = GetTextMeasure("1").Height;
			double maxValueTextWidth = (_xValues == null) ? 0 : (_xValues.Max(v => GetTextMeasure(v).Width) / 2);

			for (int i = 1; i <= numYTicks; i++)
			{
				maxValueTextWidth = Math.Max(maxValueTextWidth, GetTextMeasure(((int)(i * tickSize)).ToString()).Width); 
			}
	
			
			Thickness margin = new Thickness(maxValueTextWidth + 6, (fontHeight + 1) / 2, ((maxValueTextWidth + 1) / 2) + 2, fontHeight + 1 + 5); 
			_pointFactor = new Point((_canvas.ActualWidth - margin.Left - margin.Right) / maxValue.X,
				(_canvas.ActualHeight - margin.Top - margin.Bottom) / (numYTicks * tickSize));

			// Hmmm
			if (double.IsInfinity(_pointFactor.X))
			{
				_pointFactor.X = 1;
			}
			if (double.IsInfinity(_pointFactor.Y))
			{
				_pointFactor.Y = 1;
			}

			_origo = new Point(margin.Left, _canvas.ActualHeight - margin.Bottom);
			double yTickHeight = tickSize * _pointFactor.Y;



			// Draw background y-ticks
			DrawChartBackgroundTicks(numYTicks, tickSize, yTickHeight, margin);

			// Draw y-ticks
			for (int i = 1; i <= numYTicks; i++)
			{
				DrawLine(_canvas, -5, i * yTickHeight, 1, i * yTickHeight, _options.AxisColor, _origo, 1);
				TextBlock tb = new TextBlock();
				tb.Foreground = _options.AxisColor;

				var textValue = ((decimal)(i * tickSize));
				var text = ((int)textValue).ToString();
				tb.Text = text;
				tb.FontFamily = _options.TextTypeface.FontFamily;
				tb.FontSize = _options.FontSize;
				tb.Margin = new Thickness(maxValueTextWidth - GetTextMeasure(text).Width - 5, _origo.Y - yTickHeight * i - ((fontHeight + 1) / 2), 0, 0);
				_canvas.Children.Add(tb);
			}

			// Draw x-ticks
			if (_xValues != null)
			{
				DrawXTitles(margin);
			}

			// Draw x- and y-axis
			DrawLine(_canvas, 0, 0, _canvas.ActualWidth - margin.Left - margin.Right, 0, _options.AxisColor, _origo, _options.AxisLineThickness.Height);
			DrawLine(_canvas, 0, 0, 0, _canvas.ActualHeight - margin.Top - margin.Bottom, _options.AxisColor, _origo, _options.AxisLineThickness.Width);

			// Draw lines
			foreach (LineChartLine line in _lines)
			{
				line.Draw(_canvas, _origo, _pointFactor, _xValueIncr);
			}

			// Draw legends
			Rectangle legends = new Rectangle();
		}

		private void DrawXTitles(Thickness margin)
		{
			string[] xValues = _xValues.ToArray();
			double xTickSize = (_canvas.ActualWidth - margin.Left - margin.Right) / (xValues.Count() - 1);

			if (double.IsInfinity(xTickSize))
			{
				xTickSize = 1;
			}

			ScaleXValues(xTickSize);

			UIElement prev = null;
			for (int i = 0; i < xValues.Count() + _xValueIncr - 1; i += _xValueIncr)
			{
				int i2 = Math.Min(xValues.Count() - 1, i);

				if (i > xValues.Count() - 1 && prev != null)
				{
					_canvas.Children.Remove(prev);
				}

				DrawLine(_canvas, i2 * xTickSize, 0, i2 * xTickSize, -5, _options.AxisColor, _origo, 1);
				TextBlock tb = new TextBlock();
				tb.FontFamily = _options.TextTypeface.FontFamily;
				tb.FontSize = _options.FontSize;
				tb.Foreground = _options.AxisColor;
				tb.Text = xValues[i2];
				tb.Margin = new Thickness(margin.Left + (i2 * xTickSize) - ((GetTextMeasure(xValues[i2]).Width + 1) / 2), _origo.Y + 6, 0, 0);
				_canvas.Children.Add(tb);
				prev = tb;
			}
		}

		private void ScaleXValues(double xTickSize)
		{
			// Don't draw every x-value unless there are room
			var maxWidth = _xValues.Max(v => GetTextMeasure(v).Width + 1);
			_xValueIncr = 1;
			while (maxWidth > xTickSize * _xValueIncr)
			{
				_xValueIncr = _xValueIncr * 2;
			}
		}

		/// <summary>
		/// Draw background y-ticks
		/// </summary>
		/// <param name="numYTicks"></param>
		/// <param name="tickSize"></param>
		/// <param name="margin"></param>
		/// <returns></returns>
		private void DrawChartBackgroundTicks(int numYTicks, double tickSize, double yTickHeight, Thickness margin)
		{
			for (int i = 0; i < (numYTicks - 1); i += 2)
			{
				Rectangle r = new Rectangle();
				r.Fill = _options.BackgroundTicksBrush;
				r.Opacity = 0.25;
				r.Width = Math.Max(0, _canvas.ActualWidth - margin.Left - margin.Right);
				r.Height = Math.Max(0, yTickHeight);
				r.Margin = new Thickness(margin.Left, _origo.Y - yTickHeight * (i + 2), 0, 0);
				_canvas.Children.Add(r);
			}
		}

		/// <summary>
		/// Measure text
		/// </summary>
		/// <param name="text">Text to measure</param>
		/// <returns></returns>
		private FormattedText GetTextMeasure(string text)
		{
			return new FormattedText(text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, 
				_options.TextTypeface, _options.FontSize, _options.TextColor); 
		}

		/// <summary>
		/// Draw chart-line from point A to B.
		/// </summary>
		public static void DrawLine(Canvas canvas, double x1, double y1, double x2, double y2, SolidColorBrush stroke, Point offset, double thickness)
		{
			Line line = new Line();
			line.Opacity = 1;
			line.Stroke = stroke;
			line.StrokeThickness = thickness;
			line.X1 = offset.X + x1;
			line.X2 = offset.X + x2;
			line.Y1 = offset.Y - y1;
			line.Y2 = offset.Y - y2;
			canvas.Children.Add(line);
		}

		/// <summary>
		/// Canvas resized, recalculate factors and update graph.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			RefreshGraph();
			_canvas.UpdateLayout();
		}

		#endregion // Methods
	}
}
